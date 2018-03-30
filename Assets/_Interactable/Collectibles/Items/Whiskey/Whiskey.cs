using UnityEngine;
using Randolph.Characters;

namespace Randolph.Interactable {
    public class Whiskey : InventoryItem {

        public override bool IsApplicable(GameObject target) {
            var flytrap = target.GetComponent<Flytrap>();

            return flytrap && flytrap.Active;
        }

        public override void OnApply(GameObject target) {
            target.GetComponent<Flytrap>().Deactivate();
        }

    }
}
