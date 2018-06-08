using Randolph.Environment;
using UnityEngine;

namespace Randolph.Interactable {
    public class TiedRope : Interactable {
        [SerializeField] private Sail sail;

        public void Slash() => sail.Slash(this);
    }
}
