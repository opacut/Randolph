using System;

namespace Randolph.Interactable {
    public interface IInteractable {
        void Interact();
        event Action OnInteract;
    }
}
