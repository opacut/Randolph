using Randolph.Core;
using Randolph.Interactable;
using Randolph.UI;
using UnityEngine;

namespace Randolph.Characters {
    [RequireComponent(typeof(SpriteRenderer))]
    public class Flytrap : Clickable, IEnemy, IFeedable {
        private Sprite alive;

        private AudioSource audioSource;

        [Header("Sprites")]
        [SerializeField]
        private Sprite closed;

        [Header("Audio")]
        [SerializeField]
        private AudioClip closeSound;

        [SerializeField] private Sprite crushed;
        [SerializeField] private InventoryItem pitPrefab;
        [SerializeField] private GameObject spawnPoint;

        public bool Active { get; private set; } = true;

        public override Cursors CursorType { get; protected set; } = Cursors.Inspect;

        public void Kill() {
            spriteRenderer.sprite = crushed;
            Active = false;
        }

        public override void Restart() {
            base.Restart();
            spriteRenderer.sprite = alive;
            Active = true;
        }

        public void Feed(GameObject target) {
            if (Active) {
                if (target.GetComponent<Fruit>() || target.GetComponent<Pit>()) {
                    Destroy(target);
                    SpawnPit();
                } else if (target.GetComponent<Seed>()) {
                    Destroy(target);
                    Kill();
                }
            }
        }

        protected override void Start() {
            base.Start();
            spriteRenderer = GetComponent<SpriteRenderer>();
            audioSource = AudioPlayer.audioPlayer.AddAudioSource(gameObject);
            alive = spriteRenderer.sprite;
        }

        private void OnTriggerEnter2D(Collider2D other) {
            if (Active) {
                if (other.tag == Constants.Tag.Player) {
                    Deactivate();
                    other.gameObject.GetComponent<PlayerController>().Kill(1);
                }

                var glider = other.GetComponent<Glider>();
                if (glider) {
                    //! Crows flying into the flytrap
                    Deactivate();
                    glider.Kill();
                }
            }
        }

        public void Deactivate() {
            spriteRenderer.sprite = closed;
            AudioPlayer.audioPlayer.PlayLocalSound(audioSource, closeSound);
            Active = false;
        }

        public void SpawnPit() {
            spriteRenderer.sprite = closed;
            Instantiate(pitPrefab, spawnPoint.transform.position, Quaternion.identity);
            spriteRenderer.sprite = alive;
        }
    }
}
