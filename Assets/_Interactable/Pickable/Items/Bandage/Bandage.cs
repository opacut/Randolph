using System;
using UnityEngine;

namespace Randolph.Interactable {
	public class Bandage : InventoryItem {
        [SerializeField] private Cleanedbandage cleanedBandage;

        public override bool IsSingleUse { get; } = true;

        public override bool IsApplicable(GameObject target) => target.GetComponent<Alcohol>();

		public override void Apply(GameObject target) {
            base.Apply(target);

            var item = target.GetComponent<InventoryItem>();
            if (!inventory.Contains(item)) {
                item.Pick();
            }
            inventory.Remove(item);

            cleanedBandage.gameObject.SetActive(true);
            cleanedBandage.Pick();
            OnCombined?.Invoke(cleanedBandage.gameObject);
		}

        public event Action<GameObject> OnCombined;
    }
}
