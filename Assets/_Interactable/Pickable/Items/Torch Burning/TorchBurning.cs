using System;
using Randolph.Interactable;
using UnityEngine;

public class TorchBurning : InventoryItem {
    public override bool IsSingleUse => false;

    public override bool IsApplicable(GameObject target) {
        throw new NotImplementedException();
    }
}
