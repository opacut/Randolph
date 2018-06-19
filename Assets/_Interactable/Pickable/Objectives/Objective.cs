using Randolph.Core;
using UnityEngine;

namespace Randolph.Interactable {
    public class Objective : Pickable {
        [SerializeField]
        private Animator animator;

        [SerializeField]
        private AudioClip collectSound;

        public override bool IsSingleUse => false;

        public bool IsCompleted { get; internal set; }

        protected override void Start() {
            base.Start();
            if (!animator) {
                animator = FindObjectOfType<Canvas>()?.GetComponent<Animator>();
                if (animator) {
                    Debug.Log("Temporarily assigning an animator from the Canvas.", gameObject);
                } else {
                    Debug.LogWarning("The objective is missing an animator.", gameObject);
                }
            }
        }

        public override void Pick() {
            base.Pick();

            IsCompleted = true;
            AudioPlayer.audioPlayer.PlayGlobalSound(collectSound);
            gameObject.SetActive(false);

            animator.SetTrigger("ObjectiveFound");
        }

        public override void Restart() {
            IsCompleted = false;
            base.Restart();
        }
    }
}
