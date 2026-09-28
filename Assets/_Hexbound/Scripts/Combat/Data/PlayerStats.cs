using UnityEngine;

namespace Hexbound.Combat
{
    [CreateAssetMenu(menuName = "Hexbound/Player Stats", fileName = "PlayerStats")]
    public class PlayerStats : ScriptableObject
    {
        [Min(0f)] public float moveSpeed = 6f;
        [Tooltip("Units/s² toward target velocity. Higher = snappier, lower = floatier.")]
        [Min(0f)] public float acceleration = 60f;
        [Min(1f)] public float maxHealth = 100f;

        [Header("Dash")]
        [Tooltip("Units/s while dashing. Distance = dashSpeed × dashDuration.")]
        [Min(0f)] public float dashSpeed = 12f;
        [Tooltip("Seconds the dash lasts. The player passes through enemies during it.")]
        [Min(0.01f)] public float dashDuration = 0.15f;
        [Tooltip("Seconds after a dash ends before the next one is allowed.")]
        [Min(0f)] public float dashCooldown = 1f;
        [Tooltip("Extra invulnerable seconds after the dash ends, so landing next to an enemy doesn't hurt.")]
        [Min(0f)] public float dashGraceTime = 0.2f;
    }
}
