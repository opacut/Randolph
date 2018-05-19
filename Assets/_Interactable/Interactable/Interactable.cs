using Randolph.UI;
using UnityEngine;

namespace Randolph.Interactable {
    public abstract class Interactable : Clickable, IInteractable {

        public override Cursors CursorType { get; protected set; } = Cursors.Interact;

        public virtual void OnInteract() {
            base.ResetCursor();
        }        

    }
}
