using Randolph.Core;
using Randolph.Interactable;
using Randolph.Levels;
using UnityEngine;

namespace Randolph.Environment {
    public class InvisibleSwitch : MonoBehaviour, IRestartable {
        AudioSource audioSource;
        [SerializeField] Bats bats;
        // ReSharper disable once NotAccessedField.Local
        [SerializeField] bool isOn;
        [SerializeField] AudioClip thumpSound;

        void Awake() {
            audioSource = AudioPlayer.audioPlayer.AddAudioSource(gameObject);
        }

        void OnTriggerEnter2D(Collider2D other) {
            if (!other.GetComponent<Boulder>()) {
                return;
            }
            AudioPlayer.audioPlayer.PlayLocalSound(audioSource, thumpSound);
            Flip(true);
            bats.StartMoving();
        }

        void Flip(bool active) {
            isOn = active;
        }

        #region IRestartable
        public void SaveState() { }

        public void Restart() {
            isOn = false;
        }
        #endregion
    }
}
