using UnityEngine;

using Randolph.Characters;
using Randolph.Levels;

namespace Randolph.Environment {
    public class Hazard : MonoBehaviour, IRestartable {

        [SerializeField, ReadonlyField] Vector2 initialPosition;

        void Awake() {
            initialPosition = gameObject.transform.position;
        }

        public void Restart() {
            gameObject.transform.position = initialPosition;
        }

        public void OnTriggerEnter2D(Collider2D other) {
            if (other.tag == "Player") {
                other.GetComponent<PlayerController>().Kill();
            }
        }

    }
}
