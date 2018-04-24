using Randolph.Core;
using Randolph.UI;
using UnityEngine;

namespace Randolph.Interactable {
    public abstract class InventoryItem : Pickable {

        public Sprite icon;
        [SerializeField] AudioClip collectSound;
        [SerializeField] AudioClip applySound;
        Inventory inventory;

        protected override void Awake() {
            base.Awake();
            inventory = FindObjectOfType<Inventory>();
        }

        public override void OnPick() {
            inventory.Add(this);
            AudioPlayer.audioPlayer.PlayGlobalSound(collectSound);
            gameObject.SetActive(false);
        }

        public abstract bool IsApplicable(GameObject target);

        public virtual void OnApply(GameObject target) {
            AudioPlayer.audioPlayer.PlayGlobalSound(applySound);
        }

    }
}
