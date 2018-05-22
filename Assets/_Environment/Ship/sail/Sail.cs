using Randolph.Interactable;
using System.Linq;
using UnityEngine;

namespace Randolph.Environment {
    public class Sail : MonoBehaviour {
        [SerializeField] private Vector2 destination = new Vector2(-100f, 50f);
        [SerializeField] private float speed = 20f;
        [SerializeField] private TiedRope[] ropes;
        private Animator animator;


        private void Start() { animator = GetComponent<Animator>(); }

        private void Update() {
            if (animator.GetBool("CutOff")) {
                IncrementPosition();
            }
        }

        private void IncrementPosition() {
            var delta = speed * Time.deltaTime;

            Vector3 newPosition = Vector2.MoveTowards(transform.position, destination, delta);
            if (newPosition == transform.position) {
                Destroy(gameObject);
            }
            transform.position = newPosition;
            transform.Rotate(Vector3.forward, speed * delta * delta);
        }

        public void Slash(TiedRope rope) {
            ropes = ropes.Where(x => x != rope).ToArray();

            if (ropes.Length == 0) {
                animator.SetBool("CutOff", true);
            }
        }
    }
}
