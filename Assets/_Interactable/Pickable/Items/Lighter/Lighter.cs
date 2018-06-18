using UnityEngine;

namespace Randolph.Interactable {
    public class Lighter : InventoryItem {
        public override bool IsSingleUse => false;

        public override bool IsApplicable(GameObject target) => target.GetComponent<IFlammable>() != null;

        public override void Apply(GameObject target) {
            base.Apply(target);
            target.GetComponent<IFlammable>().Ignite();
        }
    }
}
