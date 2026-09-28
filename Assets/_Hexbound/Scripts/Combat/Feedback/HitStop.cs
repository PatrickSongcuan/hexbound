using System.Collections;
using UnityEngine;

namespace Hexbound.Combat
{
    /// <summary>Freezes time for a few real-time milliseconds on every hit. Overlapping hits extend, never stack.</summary>
    public class HitStop : MonoBehaviour
    {
        [SerializeField] private GameFeelData feel;

        private float stopUntil;
        private bool stopping;

        private void OnEnable() => Health.AnyDamaged += OnAnyDamaged;

        private void OnDisable()
        {
            Health.AnyDamaged -= OnAnyDamaged;
            if (stopping)
            {
                Time.timeScale = 1f;
                stopping = false;
            }
        }

        private void OnAnyDamaged(Health victim, float amount, Vector3 position)
        {
            if (feel.hitStopDuration <= 0f)
                return;

            stopUntil = Mathf.Max(stopUntil, Time.unscaledTime + feel.hitStopDuration);
            if (!stopping)
                StartCoroutine(Freeze());
        }

        private IEnumerator Freeze()
        {
            stopping = true;
            Time.timeScale = 0f;
            while (Time.unscaledTime < stopUntil)
                yield return null;
            Time.timeScale = 1f;
            stopping = false;
        }
    }
}
