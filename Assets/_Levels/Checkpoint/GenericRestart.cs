using UnityEngine;

namespace Randolph.Levels {
    public class GenericRestart : MonoBehaviour, IRestartable {
        [SerializeField, ReadonlyField]
        Vector3 initialPosition;
        [SerializeField, ReadonlyField]
        Quaternion initialRotation;

        void Awake() {
            initialPosition = transform.position;
            initialRotation = transform.rotation;
        }

        public void Restart() {
            transform.position = initialPosition;
            transform.rotation = initialRotation;
        }

    }
}