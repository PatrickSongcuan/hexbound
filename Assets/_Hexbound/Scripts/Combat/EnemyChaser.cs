using UnityEngine;

namespace Hexbound.Combat
{
    /// <summary>Test enemy: walks straight at the player and deals contact damage.</summary>
    [RequireComponent(typeof(Rigidbody2D), typeof(Health))]
    public class EnemyChaser : MonoBehaviour
    {
        [SerializeField] private EnemyData data;

        private Rigidbody2D body;
        private Health health;
        private float nextContactTime;

        private void Awake()
        {
            body = GetComponent<Rigidbody2D>();
            body.gravityScale = 0f;
            body.freezeRotation = true;
            health = GetComponent<Health>();
            health.Init(data.maxHealth);
            health.Died += () => Destroy(gameObject);
        }

        private void FixedUpdate()
        {
            var players = Health.Living(Team.Player);
            if (players.Count == 0)
            {
                body.linearVelocity = Vector2.zero;
                return;
            }

            Vector2 toPlayer = (Vector2)players[0].transform.position - body.position;
            body.linearVelocity = toPlayer.normalized * data.moveSpeed;
        }

        private void OnCollisionStay2D(Collision2D collision)
        {
            if (Time.time < nextContactTime)
                return;
            if (!collision.collider.TryGetComponent(out Health target) || target.Team == health.Team || target.IsDead)
                return;

            target.TakeDamage(data.contactDamage, collision.GetContact(0).point);
            nextContactTime = Time.time + data.contactInterval;
        }
    }
}
