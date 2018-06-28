using Randolph.Characters;
using Randolph.Core;
using Randolph.Levels;
using UnityEngine;

namespace Randolph.Environment {
    public class Hazard : RestartableBase {
        // TODO: Harmful to: layer/tag | Destroyed by: layer/tag

        public void OnTriggerEnter2D(Collider2D other) {
            if (other.CompareTag(Constants.Tag.Player)) {
                other.GetComponent<PlayerController>().Kill();
            }
        }
    }
}
