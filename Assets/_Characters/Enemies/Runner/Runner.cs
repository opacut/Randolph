using Randolph.Core;
using Randolph.Interactable;
using UnityEngine;

namespace Randolph.Characters {
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(Animator))]
    public class Runner : MonoBehaviour, IEnemy {
        private Animator animator;

        [SerializeField]
        private Sprite dead;

        private int goingRight = 1;

        [SerializeField]
        private float speed = 10f;

        private SpriteRenderer spriteRenderer;

        private bool alive {
            get { return animator.GetBool("alive"); }
            set { animator.SetBool("alive", value); }
        }

        public void Kill() {
            alive = false;
            spriteRenderer.sprite = dead;
            //? Collider size
            gameObject.layer = Constants.Layer.Dead;
        }

        private void Awake() {
            spriteRenderer = GetComponent<SpriteRenderer>();
            animator = GetComponent<Animator>();
            SaveState();
        }

        private void Update() {
            if (!alive) {
                return;
            }
            transform.Translate(goingRight * Vector2.right * speed * Time.deltaTime);
        }

        private void OnTriggerEnter2D(Collider2D other) {
            if (!alive || other.isTrigger || other.GetComponent<Boulder>() != null || other.tag == Constants.Tag.Player) {
                return;
            }
            Turn();
        }

        private void OnCollisionEnter2D(Collision2D collision) {
            if (!alive) {
                return;
            }
            
            var collider = collision.collider;
            if (collider.tag == Constants.Tag.Player) {
                collider.gameObject.GetComponent<PlayerController>().Kill(1);
                return;
            }

            var boulder = collider.gameObject.GetComponent<Boulder>();
            if (boulder != null) {
                if (boulder.HitFromAbove(collision)) {
                    boulder.PlayCrushSound();
                    Kill();
                } else {
                    boulder.Push(collision);
                    Turn();
                }
            }
        }

        private void Turn() {
            goingRight *= -1;

            var scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
        }

        #region IRestartable
        private Vector3 initialPosition;
        private Quaternion initialRotation;

        public void SaveState() {
            initialPosition = transform.position;
            initialRotation = transform.rotation;
        }

        public void Restart() {
            transform.position = initialPosition;
            transform.rotation = initialRotation;
            alive = true;
            goingRight = 1;
        }
        #endregion
    }
}
