using UnityEngine;
using Randolph.Characters;
using Randolph.Core;
using Randolph.Levels;

namespace Randolph.Interactable {
    public class Boulder : MonoBehaviour, IRestartable {

        [SerializeField] AudioClip crushSound;
        [SerializeField, Range(0, 180)] int maxFallAngle = 30;
        [SerializeField, Range(0, 180)] int maxPushAngle = 45;

        Vector3 initialPosition;
        Quaternion initialRotation;
        Rigidbody2D rbody;
        AudioSource audioSource;

        void Awake() {
            rbody = GetComponent<Rigidbody2D>();
            audioSource = AudioPlayer.audioPlayer.AddAudioSource(gameObject);

            initialPosition = gameObject.transform.position;
            initialRotation = gameObject.transform.rotation;
        }

        public void Restart() {
            gameObject.transform.position = initialPosition;
            gameObject.transform.rotation = initialRotation;
        }

        void OnTriggerEnter2D(Collider2D other) {
            if (other.tag == Constants.Tag.Enemy) {
                var flytrap = other.gameObject.GetComponent<Flytrap>();
                if (flytrap) {
                    PlayCrushSound();
                    flytrap.Kill();
                }
            }
        }

        public void PlayCrushSound() {
            AudioPlayer.audioPlayer.PlayLocalSound(audioSource, crushSound, volume: Constants.Audio.BackgroundVolume);
        }

        /// <summary>Boulder falls from the above, not from the side.</summary>        
        public bool HitFromAbove(Collision2D collisionWithBoulder) {
            return Methods.GetCollisionAngle(collisionWithBoulder, Vector2.up) <= maxFallAngle;
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
