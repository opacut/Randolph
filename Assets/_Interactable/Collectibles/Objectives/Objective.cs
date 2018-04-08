using UnityEngine;

namespace Randolph.Interactable {
    public class Objective : Pickable {

        [SerializeField] Animator animator;
        public override bool isSingleUse { get { return false; } }

        void Start() {
            if (!animator) {
                animator = FindObjectOfType<Canvas>()?.GetComponent<Animator>();
                if (animator) Debug.Log("Temporary assigning an animator from the Canvas.");
                else Debug.LogWarning("The objective is missing an animator.", gameObject);
            }
        }

        public bool IsCompleted { get; internal set; }

        public override void OnPick() {
            IsCompleted = true;
            gameObject.SetActive(false);

            animator.SetTrigger("ObjectiveFound");
        }

        public override void Restart() {
            IsCompleted = false;
            base.Restart();
        }

    }
}