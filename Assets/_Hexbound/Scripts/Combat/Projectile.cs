using UnityEngine;
using UnityEngine.Pool;

namespace Hexbound.Combat
{
    /// <summary>Straight-flying pooled projectile. Kinematic body + trigger collider.</summary>
    [RequireComponent(typeof(Rigidbody2D))]
    public class Projectile : MonoBehaviour
    {
        private IObjectPool<Projectile> pool;
        private Rigidbody2D body;
        private Team owner;
        private float damage;
        private float lifeLeft;
        private bool released;

        public void SetPool(IObjectPool<Projectile> owningPool) => pool = owningPool;

        private void Awake()
        {
            body = GetComponent<Rigidbody2D>();
            body.bodyType = RigidbodyType2D.Kinematic;
            body.gravityScale = 0f;
        }

        public void Launch(Vector2 position, Vector2 direction, SpellData spell, Team ownerTeam)
        {
            transform.position = position;
            body.position = position;
            body.linearVelocity = direction * spell.projectileSpeed;
            owner = ownerTeam;
            damage = spell.damage;
            lifeLeft = spell.lifetime;
            released = false;
        }

        private void Update()
        {
            lifeLeft -= Time.deltaTime;
            if (lifeLeft <= 0f)
                Release();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (released || !other.TryGetComponent(out Health target))
                return;
            if (target.Team == owner || target.IsDead)
                return;

            target.TakeDamage(damage, transform.position);
            Release();
        }

        // Guard flag: a projectile can touch two enemies in one physics step; releasing twice throws.
        private void Release()
        {
            if (released)
                return;
            released = true;

            if (pool != null)
                pool.Release(this);
            else
                Destroy(gameObject);
        }
    }
}
