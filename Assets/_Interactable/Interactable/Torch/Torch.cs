using Assets._Interactable;
using Randolph.Interactable;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Torch : InventoryItem, IFlammable
{
    public override bool IsSingleUse { get; } = true;
    [SerializeField]
    private InventoryItem burningTorchPrefab;

    public InventoryItem GetBurningVersion()
    {
        return burningTorchPrefab;
    }

    public void Ignite()
    {
        inventory.Remove(this);

        var burningTorch = Instantiate(GetBurningVersion());
        burningTorch.GetComponent<TorchBurning>().Pick();
        OnCombined?.Invoke(burningTorch.gameObject);
        Destroy(this);
    }

    public event Action<GameObject> OnCombined;

    public override bool IsApplicable(GameObject target)
    {
        return true;
    }
}
