using Randolph.Core;
using Randolph.Interactable;
using Randolph.Levels;
using Randolph.UI;
using UnityEngine;

namespace Randolph.Characters {
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(DistanceJoint2D))]
    public class PlayerController : MonoBehaviour {

        [Header("Movement")]
        [SerializeField] AudioClip deathSound;
        [SerializeField] float climbingSpeed = 6;
        [SerializeField] float movementSpeed = 6;
        [SerializeField] float jumpForce = 600;

        [Header("Jumping")]
        [SerializeField] LayerMask groundLayers;
        [SerializeField, Range(2, 12)] int groundRayCount = 4;
        [SerializeField] float groundCheckRayLength = 0.2f;
        float groundRaySpacing;
        const float SkinWidth = 0.015f; // overlapping tolerance    
        RaycastOrigins2D raycastOrigins;
        new Collider2D collider;

        bool isOnGround = false;
        bool jump = false;
        float gravity = 0;

        int currentLadder = 0;
        bool isClimbing = false;

        Animator animator;
        Rigidbody2D rbody;
        DistanceJoint2D grapplingJoint;
        LineRenderer grappleRopeRenderer;

        bool IsGrappled {
            get { return grapplingJoint.isActiveAndEnabled; }
        }

        void Awake() {
            GetComponents();
            gravity = rbody.gravityScale;
            Clickable.OnMouseDownClickable += OnMouseDownClickable;
            CalculateRaySpacing();
        }
        

        void GetComponents() {
            animator = GetComponent<Animator>();
            rbody = GetComponent<Rigidbody2D>();
            collider = GetComponent<Collider2D>();
            grapplingJoint = GetComponent<DistanceJoint2D>();
            grappleRopeRenderer = GetComponent<LineRenderer>();
        }

        void Update() {
            jump = Input.GetButtonDown("Jump");

            //! Debug
            DebugCommands();
        }

        void FixedUpdate() {
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");

            isOnGround = GroundCheck(groundCheckRayLength, groundLayers);

            Moving(horizontal);
            Jumping();
            Climbing(vertical, horizontal);
            Grappling(vertical, horizontal);
            Flipping();
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
                if (currentLadder <= 0) {
                    //rbody.velocity = new Vector2(rbody.velocity.x, 0); // Stop hopping at the end of a ladder
                    IgnoreCollision(false);
                    StopClimbing();
                }
            }
        }

        public void Kill(float delay = 0.25f) {
            StopGrappling();
            StopClimbing();
            AudioPlayer.audioPlayer.PlayGlobalSound(deathSound);
            LevelManager.levelManager.ReturnToCheckpoint(delay);
        }

        // TODO: Break into components

        #region Move

        private void Moving(float horizontal) {
            float hSpeed = horizontal * movementSpeed;

            animator.SetBool("Running", isOnGround && !Mathf.Approximately(hSpeed, 0f));
            animator.SetFloat("RunningSpeed", Mathf.Abs(hSpeed));

            if (!isClimbing && !IsGrappled) {
                // Left and right movement on the ground
                rbody.velocity = new Vector2(hSpeed, rbody.velocity.y);
            }
        }

        private void Flipping() {
            if ((rbody.velocity.x > 0 && transform.localScale.x < 0) || (rbody.velocity.x < 0 && transform.localScale.x > 0)) {
                Vector3 scale = transform.localScale;
                scale.x *= -1;
                transform.localScale = scale;
            }
        }

        #endregion

        #region Jump

        private void Jumping() {
            if (!jump) {
                return;
            }

            if (isOnGround || IsGrappled) {
                StopGrappling();
                JumpUp();
            }
        }

        private void JumpUp() {
            jump = false;
            rbody.velocity = new Vector2(rbody.velocity.x, 0);
            rbody.AddForce(Vector2.up * jumpForce);
            animator.SetTrigger("Jump");
        }

        #endregion

        #region Raycasting

        public struct RaycastOrigins2D {

            public Vector2 bottomLeft;
            public Vector2 bottomRight;

        }

        public bool GroundCheck(float rayLength, int layerMask) {
            UpdateRaycastOrigins();

            Vector2 rayOrigin = raycastOrigins.bottomLeft;
            for (int i = 0; i < groundRayCount; i++) {
                RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.down, rayLength, layerMask);
                // Debug.DrawRay(rayOrigin, Vector2.down * rayLength, Color.red);
                if (hit.collider) {
                    return true;
                }

                rayOrigin.x += groundRaySpacing;
            }

            return false;
        }

        public void CalculateRaySpacing() {
            Bounds bounds = collider.bounds;
            bounds.Expand(Constants.RaycastBoundsShrinkage);

            groundRaySpacing = bounds.size.x / (groundRayCount - 1);
        }

        void UpdateRaycastOrigins() {
            Bounds bounds = collider.bounds;
            bounds.Expand(Constants.RaycastBoundsShrinkage);

            raycastOrigins.bottomLeft = new Vector2(bounds.min.x, bounds.min.y);
            raycastOrigins.bottomRight = new Vector2(bounds.max.x, bounds.min.y);
        }

        #endregion

        #region Climb

        void Climbing(float vertical, float horizontal) {
            if (!isClimbing && currentLadder > 0 && Mathf.Abs(vertical) > Mathf.Epsilon) {
                isClimbing = true;
                rbody.gravityScale = 0;

                IgnoreCollision(true);

                animator.SetBool("Climbing", true);
                animator.SetFloat("ClimbingSpeed", 0);
            }

            if (isClimbing) {
                float vSpeed = vertical * climbingSpeed;
                float hSpeed = horizontal * climbingSpeed;
                rbody.velocity = new Vector2(hSpeed, vSpeed);
                animator.SetFloat("ClimbingSpeed", vSpeed);
            }
        }

        void StopClimbing(float vSpeed = 1f) {
            isClimbing = false;
            rbody.gravityScale = gravity;
            if (gameObject.activeSelf) animator.SetBool("Climbing", false);
        }

        void IgnoreCollision(bool ignore) {
            Methods.IgnoreLayerMaskCollision(Constants.Layer.Player, groundLayers, ignore);
        }

        #endregion

        #region Grapple

        public void GrappleTo(GameObject obj) {
            grapplingJoint.connectedAnchor = obj.transform.position;
            grapplingJoint.enabled = true;
            grapplingJoint.distance = Vector2.Distance(transform.position, obj.transform.position);
            grappleRopeRenderer.enabled = true;
            grappleRopeRenderer.SetPositions(new Vector3[] { transform.position, obj.transform.position });
        }

        private void Grappling(float vertical, float horizontal) {
            if (!IsGrappled) {
                return;
            }

            rbody.velocity = new Vector2(rbody.velocity.x + horizontal * 0.2f, rbody.velocity.y);
            rbody.velocity -= new Vector2(rbody.velocity.x * 0.01f, rbody.velocity.y * 0.01f);
            grapplingJoint.distance -= vertical * 0.1f;
            grappleRopeRenderer.SetPosition(0, transform.position);
        }

        private void StopGrappling() {
            grapplingJoint.enabled = false;
            grappleRopeRenderer.enabled = false;
        }

        #endregion

        #region MouseEvents

        void OnMouseDownClickable(Cursors cursortype, Constants.MouseButton button, Vector3 position) {
            float distance = 0;
        }

        #endregion

        #region Debug

        void DebugCommands() {
            if (Input.GetKeyDown(KeyCode.R)) {
                // DEBUG: Restart
                Kill();
            }

            if (Input.GetKeyDown(KeyCode.KeypadPlus)) {
                // DEBUG: Go to the next room
                var checkpointContainer = FindObjectOfType<CheckpointContainer>();
                Checkpoint nextCheckpoint = checkpointContainer.GetNext();
                if (nextCheckpoint) {
                    transform.position = nextCheckpoint.transform.position;
                    checkpointContainer.SetReached(nextCheckpoint);
                }
            }

            if (Input.GetKeyDown(KeyCode.KeypadMinus)) {
                // DEBUG: Go to the previous room
                var checkpointContainer = FindObjectOfType<CheckpointContainer>();
                Checkpoint previousCheckpoint = checkpointContainer.GetPrevious();
                if (previousCheckpoint) {
                    transform.position = previousCheckpoint.transform.position;
                    checkpointContainer.SetReached(previousCheckpoint);
                }
            }
        }

        #endregion

    }
}