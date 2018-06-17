using UnityEngine;
using Randolph.Core;
using Assets._Interactable;
using Randolph.Interactable;
using Randolph.UI;

namespace Randolph.Characters {
    [RequireComponent(typeof(SpriteRenderer))]
    public class Flytrap : Clickable, IEnemy, IFeedable {

        public bool Active { get; private set; } = true;

        public override Cursors CursorType { get; protected set; } = Cursors.Inspect;

        Sprite alive;
        [Header("Sprites")] [SerializeField] Sprite closed;
        [SerializeField] Sprite crushed;
        [Header("Audio")] [SerializeField] AudioClip closeSound;
        [SerializeField] InventoryItem pitPrefab;
        [SerializeField] private GameObject spawnPoint;

        SpriteRenderer spriteRenderer;
        AudioSource audioSource;

        void Start() {
            spriteRenderer = GetComponent<SpriteRenderer>();
            audioSource = AudioPlayer.audioPlayer.AddAudioSource(gameObject);
            alive = spriteRenderer.sprite;
        }

        void OnTriggerEnter2D(Collider2D other) {
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

        public void Kill() {
            spriteRenderer.sprite = crushed;
            Active = false;
        }

        public void Restart() {
            spriteRenderer.sprite = alive;
            Active = true;
        }

        public void Feed(GameObject target)
        {
            if (Active)
            {
                if (target.GetComponent<Fruit>() || target.GetComponent<Pit>())
                {
                    Destroy(target);
                    SpawnPit();
                }
                else if (target.GetComponent<Seed>())
                {
                    Destroy(target);
                    Kill();
                }
            }

        }

        public void SpawnPit()
        {
            spriteRenderer.sprite = closed;
            Instantiate(pitPrefab, spawnPoint.transform.position, Quaternion.identity);
            spriteRenderer.sprite = alive;
        }
    }
}
