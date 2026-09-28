using UnityEngine;
using UnityEngine.InputSystem;

namespace Hexbound.UI
{
    /// <summary>
    /// Floating joystick: appears wherever the finger (or mouse) goes down.
    /// Reads Pointer.current, which is the mouse in the Editor and the touchscreen on a phone.
    /// Must sit on a Screen Space - Overlay canvas; the base is anchored to the canvas center.
    /// </summary>
    public class VirtualJoystick : MonoBehaviour
    {
        [SerializeField] private RectTransform background;
        [SerializeField] private RectTransform knob;
        [Tooltip("Max knob travel in canvas units (reference 1080x1920).")]
        [SerializeField] private float radius = 110f;
        [Range(0f, 0.5f)]
        [SerializeField] private float deadZone = 0.1f;

        private RectTransform canvasRect;
        private Vector2 origin;
        private bool held;

        /// <summary>-1..1 on each axis, magnitude ≤ 1. Zero when released or inside the dead zone.</summary>
        public Vector2 Direction { get; private set; }

        private void Awake()
        {
            canvasRect = (RectTransform)GetComponentInParent<Canvas>().transform;
            background.gameObject.SetActive(false);
        }

        private void Update()
        {
            Pointer pointer = Pointer.current;
            if (pointer == null)
                return;

            Vector2 local = ScreenToCanvas(pointer.position.ReadValue());

            if (pointer.press.wasPressedThisFrame)
            {
                held = true;
                origin = local;
                background.anchoredPosition = origin;
                knob.anchoredPosition = Vector2.zero;
                background.gameObject.SetActive(true);
            }

            if (held && pointer.press.isPressed)
            {
                Vector2 offset = Vector2.ClampMagnitude(local - origin, radius);
                knob.anchoredPosition = offset;
                Vector2 dir = offset / radius;
                Direction = dir.magnitude < deadZone ? Vector2.zero : dir;
            }
            else if (held)
            {
                held = false;
                Direction = Vector2.zero;
                background.gameObject.SetActive(false);
            }
        }

        private Vector2 ScreenToCanvas(Vector2 screen)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, screen, null, out Vector2 local);
            return local;
        }
    }
}
