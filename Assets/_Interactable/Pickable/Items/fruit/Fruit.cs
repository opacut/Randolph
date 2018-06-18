using Randolph.Interactable;
using UnityEngine;

public class Fruit : InventoryItem {
    public override bool IsSingleUse => true;

    public override bool IsApplicable(GameObject target) => target.GetComponent<IFeedable>() != null;

    public override void Apply(GameObject target) {
        base.Apply(target);
        target.GetComponent<IFeedable>().Feed(gameObject);
    }

    /*
    public override void Restart()
    {
        base.Restart();
        SetComponentsActive(true);
    }
    */
}
