using Randolph.Core;
using Randolph.Levels;
using UnityEngine;

namespace Randolph.Interactable {
    public abstract class Pickable : MonoBehaviour, IPickable {

        Vector3 initialPosition;
        public abstract bool isSingleUse { get; }

        public delegate void MouseEnterPickable();
        public static event MouseEnterPickable OnMouseEnterPickable;

        public delegate void MouseExitPickable();
        public static event MouseExitPickable OnMouseExitPickable;

        public delegate void MouseDownPickable(Constants.MouseButton button);
        public static event MouseDownPickable OnMouseDownPickable;

        public delegate void MouseUpPickable(Constants.MouseButton button);
        public static event MouseUpPickable OnMouseUpPickable;

        protected virtual void Awake() {
            initialPosition = transform.position;
        }

        public virtual void Restart() {
            transform.position = initialPosition;
            gameObject.SetActive(true);
        }

        /// <summary>What should happen when the object is picked. The "Mouse Exit" event is invoked.</summary>
        public virtual void OnPick() {
            OnMouseExitPickable?.Invoke();
        }

        void OnMouseEnter() {
            OnMouseEnterPickable?.Invoke();
        }

        void OnMouseExit() {
            OnMouseExitPickable?.Invoke();
        }

        void OnMouseOver() {
            for (int i = 0; i <= 2; i++) {
                // Check for all mouse buttons
                if (Input.GetMouseButtonDown(i)) OnMouseDownPickable?.Invoke((Constants.MouseButton) i);
                else if (Input.GetMouseButtonUp(i)) OnMouseUpPickable?.Invoke((Constants.MouseButton) i);
            }
        }

        void OnDestroy() {
            // Mouse Up + Mouse Exit when deleted
            OnMouseExitPickable?.Invoke();
        }

    }
}
