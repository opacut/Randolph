using UnityEngine;
using cakeslice;

namespace Randolph.Interactable {
    [RequireComponent(typeof(Collider2D))]
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(Outline))]
    public abstract class Interactable : MonoBehaviour, IInteractable {
        public BoxCollider2D trigger { get; protected set; }
        public Outline outline { get; protected set; }

        protected void Awake() {
            trigger = GetComponent<BoxCollider2D>();
            outline = GetComponent<Outline>();
        }

        protected void Start() {
            outline.enabled = false;
        }

        protected void OnMouseEnter() {
            outline.enabled = true;
        }

        protected void OnMouseExit() {
            outline.enabled = false;
        }

        private void OnMouseOver() {
            if (Input.GetMouseButtonDown(0)) {
                Interact();
            }
        }

        public abstract void Interact();
    }
}
