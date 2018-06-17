using System;
using cakeslice;
using Randolph.Core;
using Randolph.UI;
using UnityEngine;

namespace Randolph.Interactable {
    public abstract class InventoryItem : Pickable {

        public Sprite icon;
        [SerializeField] AudioClip collectSound;
        [SerializeField] AudioClip applySound;
        protected Inventory inventory { get; private set; }
        
        Collider2D[] colliders;
        Collider2D boxCollider;

        protected override void Awake() {
            base.Awake();
            inventory = FindObjectOfType<Inventory>();
            colliders = GetComponents<Collider2D>();
            boxCollider = GetComponent<Collider2D>();
        }

        public override void Pick() {
            base.Pick();

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

        public virtual void Apply(GameObject target) {
            AudioPlayer.audioPlayer.PlayGlobalSound(applySound);
            OnApply?.Invoke();
        }

        public event Action OnApply;

        public void SetComponentsActive(bool active) {
            if (spriteRenderer) spriteRenderer.enabled = active;
            if (boxCollider) boxCollider.enabled = active;
            foreach (var collider in colliders) {
                //if (collider) collider.enabled = active;
                collider.enabled = active;
            }
        }

    }
}
