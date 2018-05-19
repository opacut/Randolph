using UnityEngine;
using Randolph.UI;
using static Randolph.Core.Constants;

namespace Randolph.Interactable {

    public abstract class Clickable : MonoBehaviour {

        /// <summary>Type of cursor to use. Override in a derived class.</summary>
        public abstract Cursors CursorType { get; protected set; }

        public delegate void MouseEnterClickable(Cursors cursorType, Vector2 position);
        public static event MouseEnterClickable OnMouseEnterClickable;

        public delegate void MouseExitClickable(Cursors cursorType, Vector2 position);
        public static event MouseExitClickable OnMouseExitClickable;

        public delegate void MouseDownClickable(Cursors cursorType, MouseButton button, Vector2 position);
        public static event MouseDownClickable OnMouseDownClickable;

        public delegate void MouseUpClickable(Cursors cursorType, MouseButton button, Vector2 position);
        public static event MouseUpClickable OnMouseUpClickable;

        protected void ResetCursor() {
            CursorManager.cursorManager.SetCursorDefault();
        }

        void OnMouseEnter() {
            OnMouseEnterClickable?.Invoke(CursorType, transform.position);
        }

        void OnMouseExit() {
            OnMouseExitClickable?.Invoke(CursorType, transform.position);
        }

        void OnMouseOver() {
            for (int i = 0; i <= 2; i++) {
                // Check for all mouse buttons
                if (Input.GetMouseButtonDown(i)) OnMouseDownClickable?.Invoke(CursorType, (MouseButton) i, transform.position);
                else if (Input.GetMouseButtonUp(i)) OnMouseUpClickable?.Invoke(CursorType, (MouseButton) i, transform.position);
            }
        }

        void OnDestroy() {
            // Mouse Up + Mouse Exit when deleted
            ResetCursor();
        }
        
    }
}
