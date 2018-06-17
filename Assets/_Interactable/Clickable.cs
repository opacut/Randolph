using cakeslice;
using Randolph.Levels;
using Randolph.UI;
using UnityEngine;
using static Randolph.Core.Constants;

namespace Randolph.Interactable {
    [RequireComponent(typeof(Outline))]
    public abstract class Clickable : MonoBehaviour, IRestartable {
        public delegate void MouseDownClickable(Clickable target, MouseButton button);

        public delegate void MouseEnterClickable(Clickable target);

        public delegate void MouseExitClickable(Clickable target);

        public delegate void MouseUpClickable(Clickable target, MouseButton button);

        [Tooltip("Randolph's comment - keep empty if none.")][SerializeField][TextArea]
        private string description;

        private Vector3 initialPosition;
        private Quaternion initialRotation;

        protected Outline outline;
        private bool shouldOutline;

        /// <summary>Type of cursor to use. Override in a derived class.</summary>
        public abstract Cursors CursorType { get; protected set; }

        public virtual void Restart()
        {
            transform.position = initialPosition;
            transform.rotation = initialRotation;
        }

        public static event MouseEnterClickable OnMouseEnterClickable;
        public static event MouseExitClickable OnMouseExitClickable;
        public static event MouseDownClickable OnMouseDownClickable;
        public static event MouseUpClickable OnMouseUpClickable;

        protected void ResetCursor() { CursorManager.cursorManager.SetCursorDefault(); }

        protected virtual void Awake() {
            initialPosition = transform.position;
            initialRotation = transform.rotation;
            outline = GetComponent<Outline>();
        }

        protected virtual void Start() { shouldOutline = false; }

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

        protected void Update() {
            outline.enabled = Input.GetAxis("Highlight") != 0.0f || shouldOutline;
        }

        private void OnDestroy() {
            // Mouse Up + Mouse Exit when deleted
            ResetCursor();
        }

        /// <summary>Randolph's comment.</summary>
        public string GetDescription() { return description.Trim(); }
    }
}
