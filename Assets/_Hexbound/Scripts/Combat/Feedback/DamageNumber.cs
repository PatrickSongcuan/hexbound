using TMPro;
using UnityEngine;
using UnityEngine.Pool;

namespace Hexbound.Combat
{
    /// <summary>One floating number: rises, fades, returns itself to the pool.</summary>
    [RequireComponent(typeof(TextMeshPro))]
    public class DamageNumber : MonoBehaviour
    {
        private const float HorizontalJitter = 0.25f;

        private TextMeshPro text;
        private IObjectPool<DamageNumber> pool;
        private GameFeelData feel;
        private Color baseColor;
        private float age;

        private void Awake() => text = GetComponent<TextMeshPro>();

        public void Init(IObjectPool<DamageNumber> owningPool, GameFeelData feelData)
        {
            pool = owningPool;
            feel = feelData;
        }

        public void Show(float amount, Vector3 position, Color color)
        {
            position.x += Random.Range(-HorizontalJitter, HorizontalJitter);
            transform.position = position;
            text.text = Mathf.RoundToInt(amount).ToString();
            baseColor = color;
            text.color = color;
            age = 0f;
        }

        private void Update()
        {
            age += Time.deltaTime;
            transform.position += Vector3.up * (feel.damageNumberRiseSpeed * Time.deltaTime);

            float t = age / feel.damageNumberLifetime;
            Color c = baseColor;
            c.a = 1f - t;
            text.color = c;

            if (t >= 1f)
                pool.Release(this);
        }
    }
}
