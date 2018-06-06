using UnityEngine;
using Randolph.UI;

namespace Randolph.Interactable {
    [RequireComponent(typeof(Collider2D))]
    [RequireComponent(typeof(SpriteRenderer))]
    public abstract class Interactable : Clickable, IInteractable {
       
        public override Cursors CursorType { get; protected set; } = Cursors.Interact;

        public BoxCollider2D trigger { get; protected set; }

        protected override void Awake() {
            base.Awake();
            trigger = GetComponent<BoxCollider2D>();
        }

        public abstract void OnInteract();
    }
}
