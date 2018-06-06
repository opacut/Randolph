﻿using Randolph.Characters;
using UnityEngine;

namespace Randolph.Interactable {
    public class Whiskey : InventoryItem {

        public override bool IsSingleUse => true;

        public override bool IsApplicable(GameObject target) => target.GetComponent<Flytrap>()?.Active ?? false;

        public override void OnApply(GameObject target) {
            base.OnApply(target);
            target.GetComponent<Flytrap>().Deactivate();
        }

    }
}