using Randolph.Characters;
using Randolph.Core;
using Randolph.Levels;
using UnityEngine;

namespace Randolph.Interactable {
    public class Boulder : MonoBehaviour, IRestartable {
        private AudioSource audioSource;

        [SerializeField]
        private AudioClip crushSound;

        [SerializeField, Range(0, 180)]
        private int maxFallAngle = 30;

        [SerializeField, Range(0, 180)]
        private int maxPushAngle = 45;

        private Rigidbody2D rbody;

        private void Awake() {
            rbody = GetComponent<Rigidbody2D>();
            audioSource = AudioPlayer.audioPlayer.AddAudioSource(gameObject);

            SaveState();
        }

        private void OnTriggerEnter2D(Collider2D other) {
            if (other.tag != Constants.Tag.Enemy) {
                return;
            }
            var flytrap = other.gameObject.GetComponent<Flytrap>();
            if (!flytrap) {
                return;
            }
            PlayCrushSound();
            flytrap.Kill();
        }

        public void PlayCrushSound() {
            AudioPlayer.audioPlayer.PlayLocalSound(audioSource, crushSound, volume: Constants.Audio.BackgroundVolume);
        }

        /// <summary>Boulder falls from the above, not from the side.</summary>
        public bool HitFromAbove(Collision2D collisionWithBoulder) => Methods.GetCollisionAngle(collisionWithBoulder, Vector2.up) <= maxFallAngle;

        /// <summary>Pushes the boulder in an opposite direction of a collision, using the pusher's mass.</summary>
        public void Push(Collision2D collision) {
            if (collision.contacts.Length == 0) {
                return;
            }
            var normal = collision.contacts[0].normal;
            var otherMass = collision.otherRigidbody.mass;
            if (Mathf.Abs(Methods.GetCollisionAngle(collision, Vector2.up)) - 90f <= maxPushAngle) {
                rbody.AddForce(-normal * otherMass, ForceMode2D.Impulse);
            }
        }

        #region IRestartable
        private Vector3 initialPosition;
        private Quaternion initialRotation;

        public void SaveState() {
            initialPosition = gameObject.transform.position;
            initialRotation = gameObject.transform.rotation;
        }

        public void Restart() {
            gameObject.transform.position = initialPosition;
            gameObject.transform.rotation = initialRotation;
        }
        #endregion
    }
}
