using System;
using UnityEngine;

namespace Randolph.Interactable {
	public class Knife : InventoryItem {

		public override bool IsSingleUse { get; } = false;
		public override bool IsApplicable(GameObject target) {
			throw new NotImplementedException();
		}

		public override void Apply(GameObject target) {
			throw new NotImplementedException();
		}

	}
}
