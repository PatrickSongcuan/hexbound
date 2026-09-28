using UnityEngine;

namespace Hexbound.Combat
{
    /// <summary>"Juice" knobs: hit-stop, screen shake, camera follow, damage numbers.</summary>
    [CreateAssetMenu(menuName = "Hexbound/Game Feel Data", fileName = "GameFeel")]
    public class GameFeelData : ScriptableObject
    {
        [Header("Hit-stop")]
        [Tooltip("Real-time seconds the game freezes on a hit. 0 disables.")]
        [Min(0f)] public float hitStopDuration = 0.05f;

        [Header("Screen shake (trauma 0..1, shake = trauma²)")]
        [Range(0f, 1f)] public float enemyHitShake = 0.25f;
        [Range(0f, 1f)] public float playerHitShake = 0.6f;
        [Tooltip("Trauma lost per second.")]
        [Min(0f)] public float shakeDecay = 1.5f;
        [Tooltip("World units of offset at full trauma.")]
        [Min(0f)] public float shakeMaxOffset = 0.35f;
        [Min(0f)] public float shakeFrequency = 25f;

        [Header("Camera follow")]
        [Min(0f)] public float followSmoothTime = 0.12f;

        [Header("Damage numbers")]
        [Min(0f)] public float damageNumberRiseSpeed = 1.5f;
        [Min(0.05f)] public float damageNumberLifetime = 0.6f;
        public Color enemyDamageColor = Color.white;
        public Color playerDamageColor = new Color(1f, 0.35f, 0.35f);
    }
}
