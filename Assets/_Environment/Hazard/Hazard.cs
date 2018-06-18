using Randolph.Characters;
using Randolph.Core;
using Randolph.Levels;
using UnityEngine;

namespace Randolph.Environment {
    public class Hazard : MonoBehaviour, IRestartable {
        // TODO: Harmful to: layer/tag | Destroyed by: layer/tag

        private void Awake() => SaveState();

        public void OnTriggerEnter2D(Collider2D other) {
            if (other.tag == Constants.Tag.Player) {
                other.GetComponent<PlayerController>().Kill();
            }
        }

        #region IRestartable
        private Vector2 initialPosition;

        public void SaveState() {
            initialPosition = gameObject.transform.position;
        }

        public void Restart() {
            gameObject.transform.position = initialPosition;
        }
        #endregion
    }
}
