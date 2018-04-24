using UnityEngine;
using Randolph.Core;

namespace Randolph.Interactable {
    public class Objective : Pickable {

        [SerializeField] AudioClip collectSound;
        [SerializeField] Animator animator;

        public override bool isSingleUse {
            get { return false; }
        }

        void Start() {
            if (!animator) {
                animator = FindObjectOfType<Canvas>()?.GetComponent<Animator>();
                if (animator) Debug.Log("Temporarily assigning an animator from the Canvas.", gameObject);
                else Debug.LogWarning("The objective is missing an animator.", gameObject);
            }
        }

        public bool IsCompleted { get; internal set; }

        public override void OnPick() {
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
