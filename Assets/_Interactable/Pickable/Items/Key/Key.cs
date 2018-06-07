using System;
using UnityEngine;

namespace Randolph.Interactable {
	public class Key : InventoryItem {
		public override bool IsSingleUse { get; } = false;

        public override bool IsApplicable(GameObject target) => !target.GetComponent<Door>()?.isLocked ?? false;
        public override void Apply(GameObject target) => target.GetComponent<Door>().isLocked = false;
    }
}
