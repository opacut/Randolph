using Assets._Interactable;
using Randolph.Interactable;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seed : InventoryItem
{
    public override bool IsSingleUse => true;

    public override bool IsApplicable(GameObject target) => target.GetComponent<IFeedable>() != null;

    public override void Apply(GameObject target)
    {
        base.Apply(target);
        target.GetComponent<IFeedable>().Feed(gameObject);
    }
}
