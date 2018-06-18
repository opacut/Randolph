using System;
using UnityEngine;

namespace Randolph.Interactable {
    public class Torch : InventoryItem, IFlammable {
        [SerializeField] private InventoryItem burningTorchPrefab;
        public override bool IsSingleUse { get; } = true;

        public InventoryItem GetBurningVersion() => burningTorchPrefab;

        public void Ignite() {
            inventory.Remove(this);

            var burningTorch = Instantiate(GetBurningVersion());
            burningTorch.GetComponent<TorchBurning>().Pick();
            OnCombined?.Invoke(burningTorch.gameObject);
            Destroy(this);
        }

        public event Action<GameObject> OnCombined;

        public override bool IsApplicable(GameObject target) => true;
    }
}
