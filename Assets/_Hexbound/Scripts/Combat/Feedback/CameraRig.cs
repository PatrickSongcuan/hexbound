using UnityEngine;

namespace Hexbound.Combat
{
    /// <summary>
    /// Smooth-follows the target and layers trauma-based screen shake on top.
    /// Shake uses unscaled time so it keeps moving during hit-stop.
    /// </summary>
    [RequireComponent(typeof(Camera))]
    public class CameraRig : MonoBehaviour
    {
        [SerializeField] private Transform target;
        [SerializeField] private GameFeelData feel;

        private Vector3 followPosition;
        private Vector3 followVelocity;
        private float trauma;
        private float noiseSeed;

        private void Awake()
        {
            followPosition = transform.position;
            noiseSeed = Random.value * 100f;
        }

        private void OnEnable() => Health.AnyDamaged += OnAnyDamaged;
        private void OnDisable() => Health.AnyDamaged -= OnAnyDamaged;

        public void AddTrauma(float amount) => trauma = Mathf.Clamp01(trauma + amount);

        private void OnAnyDamaged(Health victim, float amount, Vector3 position)
        {
            AddTrauma(victim.Team == Team.Player ? feel.playerHitShake : feel.enemyHitShake);
        }

        private void LateUpdate()
        {
            if (target != null)
            {
                Vector3 goal = new Vector3(target.position.x, target.position.y, followPosition.z);
                followPosition = Vector3.SmoothDamp(followPosition, goal, ref followVelocity, feel.followSmoothTime);
            }

            trauma = Mathf.Max(0f, trauma - feel.shakeDecay * Time.unscaledDeltaTime);
            float shake = trauma * trauma;
            float t = Time.unscaledTime * feel.shakeFrequency;
            Vector3 offset = new Vector3(
                Mathf.PerlinNoise(noiseSeed, t) * 2f - 1f,
                Mathf.PerlinNoise(noiseSeed + 1f, t) * 2f - 1f,
                0f) * (feel.shakeMaxOffset * shake);

            transform.position = followPosition + offset;
        }
    }
}
