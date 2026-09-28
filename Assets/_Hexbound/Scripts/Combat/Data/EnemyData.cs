using UnityEngine;

namespace Hexbound.Combat
{
    [CreateAssetMenu(menuName = "Hexbound/Enemy Data", fileName = "EnemyData")]
    public class EnemyData : ScriptableObject
    {
        [Min(1f)] public float maxHealth = 30f;
        [Min(0f)] public float moveSpeed = 2.2f;
        [Min(0f)] public float contactDamage = 10f;
        [Tooltip("Seconds between contact hits while touching the player.")]
        [Min(0.05f)] public float contactInterval = 0.8f;
    }
}
