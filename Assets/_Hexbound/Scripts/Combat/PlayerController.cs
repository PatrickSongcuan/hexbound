using System.Collections;
using Hexbound.UI;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace Hexbound.Combat
{
    [RequireComponent(typeof(Rigidbody2D), typeof(Collider2D), typeof(Health))]
    public class PlayerController : MonoBehaviour
    {
        private const float MoveThreshold = 0.1f;
        private const float RestartDelay = 1f;

        [SerializeField] private PlayerStats stats;
        [SerializeField] private VirtualJoystick joystick;

        private Rigidbody2D body;
        private Collider2D bodyCollider;
        private Health health;
        private Vector2 moveInput;

        // Dash timers count down to 0 in Update.
        private float dashTimeLeft;
        private float dashCooldownLeft;
        private float invulnerabilityTimeLeft;

        public bool IsMoving { get; private set; }
        public bool IsDead => health.IsDead;
        public bool IsDashing => dashTimeLeft > 0f;
        public Vector2 LastMoveDirection { get; private set; } = Vector2.up;

        private void Awake()
        {
            body = GetComponent<Rigidbody2D>();
            body.gravityScale = 0f;
            bodyCollider = GetComponent<Collider2D>();
            health = GetComponent<Health>();
            health.Init(stats.maxHealth);
            health.Died += OnDied;
        }

        private void Update()
        {
            moveInput = ReadInput();
            IsMoving = moveInput.sqrMagnitude > MoveThreshold * MoveThreshold;
            if (IsMoving)
                LastMoveDirection = moveInput.normalized;

            UpdateDash();
        }

        private void UpdateDash()
        {
            dashTimeLeft = Mathf.Max(0f, dashTimeLeft - Time.deltaTime);
            invulnerabilityTimeLeft = Mathf.Max(0f, invulnerabilityTimeLeft - Time.deltaTime);

            // Cooldown only starts once the dash itself has finished.
            if (!IsDashing)
                dashCooldownLeft = Mathf.Max(0f, dashCooldownLeft - Time.deltaTime);

            if (DashPressed() && !IsDashing && dashCooldownLeft <= 0f && !IsDead)
            {
                dashTimeLeft = stats.dashDuration;
                dashCooldownLeft = stats.dashCooldown;
                invulnerabilityTimeLeft = stats.dashDuration + stats.dashGraceTime;
            }

            // Compute state first, then publish it: pass through enemies while dashing,
            // stay invulnerable a little longer (grace time) so landing next to one doesn't hurt.
            health.IsInvulnerable = invulnerabilityTimeLeft > 0f;
            bodyCollider.isTrigger = IsDashing;
        }

        private static bool DashPressed()
        {
            var kb = Keyboard.current;
            return kb != null && kb.spaceKey.wasPressedThisFrame;
        }

        private void FixedUpdate()
        {
            if (IsDead)
                return;

            if (IsDashing)
            {
                body.linearVelocity = LastMoveDirection * stats.dashSpeed;
                return;
            }

            Vector2 target = moveInput * stats.moveSpeed;
            body.linearVelocity = Vector2.MoveTowards(body.linearVelocity, target, stats.acceleration * Time.fixedDeltaTime);
        }

        private Vector2 ReadInput()
        {
            if (IsDead)
                return Vector2.zero;

            Vector2 dir = joystick != null ? joystick.Direction : Vector2.zero;

            // Editor convenience: WASD / arrows when the joystick is idle.
            var kb = Keyboard.current;
            if (dir == Vector2.zero && kb != null)
            {
                if (kb.wKey.isPressed || kb.upArrowKey.isPressed) dir.y += 1f;
                if (kb.sKey.isPressed || kb.downArrowKey.isPressed) dir.y -= 1f;
                if (kb.dKey.isPressed || kb.rightArrowKey.isPressed) dir.x += 1f;
                if (kb.aKey.isPressed || kb.leftArrowKey.isPressed) dir.x -= 1f;
            }

            return Vector2.ClampMagnitude(dir, 1f);
        }

        private void OnDied()
        {
            body.linearVelocity = Vector2.zero;
            StartCoroutine(RestartAfterDelay());
        }

        private IEnumerator RestartAfterDelay()
        {
            yield return new WaitForSeconds(RestartDelay);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
