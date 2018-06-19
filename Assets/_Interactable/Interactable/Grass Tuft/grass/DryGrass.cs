using Randolph.Interactable;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DryGrass : InventoryItem
{
    [SerializeField] private Transform torchPrefab;
    public override bool IsSingleUse => true;

    public override bool IsApplicable(GameObject target) => target.GetComponent<Twig>();

    public override void Apply(GameObject target)
    {
        base.Apply(target);

        var item = target.GetComponent<InventoryItem>();
        if (!inventory.Contains(item))
        {
            item.Pick();
        }
        inventory.Remove(item);

        Transform torch = Instantiate(torchPrefab);
        //torch.gameObject.SetActive(true);

        torch.GetComponent<Torch>().Pick();
        OnCombined?.Invoke(torch.gameObject);
    }

    public new event Action<GameObject> OnCombined;

}
