using Randolph.Interactable;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DryGrass : InventoryItem {
    public override bool IsSingleUse => true;

    public override bool IsApplicable(GameObject target) => false;

    public override void OnApply(GameObject target)
    {
        base.OnApply(target);
    }
}
