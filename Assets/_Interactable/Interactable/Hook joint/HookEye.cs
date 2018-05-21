using UnityEngine;
using Randolph.Characters;

namespace Randolph.Interactable {
    public class HookEye : Interactable {
        PlayerController player;

        protected override void Awake() {
            base.Awake();
            player = FindObjectOfType<PlayerController>();
        }

        public void Activate() => player.GrappleTo(gameObject);

        public override void OnInteract() => Debug.Log("Clicked on hook eye");
    }
}
