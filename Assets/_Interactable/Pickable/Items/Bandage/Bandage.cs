using System;
using UnityEngine;

namespace Randolph.Interactable {
	public class Bandage : InventoryItem {

		public override bool IsSingleUse { get; } = true;

        public override bool IsApplicable(GameObject target) => false;

		public override void OnApply(GameObject target) {
            base.OnApply(target);

		}

	}
}
