using UnityEngine;

namespace Randolph.Interactable {
    public abstract class Pickable : MonoBehaviour, IPickable {

        private Vector3 initialPosition;

        private void Awake() {
            initialPosition = transform.position;
        }

        public virtual void Restart() {
            gameObject.transform.position = initialPosition;
            gameObject.SetActive(true);
        }

        public abstract void OnPick();
    }
}