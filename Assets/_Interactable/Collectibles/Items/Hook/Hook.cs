using UnityEngine;
using Randolph.Environment;

namespace Randolph.Interactable {
    public class Hook : InventoryItem {

        public override bool isSingleUse { get { return false; } }

        public override bool IsApplicable(GameObject target) {
            return target.GetComponent<HookEye>();
        }

        public override void OnApply(GameObject target) {
            target.GetComponent<HookEye>().Activate();
        }
    }
}