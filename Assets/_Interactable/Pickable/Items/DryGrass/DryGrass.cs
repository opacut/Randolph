using System;
using Randolph.Interactable;
using UnityEngine;

public class DryGrass : InventoryItem {
    [SerializeField] private Torch torchPrefab;
    public override bool IsSingleUse => true;

    public override bool IsApplicable(GameObject target) => target.GetComponent<Twig>();

    public override void Apply(GameObject target) {
        base.Apply(target);
        CombineWith(target.GetComponent<Twig>(), torchPrefab);
    }
}
