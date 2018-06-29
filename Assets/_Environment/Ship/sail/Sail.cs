using System;
using System.Linq;
using Randolph.Interactable;
using Randolph.Levels;
using UnityEngine;

namespace Randolph.Environment {
    public class Sail : RestartableBase {
        private Animator animator;
        [SerializeField] private Vector2 destination = new Vector2(-100f, 50f);
        [SerializeField] private TiedRope[] ropes;
        [SerializeField] private float speed = 20f;


        private void Awake() {
            animator = GetComponent<Animator>();
        }

        private void Update() {
            if (animator.GetBool("CutOff")) {
                IncrementPosition();
            }
        }

        private void IncrementPosition() {
            var delta = speed * Time.deltaTime;

            Vector3 newPosition = Vector2.MoveTowards(transform.position, destination, delta);
            /*
            if (newPosition == transform.position) {
                Destroy(gameObject);
            }
            */
            transform.position = newPosition;
            transform.Rotate(Vector3.forward, speed * delta * delta);
        }

        public void Slash(TiedRope rope) {
            ropes = ropes.Where(x => x != rope).ToArray();
            OnSlash?.Invoke();

            if (ropes.Length == 0) {
                animator.SetBool("CutOff", true);
            }
        }

        public event Action OnSlash;

        #region IRestartable
        private TiedRope[] initialRopes;

        public override void SaveState() {
            base.SaveState();

            initialRopes = ropes;
        }

        public override void Restart() {
            base.Restart();
            
            animator.SetBool("CutOff", false);
            ropes = initialRopes;
        }
        #endregion
    }
}
