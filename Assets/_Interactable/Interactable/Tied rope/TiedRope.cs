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

        public override void OnInteract() => Debug.Log("Rope clicked.");
    }
}
