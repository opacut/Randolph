using System;
using UnityEngine;

namespace Randolph.Interactable {
    public class Alcohol : InventoryItem {
        [SerializeField] private Cleanedbandage cleanedBandage;

        public override bool IsSingleUse { get; } = true;

        public override bool IsApplicable(GameObject target) => target.GetComponent<Bandage>();

        public override void Apply(GameObject target) {
            base.Apply(target);
            Destroy(target);
            cleanedBandage.gameObject.SetActive(true);
            cleanedBandage.Pick();
            OnCombined?.Invoke(cleanedBandage.gameObject);
        }
        
        public event Action<GameObject> OnCombined;
    }
}