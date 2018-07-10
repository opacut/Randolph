using System;
using UnityEngine;

namespace Randolph.Interactable {
	public class Torchburning : InventoryItem {

		public override bool IsSingleUse => false;
        public override bool IsApplicable(GameObject target) => false;

		public override void Apply(GameObject target) {
            return;
		}

	}
}
