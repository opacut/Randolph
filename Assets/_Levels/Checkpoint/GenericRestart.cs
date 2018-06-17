using Randolph.Interactable;
using Randolph.UI;
using UnityEngine;

namespace Randolph.Levels {
    public class GenericRestart : MonoBehaviour, IRestartable {

        [SerializeField, ReadonlyField] Vector2 initialPosition;
        [SerializeField, ReadonlyField] Quaternion initialRotation;

        //public override Cursors CursorType { get; protected set; } = Cursors.Inspect;

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
