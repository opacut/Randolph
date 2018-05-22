using Randolph.Characters;
using UnityEngine;
using Randolph.Environment;

namespace Randolph.Interactable {
    public class Hook : InventoryItem {
        [SerializeField] private ClimbableRope connectedRope;

        public override bool IsSingleUse => false;

        public override void OnPick() {
            base.OnPick();
            FindObjectOfType<PlayerController>().StopGrappling();
            if (connectedRope) Destroy(connectedRope.gameObject);
        }

        public override bool IsApplicable(GameObject target) => target.GetComponent<HookEye>();

        public override void OnApply(GameObject target) {
            base.OnApply(target);
            target.GetComponent<HookEye>().Activate();
        }
    }
}