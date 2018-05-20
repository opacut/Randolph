using UnityEngine;
using cakeslice;
using Randolph.UI;

namespace Randolph.Interactable {
    [RequireComponent(typeof(Collider2D))]
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(Outline))]
    public abstract class Interactable : Clickable, IInteractable {
       
        public override Cursors CursorType { get; protected set; } = Cursors.Interact;

        public BoxCollider2D trigger { get; protected set; }
        public Outline outline { get; protected set; }

        protected override void Awake() {
            base.Awake();

            trigger = GetComponent<BoxCollider2D>();
            outline = GetComponent<Outline>();
        }

        protected void Start() {
            outline.enabled = false;
        }

        // TODO: Events from Clickable

        protected override void OnMouseEnter() {
            base.OnMouseEnter();
            outline.enabled = true;
        }

        protected override void OnMouseExit() {
            base.OnMouseExit();
            outline.enabled = false;
        }

        protected override void OnMouseOver() {            
            base.OnMouseOver();

            if (Input.GetMouseButtonDown(0)) {
                OnInteract();
            }
        }

        public abstract void OnInteract();
    }
}
