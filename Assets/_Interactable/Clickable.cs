using Randolph.Core;

namespace Randolph.Interactable {
    public abstract class Clickable {

        public delegate void MouseEnterClickable<T>();
        public static event MouseEnterClickable<Clickable> OnMouseEnterClickable;

        public delegate void MouseExitClickable<T>();
        public static event MouseExitClickable<Clickable> OnMouseExitClickable;

        public delegate void MouseDownClickable<T>(Constants.MouseButton button);
        public static event MouseDownClickable<Clickable> OnMouseDownClickable;

        public delegate void MouseUpClickable<T>(Constants.MouseButton button);
        public static event MouseUpClickable<Clickable> OnMouseUpClickable;

        void OnMouseEnter() {
            OnOnMouseEnterClickable();
        }

        protected static void OnOnMouseEnterClickable() {
            OnMouseEnterClickable?.Invoke();
        }

    }
}
