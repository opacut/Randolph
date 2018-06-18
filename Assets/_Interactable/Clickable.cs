using cakeslice;
using Randolph.Levels;
using Randolph.UI;
using UnityEngine;
using static Randolph.Core.Constants;

namespace Randolph.Interactable {
    [RequireComponent(typeof(Outline))]
    [RequireComponent(typeof(SpriteRenderer))]
    public abstract class Clickable : MonoBehaviour, IRestartable {
        [Tooltip("Randolph's comment - keep empty if none.")]
        [SerializeField, TextArea]
        private string description;

        protected Outline outline;
        protected bool shouldOutline;

        protected SpriteRenderer spriteRenderer;

        /// <summary>Type of cursor to use. Override in a derived class.</summary>
        public abstract Cursors CursorType { get; protected set; }

        protected virtual void Awake() {
            outline = GetComponent<Outline>();
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        protected virtual void Start() {
            shouldOutline = false;
            SaveState();
        }

        protected virtual void Update() {
            if (spriteRenderer.enabled) {
                outline.enabled = Input.GetAxis("Highlight") != 0.0f || shouldOutline;
            }
        }

        private void OnDestroy() {
            // Mouse Up + Mouse Exit when deleted
            ResetCursor();
        }

        protected void ResetCursor() => CursorManager.cursorManager.SetCursorDefault();

        /// <summary>Randolph's comment.</summary>
        public string GetDescription() => description.Trim();

        #region MouseEvents
        public delegate void MouseDownClickable(Clickable target, MouseButton button);

        public delegate void MouseEnterClickable(Clickable target);

        public delegate void MouseExitClickable(Clickable target);

        public delegate void MouseUpClickable(Clickable target, MouseButton button);

        public static event MouseEnterClickable OnMouseEnterClickable;
        public static event MouseExitClickable OnMouseExitClickable;
        public static event MouseDownClickable OnMouseDownClickable;
        public static event MouseUpClickable OnMouseUpClickable;

        protected virtual void OnMouseEnter() {
            OnMouseEnterClickable?.Invoke(this);
            shouldOutline = true;
        }

        protected virtual void OnMouseExit() {
            OnMouseExitClickable?.Invoke(this);
            shouldOutline = false;
        }

        protected virtual void OnMouseOver() {
            for (var i = 0; i <= 2; i++) {
                // Check for all mouse buttons
                if (Input.GetMouseButtonDown(i)) {
                    OnMouseDownClickable?.Invoke(this, (MouseButton) i);
                } else if (Input.GetMouseButtonUp(i)) {
                    OnMouseUpClickable?.Invoke(this, (MouseButton) i);
                }
            }
        }
        #endregion

        #region IRestartable
        private bool initialActiveState;
        protected Vector3 initialPosition { get; private set; }
        private Quaternion initialRotation;

        public virtual void SaveState() {
            initialActiveState = gameObject.activeSelf;
            initialPosition = transform.position;
            initialRotation = transform.rotation;
        }

        public virtual void Restart() {
            gameObject.SetActive(initialActiveState);
            transform.position = initialPosition;
            transform.rotation = initialRotation;
        }
        #endregion
    }
}
