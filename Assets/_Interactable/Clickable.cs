using Randolph.Levels;
using UnityEngine;
using Randolph.UI;
using cakeslice;
using static Randolph.Core.Constants;

namespace Randolph.Interactable {
    
    [RequireComponent(typeof(Outline))]
    public abstract class Clickable : MonoBehaviour, IRestartable {

        /// <summary>Type of cursor to use. Override in a derived class.</summary>
        public abstract Cursors CursorType { get; protected set; }

        protected Outline outline;

        [Tooltip("Randolph's comment - keep empty if none.")]
        [SerializeField, TextArea] string description;

        Vector3 initialPosition;

        public delegate void MouseEnterClickable(Clickable target);
        public static event MouseEnterClickable OnMouseEnterClickable;

        public delegate void MouseExitClickable(Clickable target);
        public static event MouseExitClickable OnMouseExitClickable;

        public delegate void MouseDownClickable(Clickable target, MouseButton button);
        public static event MouseDownClickable OnMouseDownClickable;

        public delegate void MouseUpClickable(Clickable target, MouseButton button);
        public static event MouseUpClickable OnMouseUpClickable;

        protected void ResetCursor() {
            CursorManager.cursorManager.SetCursorDefault();
        }

        protected virtual void Awake() {
            initialPosition = transform.position;
            outline = GetComponent<Outline>();
        }

        protected virtual void Start() {
            outline.enabled = false;
        }

        public virtual void Restart() {
            transform.position = initialPosition;            
        }

        protected virtual void OnMouseEnter() {
            OnMouseEnterClickable?.Invoke(this);
            outline.enabled = true;
        }

        protected virtual void OnMouseExit() {
            OnMouseExitClickable?.Invoke(this);
            outline.enabled = false;
        }

        protected virtual void OnMouseOver() {
            for (int i = 0; i <= 2; i++) {
                // Check for all mouse buttons
                if (Input.GetMouseButtonDown(i)) OnMouseDownClickable?.Invoke(this, (MouseButton) i);
                else if (Input.GetMouseButtonUp(i)) OnMouseUpClickable?.Invoke(this, (MouseButton) i);
            }
        }

        void OnDestroy() {
            // Mouse Up + Mouse Exit when deleted
            ResetCursor();
        }

        /// <summary>Randolph's comment.</summary>
        public string GetDescription() {
            return description.Trim();
        }
    }
}
