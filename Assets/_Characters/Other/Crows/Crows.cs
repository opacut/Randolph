using Randolph.Core;
using Randolph.Interactable;
using Randolph.Levels;
using Randolph.UI;
using UnityEngine;

namespace Randolph.Characters {
    [RequireComponent(typeof(Glider))]
    public class Crows : Clickable, IRestartable {

        Animator animator;
        AudioSource audioSource;
        Glider glider;

        [SerializeField] AudioClip cawingSound;

        public override Cursors CursorType { get; protected set; } = Cursors.Inspect;

        private void Awake()
        {
            base.Awake();
            animator = GetComponent<Animator>();
            audioSource = AudioPlayer.audioPlayer.AddAudioSource(gameObject);
            glider = GetComponent<Glider>();

            glider.OnPlayerDisturbed += OnPlayerDisturbed;
        }

        public void Restart()
        {
            base.Restart();
            animator.SetBool("Flying", false);
        }

        void OnPlayerDisturbed(PlayerController player) {
            animator.SetBool("Flying", true);
            AudioPlayer.audioPlayer.PlayLocalSound(audioSource, cawingSound);
        }

    }
}
