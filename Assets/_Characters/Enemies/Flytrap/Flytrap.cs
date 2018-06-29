using Randolph.Core;
using Randolph.Interactable;
using Randolph.UI;
using UnityEngine;

namespace Randolph.Characters {
    [RequireComponent(typeof(SpriteRenderer))]
    public class Flytrap : Clickable, IEnemy {
        private Sprite alive;

        private AudioSource audioSource;

        [Header("Sprites")]
        [SerializeField] private Sprite closed;

        [Header("Audio")]
        [SerializeField] private AudioClip closeSound;

        [SerializeField] private Sprite crushed;

        public bool Active { get; private set; } = true;

        public override Cursors CursorType { get; protected set; } = Cursors.Inspect;

        public override void Restart() {
            base.Restart();
            spriteRenderer.sprite = alive;
            Active = true;
        }

        public void Kill() {
            spriteRenderer.sprite = crushed;
            Active = false;
        }

        protected override void Start() {
            base.Start();
            spriteRenderer = GetComponent<SpriteRenderer>();
            audioSource = AudioPlayer.audioPlayer.AddAudioSource(gameObject);
            alive = spriteRenderer.sprite;
            outline.color = 2;
        }

        private void OnTriggerEnter2D(Collider2D other) {
            if (Active) {
                if (other.CompareTag(Constants.Tag.Player)) {
                    Deactivate();
                    other.gameObject.GetComponent<PlayerController>().Kill(1, true);
                }

                var glider = other.GetComponent<Glider>();
                if (glider) {
                    //! Crows flying into the flytrap
                    Deactivate();
                    // TODO: Make flytrap eat only crows and not any glider
                    glider.Kill();
                }
            }
        }

        public void Deactivate() {
            spriteRenderer.sprite = closed;
            AudioPlayer.audioPlayer.PlayLocalSound(audioSource, closeSound);
            Active = false;
        }
    }
}
