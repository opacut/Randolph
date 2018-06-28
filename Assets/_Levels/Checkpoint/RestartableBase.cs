using UnityEngine;

namespace Randolph.Levels {
    public class RestartableBase : MonoBehaviour, IRestartable {
        private bool savedActiveState;
        private Vector3 savedPosition;
        private Quaternion savedRotation;
        private bool wasSaved;

        private bool IsChildOfArea {
            get {
                var parent = transform.parent;
                while (parent != null) {
                    if (parent.GetComponent<Area>()) {
                        return true;
                    }
                    parent = parent.parent;
                }
                return false;
            }
        }

        private bool IsOnlyRestartable => GetComponents<IRestartable>().Length == 1;

        protected virtual void Start() {
            Debug.Assert(IsChildOfArea, "Restartable object without an Area parent.", this);
        }

        protected virtual void OnDestroy() {
            // TODO: Skip assert if exitting play mode in editor
            Debug.Assert(!wasSaved, "Destroying saved restartable object.", this);
        }

        public virtual void SaveState() {
            wasSaved = true;

            savedActiveState = gameObject.activeSelf;
            savedPosition = transform.position;
            savedRotation = transform.rotation;
        }

        public virtual void Restart() {
            if (!wasSaved) {
                Destroy(gameObject);
                return;
            }

            gameObject.SetActive(savedActiveState);
            transform.position = savedPosition;
            transform.rotation = savedRotation;
        }
    }
}
