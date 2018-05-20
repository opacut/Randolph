using UnityEngine;
using Randolph.Environment;

namespace Randolph.Interactable {
    public class Sabre : InventoryItem {

        public override bool isSingleUse { get { return false; } }

        public override bool IsApplicable(GameObject target) {
            return target.GetComponent<TiedRope>();
        }

        public override void OnApply(GameObject target) {
            base.OnApply(target);
            Destroy(target);
        }
    }
}