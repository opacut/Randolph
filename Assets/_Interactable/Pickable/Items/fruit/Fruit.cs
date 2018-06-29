using Randolph.Characters;
using Randolph.Interactable;
using UnityEngine;

public class Fruit : InventoryItem {
    [SerializeField] private Pit pit;

    public override bool IsSingleUse => true;

    public override bool IsApplicable(GameObject target) => target.GetComponent<Flytrap>() != null;

    public override void Apply(GameObject target) {
        base.Apply(target);
        var newItem = Instantiate(pit, transform.parent);
        newItem.Pick();
    }
}
