using UnityEngine;
using Randolph.Environment;

namespace Randolph.Interactable {
    public class Hook : InventoryItem {

        public override bool IsSingleUse => false;

        public override bool IsApplicable(GameObject target) => target.GetComponent<HookEye>();

        public override void OnApply(GameObject target) {
            base.OnApply(target);
            target.GetComponent<HookEye>().Activate();
        }
    }
}