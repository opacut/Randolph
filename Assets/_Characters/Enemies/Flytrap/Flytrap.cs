using UnityEngine;
using Randolph.Core;

namespace Randolph.Characters {
    [RequireComponent(typeof(SpriteRenderer))]
    public class Flytrap : MonoBehaviour, IEnemy {

        public bool Active { get; private set; } = true;

        Sprite alive;
        [Header("Sprites")][SerializeField] Sprite closed;
        [SerializeField] Sprite crushed;
        [Header("Audio")] [SerializeField] AudioClip closeSound;
        [SerializeField] AudioClip crushSound;        

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
            AudioPlayer.audioPlayer.PlayLocalSound(audioSource, crushSound);
            Active = false;
        }

        public void Restart() {
            spriteRenderer.sprite = alive;
            Active = true;
        }

    }
}
