using UnityEngine;

using Randolph.Environment;
using Randolph.Interactable;
using Randolph.Levels;

namespace Randolph.Characters {
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerController : MonoBehaviour {

        [SerializeField] float skin = 1.3f;
        [SerializeField] float climbingSpeed = 6;
        [SerializeField] float movementSpeed = 6;
        [SerializeField] float jumpForce = 800;
        [SerializeField] float fallForce = 50;
        [SerializeField] LayerMask groundLayer;

        const string LadderTag = "Ladder";
        const string PickableTag = "Pickable";

        Animator animator;
        Rigidbody2D rbody;
        new Collider2D collider;

        float gravity = 0;
        bool jump = false;
        bool onGround = false;

        Ladder currentLadder = null;
        bool climbing = false;

        private void Awake() {
            animator = GetComponent<Animator>();
            rbody = GetComponent<Rigidbody2D>();
            collider = GetComponent<Collider2D>();
            gravity = rbody.gravityScale;
        }

        private void OnTriggerEnter2D(Collider2D other) {
            if (other.tag == LadderTag) {
                var ladder = other.GetComponent<Ladder>();
                Debug.Assert(ladder, "An object with a Ladder tag doesn't have a Ladder script attached.", other.gameObject);
                currentLadder = ladder;
            }

            if (other.tag == PickableTag) {
                var pickable = other.GetComponent<Pickable>();
                Debug.Assert(pickable, "An object with a Pickable tag doesn't have a Pickable script attached", other.gameObject);
                pickable.OnPick();
            }
        }

        private void OnTriggerExit2D(Collider2D other) {
            if (other.tag == LadderTag) {
                IgnoreCollision(currentLadder.attachedPlatform, false);
                currentLadder = null;
            }
        }

        private void Update() {
            if (Input.GetButtonDown("Jump")) {
                jump = true;
            }
        }

        private void FixedUpdate() {
            GroundCheck();

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
            if (jump && (climbing || onGround && Mathf.Approximately(rbody.velocity.y, 0f))) {
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
            // TODO: Move from ladder sideways

            if (!climbing && currentLadder && Mathf.Abs(vertical) > 0.001f) {
                climbing = true;
                rbody.gravityScale = 0;

                IgnoreCollision(currentLadder.attachedPlatform, true);

                animator.SetBool("Climbing", true);
                animator.SetFloat("ClimbingSpeed", 0);
            }

            if (climbing) {
                float vSpeed = vertical * climbingSpeed;
                float hSpeed = horizontal * climbingSpeed;

                rbody.velocity = new Vector2(hSpeed, vSpeed);

                animator.SetFloat("ClimbingSpeed", vSpeed);

                if (!currentLadder || onGround) {
                    StopClimbing(vSpeed);
                }
            }
        }

        private void StopClimbing(float vSpeed = 1f) {
            climbing = false;
            rbody.gravityScale = gravity;
            if (!onGround) rbody.AddForce(Vector2.up * vSpeed);

            animator.SetBool("Climbing", false);
        }

        private void Flipping() {
            if ((rbody.velocity.x > 0 && transform.localScale.x < 0) || (rbody.velocity.x < 0 && transform.localScale.x > 0)) {
                Vector3 scale = transform.localScale;
                scale.x *= -1;
                transform.localScale = scale;
            }
        }

        private void GroundCheck() {
            Debug.DrawRay(transform.position, Vector2.down * skin, Color.green);
            Collider2D coll = Physics2D.Raycast(transform.position, Vector2.down, skin, Randolph.Core.Constants.GroundLayer).collider;
            onGround = coll && rbody.IsTouching(coll);
        }

        private void IgnoreCollision(Collider2D other, bool ignore) {
            Physics2D.IgnoreCollision(collider, other, ignore);
        }

        public void Kill(float delay = 0.25f) {
            LevelManager.levelManager.ReturnToCheckpoint(delay);
            gameObject.SetActive(false);
        }

    }
}