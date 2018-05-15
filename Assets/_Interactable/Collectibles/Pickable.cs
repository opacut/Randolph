using Randolph.Levels;
using UnityEngine;

namespace Randolph.Interactable {
    public abstract class Pickable : MonoBehaviour, IPickable {

        private Vector3 initialPosition;
        public abstract bool isSingleUse { get; }

        public delegate void MouseEnterPickable();

        public static event MouseEnterPickable OnMouseEnterPickable;

        public delegate void MouseExitPickable();

        public static event MouseExitPickable OnMouseExitPickable;

        public delegate void MouseDownPickable();

        public static event MouseDownPickable OnMouseDownPickable;

        public delegate void MouseUpPickable();

        public static event MouseUpPickable OnMouseUpPickable;

        protected virtual void Awake() {
            initialPosition = transform.position;
        }

        public virtual void Restart() {
            transform.position = initialPosition;
            gameObject.SetActive(true);
        }

        public abstract void OnPick();

        void OnMouseEnter() {
            OnMouseEnterPickable?.Invoke();
        }

        void OnMouseExit() {
            OnMouseExitPickable?.Invoke();
        }

        void OnMouseDown() {
            OnMouseDownPickable?.Invoke();
        }

        void OnMouseUp() {
            OnMouseUpPickable?.Invoke();
        }

    }
}
