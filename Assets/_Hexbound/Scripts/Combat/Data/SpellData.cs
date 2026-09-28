using UnityEngine;

namespace Hexbound.Combat
{
    /// <summary>Tuning for one auto-cast spell. Edit the asset in Play mode to tune live.</summary>
    [CreateAssetMenu(menuName = "Hexbound/Spell Data", fileName = "SpellData")]
    public class SpellData : ScriptableObject
    {
        [Min(0f)] public float damage = 10f;
        [Tooltip("Seconds between casts.")]
        [Min(0.05f)] public float fireInterval = 0.6f;
        [Min(0f)] public float projectileSpeed = 14f;
        [Tooltip("Seconds before an unhit projectile returns to the pool.")]
        [Min(0.1f)] public float lifetime = 1.5f;
        [Tooltip("Only enemies within this distance are targeted.")]
        [Min(0f)] public float range = 9f;
        public Projectile projectilePrefab;
    }
}
