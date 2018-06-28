using System;
using System.Linq;
using Randolph.Interactable;
using Randolph.Levels;
using UnityEngine;

namespace Randolph.Environment {
    public class Sail : MonoBehaviour, IRestartable {
        private Animator animator;
        [SerializeField] private Vector2 destination = new Vector2(-100f, 50f);
        [SerializeField] private TiedRope[] ropes;
        [SerializeField] private float speed = 20f;


        private void Start() {
            SaveState();
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
        private bool initialActiveState;
        private Vector3 initialPosition;
        private Quaternion initialRotation;
        private TiedRope[] initialRopes;

        public void SaveState() {
            initialActiveState = gameObject.activeSelf;
            initialPosition = transform.position;
            initialRotation = transform.rotation;
            initialRopes = ropes;
        }

        public void Restart() {
            animator.SetBool("CutOff", false);
            gameObject.SetActive(initialActiveState);
            transform.position = initialPosition;
            transform.rotation = initialRotation;
            ropes = initialRopes;
        }
        #endregion
    }
}
