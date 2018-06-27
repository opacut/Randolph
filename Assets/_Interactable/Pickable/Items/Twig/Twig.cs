using System;
using UnityEngine;

namespace Randolph.Interactable {
    public class Twig : InventoryItem {
        [SerializeField] private Torch torchPrefab;

        public override bool IsSingleUse => true;

        public override bool IsApplicable(GameObject target) => target.GetComponent<DryGrass>();

        public override void Apply(GameObject target) {
            base.Apply(target);
            CombineWith(target.GetComponent<DryGrass>(), torchPrefab);
        }
    }
}
