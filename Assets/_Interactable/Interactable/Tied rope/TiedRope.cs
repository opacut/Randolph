using Randolph.Environment;
using UnityEngine;

namespace Randolph.Interactable {
    public class TiedRope : Interactable, ISlashable {
        [SerializeField] private Sail sail;

        public void Slash() {
            sail.Slash(this);
            gameObject.SetActive(false);
        }

        public override void Interact() => Debug.Log("Rope clicked.");
    }
}
