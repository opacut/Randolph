using Assets._Interactable;
using Randolph.Interactable;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pit : InventoryItem
{
    public override bool IsSingleUse => true;

    public override bool IsApplicable(GameObject target)
    {
        if((target.GetComponent<IFeedable>() != null)||(target.GetComponent<Moonstone>() != null))
        {
            return true;
        }
        return false;
    }

    public override void Apply(GameObject target)
    {
        base.Apply(target);
        if (target.GetComponent<IFeedable>() != null)
        {
            target.GetComponent<IFeedable>().Feed(gameObject);
        }
        else if (target.GetComponent<Moonstone>() != null)
        {
            Debug.Log("entered Apply on Moonstone in Pit");
            target.GetComponent<Moonstone>().Cut(this.gameObject);
        }
    }
}
