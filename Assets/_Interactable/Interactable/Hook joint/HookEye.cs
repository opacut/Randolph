using UnityEngine;
using Randolph.Characters;

namespace Randolph.Interactable {
    public class HookEye : Interactable {

        public void Activate() => FindObjectOfType<PlayerController>().GrappleTo(transform.position);
    }
}
