using UnityEngine;
using Randolph.Characters;

namespace Randolph.Interactable {
    public class HookEye : Interactable {

        public void Activate() => FindObjectOfType<PlayerController>().GrappleTo(transform.position);

        public override void OnInteract() => Debug.Log("Clicked on hook eye");
    }
}
