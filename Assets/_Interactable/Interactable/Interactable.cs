using UnityEngine;

namespace Randolph.Interactable {
    public abstract class Interactable : MonoBehaviour, IInteractable {

        public abstract void OnClick();

        public void OnMouseDown() {
            OnClick();
        }

    }
}
