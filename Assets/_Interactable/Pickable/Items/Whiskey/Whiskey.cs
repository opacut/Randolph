﻿using Randolph.Characters;
using UnityEngine;

namespace Randolph.Interactable {
    public class Whiskey : InventoryItem {

        public override bool IsSingleUse { get { return true; } }

        public override bool IsApplicable(GameObject target) {
            var flytrap = target.GetComponent<Flytrap>();
            return flytrap && flytrap.Active;
        }

        public override void OnApply(GameObject target) {
            base.OnApply(target);
            target.GetComponent<Flytrap>().Deactivate();
        }

    }
}