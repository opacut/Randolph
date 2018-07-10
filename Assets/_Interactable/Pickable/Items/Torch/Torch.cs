using System;
using UnityEngine;

namespace Randolph.Interactable {
    public class Torch : InventoryItem, IFlammable {
        [SerializeField] private Torchburning burningTorchPrefab;

        public override bool IsSingleUse => true;

        public InventoryItem BurningVersion => burningTorchPrefab;

        public override bool IsApplicable(GameObject target) => target.GetComponent<Lighter>() != null;

        public override void Apply(GameObject target) {
            base.Apply(target);
            CombineWith(target.GetComponent<Lighter>(), BurningVersion);
        }
    }
}
