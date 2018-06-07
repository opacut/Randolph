using Randolph.Characters;
using UnityEngine;
using Randolph.Environment;

namespace Randolph.Interactable {
    public class Hook : InventoryItem {
        [SerializeField] private ClimbableRope connectedRope;

        public override bool IsSingleUse => false;

        public override void Pick() {
            base.Pick();
            FindObjectOfType<PlayerController>().StopGrappling();
            if (connectedRope) Destroy(connectedRope.gameObject);
        }

        public override bool IsApplicable(GameObject target) => target.GetComponent<HookEye>();

        public override void Apply(GameObject target) {
            base.Apply(target);
            target.GetComponent<HookEye>().Activate();
        }
    }
}