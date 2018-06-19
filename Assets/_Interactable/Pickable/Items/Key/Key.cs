using UnityEngine;

namespace Randolph.Interactable {
    public class Key : InventoryItem {
        public override bool IsSingleUse { get; } = true;

        public override bool IsApplicable(GameObject target) => target.GetComponent<Door>()?.isLocked ?? false;

        public override void Apply(GameObject target) {
            base.Apply(target);
            target.GetComponent<Door>().isLocked = false;
        }
    }
}
