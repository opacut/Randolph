using Randolph.Characters;
using Randolph.Core;
using Randolph.UI;
using UnityEngine;

namespace Randolph.Interactable {
    public class Boulder : Clickable {
        private AudioSource audioSource;

        [SerializeField]
        private AudioClip crushSound;

        [SerializeField, Range(0, 180)]
        private int maxFallAngle = 30;

        [SerializeField, Range(0, 180)]
        private int maxPushAngle = 45;

        private Rigidbody2D rbody;
        
        public override Cursors CursorType { get; protected set; } = Cursors.Inspect;

        protected override void Awake() {
            base.Awake();

            rbody = GetComponent<Rigidbody2D>();
            audioSource = AudioPlayer.audioPlayer.AddAudioSource(gameObject);
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
        private Vector2 savedVelocity;
        private float savedAngularVelocity;

        public override void SaveState() {
            base.SaveState();
            savedVelocity = rbody.velocity;
            savedAngularVelocity = rbody.angularVelocity;
        }

        public override void Restart() {
            base.Restart();
            rbody.velocity = savedVelocity;
            rbody.angularVelocity = savedAngularVelocity;
        }
        #endregion
    }
}
