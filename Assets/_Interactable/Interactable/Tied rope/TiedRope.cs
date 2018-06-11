using Assets._Interactable;
using Randolph.Environment;
using UnityEngine;

namespace Randolph.Interactable {
    public class TiedRope : Interactable, ISlashable {
        [SerializeField] private Sail sail;

        public void Slash()
        {
            sail.Slash(this);
            Destroy(this);
        }

        public override void Interact() => Debug.Log("Rope clicked.");
    }
}
