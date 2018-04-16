using UnityEngine;

using Randolph.Characters;
using Randolph.Core;
using Randolph.Levels;

namespace Randolph.Interactable {
    public class Boulder : MonoBehaviour, IRestartable {
        
        [SerializeField] float maxFallAngle = 30f;
        [SerializeField] float maxPushAngle = 45f;

        Vector3 initialPosition;
        Quaternion initialRotation;
        Rigidbody2D rbody;

        void Awake() {
            rbody = GetComponent<Rigidbody2D>();

            initialPosition = gameObject.transform.position;
            initialRotation = gameObject.transform.rotation;
        }

        public void Restart() {
            gameObject.transform.position = initialPosition;
            gameObject.transform.rotation = initialRotation;
        }

        private void OnTriggerEnter2D(Collider2D other) {            
            if (other.tag == Constants.Tag.Enemy) {
                other.gameObject.GetComponent<Flytrap>()?.Kill();
            }
        }

        /// <summary>Boulder falls from the above, not from the side.</summary>        
        public bool HitFromAbove(Collision2D collision) {
            return Methods.GetCollisionAngle(collision, Vector2.up) <= maxFallAngle;
        }
        
        /// <summary>Pushes the boulder in an opposite direction of a collision, using the pusher's mass.</summary>        
        public void Push(Collision2D collision) {
            if (collision.contacts.Length == 0) return;
            Vector2 normal = collision.contacts[0].normal;
            float otherMass = collision.otherRigidbody.mass;
            if (Mathf.Abs(Methods.GetCollisionAngle(collision, Vector2.up)) - 90f <= maxPushAngle) {
                rbody.AddForce(-normal * otherMass, ForceMode2D.Impulse);
            }
        }
    }
}
