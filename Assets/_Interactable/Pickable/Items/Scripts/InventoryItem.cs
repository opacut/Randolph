using cakeslice;
using Randolph.Core;
using Randolph.UI;
using UnityEngine;

namespace Randolph.Interactable {
    public abstract class InventoryItem : Pickable {

        public Sprite icon;
        [SerializeField] AudioClip collectSound;
        [SerializeField] AudioClip applySound;
        Inventory inventory;

        SpriteRenderer spriteRenderer;
        Collider2D boxCollider;
        Outline outline;

        protected override void Awake() {
            base.Awake();
            inventory = FindObjectOfType<Inventory>();

            spriteRenderer = GetComponent<SpriteRenderer>();
            boxCollider = GetComponent<Collider2D>();
            outline = GetComponent<Outline>();
        }

        public override void OnPick() {
            base.OnPick();

            inventory.Add(this);
            AudioPlayer.audioPlayer.PlayGlobalSound(collectSound);
            // gameObject.SetActive(false);
            SetComponentsActive(false);
            CursorManager.cursorManager.SetCursorDefault();
        }

        public override void Restart() {
            base.Restart();
            SetComponentsActive(true);
        }

        public abstract bool IsApplicable(GameObject target);

        public virtual void OnApply(GameObject target) {
            AudioPlayer.audioPlayer.PlayGlobalSound(applySound);
        }

        public void SetComponentsActive(bool active) {
            if (spriteRenderer) spriteRenderer.enabled = active;
            if (boxCollider) boxCollider.enabled = active;
            if (outline) outline.enabled = active;
        }

    }
}
