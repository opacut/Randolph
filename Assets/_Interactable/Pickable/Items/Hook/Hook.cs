using Randolph.Core;
using UnityEngine;

namespace Randolph.Interactable {
    public class Hook : InventoryItem {
        public override bool IsSingleUse => false;

        public override void Pick() {
            base.Pick();
            Constants.Randolph.StopGrappling();
        }

        public override bool IsApplicable(GameObject target) => target.GetComponent<HookEye>();

        public override void Apply(GameObject target) {
            base.Apply(target);
            target.GetComponent<HookEye>().Activate();
        }
    }
}
