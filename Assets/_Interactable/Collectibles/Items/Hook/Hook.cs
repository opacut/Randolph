using Randolph.Characters;
using UnityEngine;

namespace Randolph.Interactable {
    public class Hook : InventoryItem {

        public override bool isSingleUse { get { return false; } }

        public override bool IsApplicable(GameObject target) {
            return target.GetComponent<HookJoint>();
        }

        public override void OnApply(GameObject target) {
            target.GetComponent<HookJoint>().Activate();
        }
    }
}