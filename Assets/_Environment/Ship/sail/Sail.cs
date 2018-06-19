using System;
using Randolph.Interactable;
using System.Linq;
using UnityEngine;
using Randolph.Levels;

namespace Randolph.Environment {
    public class Sail : MonoBehaviour, IRestartable {
        [SerializeField] private Vector2 destination = new Vector2(-100f, 50f);
        [SerializeField] private float speed = 20f;
        [SerializeField] private TiedRope[] ropes;
        private Animator animator;


        private void Start()
        {
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

        #region IRestartable
        private bool initialActiveState;
        protected Vector3 initialPosition { get; private set; }
        private Quaternion initialRotation;

        public void SaveState()
        {
            initialActiveState = gameObject.activeSelf;
            initialPosition = transform.position;
            initialRotation = transform.rotation;
        }

        public void Restart()
        {
            animator.SetBool("CutOff", false);
            gameObject.SetActive(initialActiveState);
            transform.position = initialPosition;
            transform.rotation = initialRotation;
        }
        #endregion

        public event Action OnSlash;
    }
}
