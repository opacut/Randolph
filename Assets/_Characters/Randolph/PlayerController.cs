using Randolph.Core;
using Randolph.Interactable;
using Randolph.Levels;
using UnityEngine;

namespace Randolph.Characters {
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerController : MonoBehaviour {

        [SerializeField] float climbingSpeed = 6;
        [SerializeField] float movementSpeed = 6;
        [SerializeField] float jumpForce = 800;
        [SerializeField] float fallForce = 50;

        Animator animator;
        Rigidbody2D rbody;

        float gravity = 0;
        bool jump = false;
        bool onGround = false;

        int currentLadder = 0;
        bool climbing = false;

        private void Awake() {
            animator = GetComponent<Animator>();
            rbody = GetComponent<Rigidbody2D>();
            gravity = rbody.gravityScale;
        }

        private void OnTriggerEnter2D(Collider2D other) {
            if (other.tag == Constants.Tag.Ladder) {
                ++currentLadder;
            }

            if (other.tag == Constants.Tag.Deadly) {
                Kill();
            }

            if (other.tag == Constants.Tag.Pickable) {
                var pickable = other.GetComponent<Pickable>();
                Debug.Assert(pickable, "An object with a Pickable tag doesn't have a Pickable script attached", other.gameObject);
                pickable.OnPick();
            }
        }

        private void OnTriggerExit2D(Collider2D other) {
            if (other.tag == Constants.Tag.Ladder) {
                --currentLadder;
                if (currentLadder <= 0)
                    IgnoreCollision(false);
            }
        }

        private void OnCollisionEnter2D(Collision2D other) {
            if (other.gameObject.layer == Constants.Layer.Ground) {
                onGround = true;
            }
        }

        private void OnCollisionExit2D(Collision2D other) {
            if (other.gameObject.layer == Constants.Layer.Ground) {
                onGround = false;
            }
        }

        private void Update() {
            jump = Input.GetButton("Jump");
        }

        private void FixedUpdate() {
            float vertical = Input.GetAxisRaw("Vertical");
            float horizontal = Input.GetAxisRaw("Horizontal");

            Moving(horizontal);
            Jumping(vertical);
            Climbing(vertical, horizontal);
            Flipping();
        }

        private void Moving(float horizontal) {
            float hSpeed = horizontal * movementSpeed;

            animator.SetBool("Running", onGround && !Mathf.Approximately(hSpeed, 0f));
            animator.SetFloat("RunningSpeed", Mathf.Abs(hSpeed));

            if (onGround || !climbing) {
                // Left and right movement on the ground
                rbody.velocity = new Vector2(hSpeed, rbody.velocity.y);
            }
        }

        private void Jumping(float vertical) {
            if (jump && onGround) {
                // Jumping while on ground or climbing
                StopClimbing();
                JumpUp();
            }

            if (!onGround && !climbing && rbody.velocity.y < 0) {
                rbody.AddForce(Vector2.down * fallForce);
            }
        }

        private void JumpUp() {
            jump = onGround = false;
            rbody.AddForce(Vector2.up * jumpForce);
            animator.SetTrigger("Jump");
        }

        void Climbing(float vertical, float horizontal = 0f) {
            if (!climbing && currentLadder > 0 && Mathf.Abs(vertical) > 0.001f) {
                climbing = true;
                rbody.gravityScale = 0;

                IgnoreCollision(true);

                animator.SetBool("Climbing", true);
                animator.SetFloat("ClimbingSpeed", 0);
            }

            if (climbing) {
                float vSpeed = vertical * climbingSpeed;
                float hSpeed = horizontal * climbingSpeed;

                rbody.velocity = new Vector2(hSpeed, vSpeed);

                animator.SetFloat("ClimbingSpeed", vSpeed);

                if (currentLadder <= 0 || onGround) {
                    StopClimbing(vSpeed);
                }
            }
        }

        private void StopClimbing(float vSpeed = 1f) {
            climbing = false;
            rbody.gravityScale = gravity;
            if (!onGround) {
                rbody.AddForce(Vector2.up * vSpeed);
            }
            animator.SetBool("Climbing", false);
        }

        private void Flipping() {
            if ((rbody.velocity.x > 0 && transform.localScale.x < 0) || (rbody.velocity.x < 0 && transform.localScale.x > 0)) {
                Vector3 scale = transform.localScale;
                scale.x *= -1;
                transform.localScale = scale;
            }
        }

        private void IgnoreCollision(bool ignore) {
            Physics2D.IgnoreLayerCollision(Constants.Layer.Player, Constants.Layer.Ground, ignore);
        }

        public void Kill(float delay = 0.25f) {
            LevelManager.levelManager.ReturnToCheckpoint(delay);
            gameObject.SetActive(false);
        }

    }
}