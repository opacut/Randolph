using System;
using UnityEngine;

namespace Randolph.Interactable {
    public class Alcohol : InventoryItem {

        public override bool IsSingleUse { get; } = true;

        public override bool IsApplicable(GameObject target) => false;

        public override void Apply(GameObject target) {
            base.Apply(target);

        }

    }
}