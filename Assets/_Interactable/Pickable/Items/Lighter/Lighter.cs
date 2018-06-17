using Assets._Interactable;
using Randolph.Interactable;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lighter : InventoryItem {
    public override bool IsSingleUse { get; } = false;

    public override bool IsApplicable(GameObject target) => target.GetComponent<IFlammable>() != null;

    public override void Apply(GameObject target)
    {
        base.Apply(target);
        target.GetComponent<IFlammable>().Ignite();
    }
}
