using UnityEngine;

namespace Randolph.Levels {
    public class GenericRestart : MonoBehaviour, IRestartable {

        [SerializeField, ReadonlyField] Vector3 initialPosition;

        void Awake() {
            initialPosition = gameObject.transform.position;
        }

        public void Restart() {
            gameObject.transform.position = initialPosition;
        }

    }
}
