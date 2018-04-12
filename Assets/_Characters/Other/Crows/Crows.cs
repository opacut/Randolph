using Randolph.Core;
using Randolph.Levels;
using UnityEngine;

namespace Randolph.Characters {
    [RequireComponent(typeof(Glider))]
    public class Crows : MonoBehaviour, IRestartable {

        Animator animator;
        AudioSource audioSource;
        Glider glider;

        [SerializeField] AudioClip cawingSound;

        private void Awake() {
            animator = GetComponent<Animator>();
            audioSource = AudioPlayer.audioPlayer.AddLocalAudioSource(gameObject);
            glider = GetComponent<Glider>();

            glider.OnPlayerDisturbed += OnPlayerDisturbed;
        }

        public void Restart() {
            animator.SetBool("Flying", false);
        }

        void OnPlayerDisturbed(PlayerController player) {
            animator.SetBool("Flying", true);
            AudioPlayer.audioPlayer.PlayLocalSound(audioSource, cawingSound);
        }

    }
}
