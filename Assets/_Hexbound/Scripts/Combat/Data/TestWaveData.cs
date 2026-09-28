using UnityEngine;

namespace Hexbound.Combat
{
    /// <summary>Prototype-only endless spawner settings for the M1 "fun test".</summary>
    [CreateAssetMenu(menuName = "Hexbound/Test Wave Data", fileName = "TestWave")]
    public class TestWaveData : ScriptableObject
    {
        public EnemyChaser enemyPrefab;
        [Min(0.05f)] public float spawnInterval = 1.2f;
        [Min(1)] public int maxAlive = 20;
        [Tooltip("Enemies appear on a ring this far from the player.")]
        [Min(1f)] public float spawnRadius = 9f;
    }
}
