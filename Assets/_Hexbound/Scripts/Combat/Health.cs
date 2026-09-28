using System;
using System.Collections.Generic;
using UnityEngine;

namespace Hexbound.Combat
{
    /// <summary>
    /// Shared by player and enemies. Keeps a static per-team registry of living units
    /// so targeting never needs tags or FindObjectsByType.
    /// </summary>
    public class Health : MonoBehaviour
    {
        private static readonly Dictionary<Team, List<Health>> living = new();

        /// <summary>Fired for every hit on any unit: (target, amount, hit position). Feedback systems listen here.</summary>
        public static event Action<Health, float, Vector3> AnyDamaged;

        [SerializeField] private Team team;

        public Team Team => team;
        public float Max { get; private set; }
        public float Current { get; private set; }
        public bool IsDead => Current <= 0f;
        public bool IsInvulnerable { get; set; }

        public event Action<float> Damaged;
        public event Action Died;

        public static IReadOnlyList<Health> Living(Team team) => ListFor(team);

        /// <summary>Called by the owner (PlayerController / EnemyChaser) with its data-driven max.</summary>
        public void Init(float max)
        {
            Max = max;
            Current = max;
        }

        private void OnEnable() => ListFor(team).Add(this);
        private void OnDisable() => ListFor(team).Remove(this);

        public void TakeDamage(float amount, Vector3 hitPosition)
        {
            if (IsDead || amount <= 0f || IsInvulnerable)
                return;

            Current = Mathf.Max(0f, Current - amount);
            Damaged?.Invoke(amount);
            AnyDamaged?.Invoke(this, amount, hitPosition);

            if (IsDead)
            {
                ListFor(team).Remove(this);
                Died?.Invoke();
            }
        }

        private static List<Health> ListFor(Team t)
        {
            if (!living.TryGetValue(t, out var list))
                living[t] = list = new List<Health>();
            return list;
        }

        // Statics survive Play mode when "Enter Play Mode Options" skips domain reload; reset them explicitly.
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void ResetStatics()
        {
            living.Clear();
            AnyDamaged = null;
        }
    }
}
