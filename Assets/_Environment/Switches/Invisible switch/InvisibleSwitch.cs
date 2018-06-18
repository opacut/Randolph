using Randolph.Core;
using Randolph.Interactable;
using Randolph.Levels;
using UnityEngine;

namespace Randolph.Environment {
    public class InvisibleSwitch : MonoBehaviour, IRestartable {
        private AudioSource audioSource;
        [SerializeField] private Bats bats;
        [SerializeField] private bool isOn;
        [SerializeField] private AudioClip thumpSound;

        private void Awake() {
            audioSource = AudioPlayer.audioPlayer.AddAudioSource(gameObject);
        }

        private void OnTriggerEnter2D(Collider2D other) {
            if (!other.GetComponent<Boulder>()) {
                return;
            }
            AudioPlayer.audioPlayer.PlayLocalSound(audioSource, thumpSound);
            Flip(true);
            bats.StartMoving();
        }

        private void Flip(bool active) {
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
