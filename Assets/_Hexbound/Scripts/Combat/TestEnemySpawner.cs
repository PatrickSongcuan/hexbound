using UnityEngine;

namespace Hexbound.Combat
{
    /// <summary>Prototype-only: keeps enemies coming so the combat loop can be judged for fun.</summary>
    public class TestEnemySpawner : MonoBehaviour
    {
        [SerializeField] private TestWaveData wave;

        private float timer;

        private void Update()
        {
            var players = Health.Living(Team.Player);
            if (players.Count == 0)
                return;

            timer -= Time.deltaTime;
            if (timer > 0f)
                return;
            timer = wave.spawnInterval;

            if (Health.Living(Team.Enemy).Count >= wave.maxAlive)
                return;

            float angle = Random.Range(0f, Mathf.PI * 2f);
            Vector2 offset = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * wave.spawnRadius;
            Vector2 position = (Vector2)players[0].transform.position + offset;
            Instantiate(wave.enemyPrefab, position, Quaternion.identity);
        }
    }
}
