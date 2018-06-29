using UnityEngine;

namespace Randolph.Interactable {
    public class CleanedBandage : InventoryItem {
        public override bool IsSingleUse { get; } = true;
        public override bool IsApplicable(GameObject target) => target.GetComponent<Talkable>() && target.name == "HowardAndGerald";
    }
}
