using Randolph.UI;
using UnityEngine;

namespace Randolph.Interactable {
    public class Cleanedbandage : InventoryItem {
        public override bool IsSingleUse { get; } = false;
        public override bool IsApplicable(GameObject target) => target.GetComponent<SpeechBubble>();
        
        public override void Apply(GameObject target) {
            base.Apply(target);

        }
    }
}
