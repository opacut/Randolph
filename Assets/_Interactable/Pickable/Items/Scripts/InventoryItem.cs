using Randolph.Core;
using Randolph.UI;
using System;
using UnityEngine;

namespace Randolph.Interactable
{
    public abstract class InventoryItem : Pickable {

        public Sprite icon;
        [SerializeField] AudioClip collectSound;
        [SerializeField] AudioClip applySound;
        protected Inventory inventory { get; private set; }
        
        Collider2D[] colliders;
       
        public override bool isWithinReach => IsPickedUp || base.isWithinReach;

        protected override void Awake() {
            base.Awake();
            inventory = FindObjectOfType<Inventory>();
            colliders = GetComponents<Collider2D>();
        }

        public override void Pick() {
            if (IsPickedUp) {
                return;
            }
            base.Pick();

            inventory.Add(this);
            AudioPlayer.audioPlayer.PlayGlobalSound(collectSound);
            // gameObject.SetActive(false);
            SetComponentsActive(false);
            CursorManager.cursorManager.SetCursorDefault();
        }

        public void CombineWith(InventoryItem other, InventoryItem result) {
            if (!inventory.Contains(other)) {
                other.Pick();
            }
            if (other.IsSingleUse) {
                inventory.Remove(other);
            }
            var newItem = Instantiate(result);
            newItem.Pick();
            OnCombined?.Invoke(newItem);
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
        public event Action<InventoryItem> OnCombined;

        public void SetComponentsActive(bool active) {
            if (spriteRenderer) spriteRenderer.enabled = active;
            foreach (var col in colliders) {
                //if (collider) collider.enabled = active;
                col.enabled = active;
            }
        }

    }
}
