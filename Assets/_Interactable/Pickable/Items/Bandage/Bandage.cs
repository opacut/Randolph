using UnityEngine;

namespace Randolph.Interactable {
    public class Bandage : InventoryItem {
        [SerializeField] private CleanedBandage cleanedBandage;

        public override bool IsSingleUse { get; } = true;

        public override bool IsApplicable(GameObject target) => target.GetComponent<Alcohol>();

        public override void Apply(GameObject target) {
            base.Apply(target);
            CombineWith(target.GetComponent<InventoryItem>(), cleanedBandage);
        }
    }
}
