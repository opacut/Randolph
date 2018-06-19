using System;
using UnityEngine;

namespace Randolph.Interactable {
    public class Twig : InventoryItem {
        [SerializeField]
        private Transform torchPrefab;

        public override bool IsSingleUse { get; } = true;

        public override bool IsApplicable(GameObject target) => target.GetComponent<DryGrass>();

        public override void Apply(GameObject target) {
            base.Apply(target);

            var item = target.GetComponent<InventoryItem>();
            if (!inventory.Contains(item)) {
                item.Pick();
            }
            inventory.Remove(item);

            var torch = Instantiate(torchPrefab);
            //torch.gameObject.SetActive(true);

            torch.GetComponent<Torch>().Pick();
            OnCombined?.Invoke(torch.gameObject);
            Destroy(this);
        }

        public new event Action<GameObject> OnCombined;
    }
}
