using Randolph.Interactable;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorchBurning : InventoryItem {
    public override bool IsSingleUse => false;

    public override bool IsApplicable(GameObject target)
    {
        throw new System.NotImplementedException();
    }
}
