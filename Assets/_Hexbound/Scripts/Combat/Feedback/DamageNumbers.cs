using UnityEngine;
using UnityEngine.Pool;

namespace Hexbound.Combat
{
    /// <summary>Listens to every hit and pops a pooled damage number at the hit point.</summary>
    public class DamageNumbers : MonoBehaviour
    {
        [SerializeField] private DamageNumber prefab;
        [SerializeField] private GameFeelData feel;

        private ObjectPool<DamageNumber> pool;

        private void Awake()
        {
            pool = new ObjectPool<DamageNumber>(
                createFunc: () =>
                {
                    DamageNumber n = Instantiate(prefab, transform);
                    n.Init(pool, feel);
                    return n;
                },
                actionOnGet: n => n.gameObject.SetActive(true),
                actionOnRelease: n => n.gameObject.SetActive(false),
                actionOnDestroy: n => { if (n != null) Destroy(n.gameObject); },
                collectionCheck: true,
                defaultCapacity: 16,
                maxSize: 64);
        }

        private void OnEnable() => Health.AnyDamaged += OnAnyDamaged;
        private void OnDisable() => Health.AnyDamaged -= OnAnyDamaged;

        private void OnAnyDamaged(Health victim, float amount, Vector3 position)
        {
            Color color = victim.Team == Team.Player ? feel.playerDamageColor : feel.enemyDamageColor;
            pool.Get().Show(amount, position, color);
        }
    }
}
