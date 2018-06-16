using Randolph.UI;
using UnityEngine;

namespace Randolph.Interactable {
    public class Cleanedbandage : InventoryItem {
        public override bool IsSingleUse { get; } = true;
        public override bool IsApplicable(GameObject target) => target.GetComponent<SpeechBubble>() && target.name == "Howard_Gerald";
    }
}