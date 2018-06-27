using Randolph.Characters;
using UnityEngine;

namespace Randolph.Interactable {
    public class Seed : InventoryItem {
        public override bool IsSingleUse => true;

        public override bool IsApplicable(GameObject target) => target.GetComponent<Flytrap>() != null;

        public override void Apply(GameObject target) {
            base.Apply(target);
            target.GetComponent<Flytrap>().Kill();
        }
    }
}
