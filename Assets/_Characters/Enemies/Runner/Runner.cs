using UnityEngine;

using Randolph.Core;
using Randolph.Interactable;

namespace Randolph.Characters {
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(Animator))]
    public class Runner : MonoBehaviour, IEnemy {

        [SerializeField] Sprite dead;
        [SerializeField] float speed = 10f;        

        bool alive = true;
        int goingRight;

        Animator animator;
        Vector3 initialPosition;
        Quaternion initialRotation;

        SpriteRenderer spriteRenderer;

        void Awake() {
            initialPosition = gameObject.transform.position;
            initialRotation = gameObject.transform.rotation;
            spriteRenderer = GetComponent<SpriteRenderer>();
            animator = GetComponent<Animator>();
        }

        void Start() {
            goingRight = 1;
        }

        void Update() {
            if (!alive) return;
            transform.Translate(goingRight * Vector2.right * speed * Time.deltaTime);
        }

        private void OnTriggerEnter2D(Collider2D other) {
            if (!alive || other.isTrigger || other.GetComponent<Boulder>() != null || other.tag == Constants.Tag.Player) return;
            Turn();
        }

        private void OnCollisionEnter2D(Collision2D collision) {
            Collider2D collider = collision.collider;
            Vector3 contactPoint = collision.contacts[0].point;
            Vector3 center = collider.bounds.center;

            if (alive) {
                if (collider.tag == Constants.Tag.Player) {
                    collider.gameObject.GetComponent<PlayerController>().Kill(1);
                } else {
                    var squasher = collider.gameObject.GetComponent<Boulder>();
                    if (squasher != null) {
                        if (squasher.HitFromAbove(collision)) {
                            Kill();
                        } else {
                            squasher.Push(collision);
                            Turn();
                        }
                    }
                }
            }
        }

        void Turn() {
            goingRight *= -1;

            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
        }

        public void Kill() {
            alive = false;
            animator.SetBool("Alive", false);
            spriteRenderer.sprite = dead;
            // TODO: Collider size
            gameObject.layer = Constants.Layer.Dead;
        }

        public void Restart() {
            gameObject.transform.position = initialPosition;
            gameObject.transform.rotation = initialRotation;
            alive = true;
            animator.SetBool("Alive", true);
        }

    }
}
