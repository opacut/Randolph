using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Whiskey : InventoryItem
{
    public override bool IsApplicable(GameObject target)
    {
        Flytrap flytrap = target.GetComponent<Flytrap>();

        return flytrap && flytrap.Active;
    }
    
    public override void OnApply(GameObject target)
    {
       target.GetComponent<Flytrap>().Deactivate();
    }
}
