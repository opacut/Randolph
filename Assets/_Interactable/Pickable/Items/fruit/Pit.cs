using Randolph.Characters;
using UnityEngine;

namespace Randolph.Interactable {
    public class Pit : InventoryItem {
        [SerializeField] private Seed seed;

        public override bool IsSingleUse => true;

        public override bool IsApplicable(GameObject target) {
            return target.GetComponent<Flytrap>() != null || target.GetComponent<Moonstone>() != null;
        }

        public override void Apply(GameObject target) {
            base.Apply(target);
            if (target.GetComponent<Flytrap>() != null) {
                var newItem = Instantiate(this, transform.parent);
                newItem.Pick();
            } else if (target.GetComponent<Moonstone>() != null) {
                var newItem = Instantiate(seed, transform.parent);
                newItem.Pick();
            }
        }
    }
}
