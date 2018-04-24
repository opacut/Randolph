using UnityEngine;
using Randolph.Characters;

namespace Randolph.Environment {
    public class HookEye : MonoBehaviour {

        PlayerController player;

        void Awake() {
            player = FindObjectOfType<PlayerController>();
        }

        public void Activate() {
            player.GrappleTo(gameObject);
        }

    }
}
