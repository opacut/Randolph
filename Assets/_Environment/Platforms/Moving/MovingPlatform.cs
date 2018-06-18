using Randolph.Characters;
using Randolph.Core;
using Randolph.Levels;
using UnityEngine;

namespace Randolph.Environment {
    [RequireComponent(typeof(Glider))]
    public class MovingPlatform : MonoBehaviour, IRestartable {
        private Animator animator;
        private Transform attachedPlayer;

        [SerializeField]
        private Sprite chainCorner;

        private Transform chainHolder;

        [SerializeField]
        private GameObject chainLink;

        private Vector2 lastPosition;

        [SerializeField]
        private bool visibleChain;

        private void Awake() {
            animator = GetComponent<Animator>();
            lastPosition = transform.position;

            if (visibleChain) {
                ConstructChain();
            }
            SaveState();
        }

        private void FixedUpdate() {
            if (attachedPlayer != null) {
                var positionChange = GetPositionChange();
                if (positionChange != Vector2.zero) {
                    attachedPlayer.position += new Vector3(positionChange.x, positionChange.y, 0);
                }
            }

            animator.SetBool("Move", lastPosition != (Vector2)transform.position); // Move animation when moving
            lastPosition = transform.position;
        }

        private void OnCollisionEnter2D(Collision2D collision) {
            if (collision.gameObject.tag == Constants.Tag.Player) {
                attachedPlayer = collision.transform;
            }
        }

        private void OnCollisionExit2D(Collision2D collision) {
            if (collision.gameObject.tag == Constants.Tag.Player) {
                attachedPlayer = null;
            }
        }

        private void ConstructChain() {
            if (transform.childCount == 0) {
                Instantiate(new GameObject("Chain"), transform);
            }
            chainHolder = transform.GetChild(0);
            if (chainHolder.childCount > 0) {
                // Remove old chain
                for (var i = 0; i < transform.childCount; i++) {
                    Destroy(transform.GetChild(i));
                }
            }
        }

        private Vector2 GetPositionChange() {
            if (Mathf.Approximately(Vector2.Distance(transform.position, lastPosition), 0f)) {
                return Vector2.zero;
            }
            return transform.position - new Vector3(lastPosition.x, lastPosition.y, 0f);
        }

        #region IRestartable
        private Vector3 initialPosition;
        private Quaternion initialRotation;

        public void SaveState() {
            initialPosition = transform.position;
            initialRotation = transform.rotation;
        }

        public void Restart() {
            // Position handled by the glider
            attachedPlayer = null;
            animator.SetBool("Move", false);

            transform.position = initialPosition;
            transform.rotation = initialRotation;
        }
        #endregion
    }
}
