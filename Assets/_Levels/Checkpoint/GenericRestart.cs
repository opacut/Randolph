using UnityEngine;

namespace Randolph.Levels {
    public class GenericRestart : MonoBehaviour, IRestartable {
        private Vector2 initialPosition;
        private Quaternion initialRotation;

        public void SaveState() {
            initialPosition = transform.position;
            initialRotation = transform.rotation;
        }

        public void Restart() {
            transform.position = initialPosition;
            transform.rotation = initialRotation;
        }

        private void Awake() => SaveState();
    }
}
