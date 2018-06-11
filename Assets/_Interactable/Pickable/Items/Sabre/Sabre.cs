using UnityEngine;
using Randolph.Environment;

namespace Randolph.Interactable {
    public class Sabre : InventoryItem {

        public override bool IsSingleUse => false;

        public override bool IsApplicable(GameObject target) => target.GetComponent<TiedRope>();

        public override void Apply(GameObject target) {
            base.Apply(target);
            target.GetComponent<TiedRope>().Slash();
            Destroy(target);
        }
    }
}