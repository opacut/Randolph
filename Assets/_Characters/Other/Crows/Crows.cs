using Randolph.Core;
using Randolph.Interactable;
using Randolph.Levels;
using Randolph.UI;
using UnityEngine;

namespace Randolph.Characters {
    [RequireComponent(typeof(Glider))]
    public class Crows : Clickable {
        private Animator animator;
        private AudioSource audioSource;

        [SerializeField] private AudioClip cawingSound;
        private Glider glider;

        public override Cursors CursorType { get; protected set; } = Cursors.Inspect;

        public override void Restart() {
            base.Restart();
            animator.SetBool("Flying", false);
        }

        protected override void Awake() {
            base.Awake();
            animator = GetComponent<Animator>();
            audioSource = AudioPlayer.audioPlayer.AddAudioSource(gameObject);
            glider = GetComponent<Glider>();

            glider.OnPlayerDisturbed += OnPlayerDisturbed;
        }

        private void OnPlayerDisturbed() {
            animator.SetBool("Flying", true);
            AudioPlayer.audioPlayer.PlayLocalSound(audioSource, cawingSound);
        }
    }
}
