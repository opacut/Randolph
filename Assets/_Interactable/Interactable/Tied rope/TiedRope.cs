using Randolph.Environment;
using UnityEngine;

namespace Randolph.Interactable {
    public class TiedRope : Interactable {
        [SerializeField] private Sail sail;

        public void Slash() => sail.Slash(this);

        public override void OnInteract() => Debug.Log("Rope clicked.");
    }
}
