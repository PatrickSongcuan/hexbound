using UnityEngine;
using UnityEngine.Pool;

namespace Hexbound.Combat
{
    /// <summary>Auto-casts its spell at the nearest enemy whenever the player stands still.</summary>
    [RequireComponent(typeof(PlayerController))]
    public class SpellCaster : MonoBehaviour
    {
        [SerializeField] private SpellData spell;

        private PlayerController player;
        private ObjectPool<Projectile> pool;
        private float cooldown;

        private void Awake()
        {
            player = GetComponent<PlayerController>();
            pool = new ObjectPool<Projectile>(
                createFunc: CreateProjectile,
                actionOnGet: p => p.gameObject.SetActive(true),
                actionOnRelease: p => p.gameObject.SetActive(false),
                actionOnDestroy: p => { if (p != null) Destroy(p.gameObject); },
                collectionCheck: true,
                defaultCapacity: 16,
                maxSize: 64);
        }

        private void OnDestroy() => pool?.Dispose();

        private void Update()
        {
            // Cooldown ticks even while moving, so stopping gives an instant shot if it's ready.
            cooldown = Mathf.Max(0f, cooldown - Time.deltaTime);
            if (cooldown > 0f || player.IsMoving || player.IsDead)
                return;

            Health target = FindNearestEnemy();
            if (target == null)
                return;

            Fire(target);
            cooldown = spell.fireInterval;
        }

        private void Fire(Health target)
        {
            Vector2 origin = transform.position;
            Vector2 direction = ((Vector2)target.transform.position - origin).normalized;
            pool.Get().Launch(origin, direction, spell, Team.Player);
        }

        private Health FindNearestEnemy()
        {
            Vector2 origin = transform.position;
            float bestSqr = spell.range * spell.range;
            Health best = null;

            foreach (Health enemy in Health.Living(Team.Enemy))
            {
                float sqr = ((Vector2)enemy.transform.position - origin).sqrMagnitude;
                if (sqr <= bestSqr)
                {
                    bestSqr = sqr;
                    best = enemy;
                }
            }

            return best;
        }

        private Projectile CreateProjectile()
        {
            Projectile p = Instantiate(spell.projectilePrefab);
            p.SetPool(pool);
            return p;
        }
    }
}
