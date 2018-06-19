using System;
using System.Threading.Tasks;
using Randolph.Core;
using Randolph.Interactable;
using Randolph.Levels;
using Randolph.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Randolph.Characters {
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(DistanceJoint2D))]
    public class PlayerController : MonoBehaviour {
        private const float SkinWidth = 0.015f; // overlapping tolerance    

        private Animator animator;
        [SerializeField][ReadonlyField] private Text bubbleText;
        [SerializeField] private float climbingSpeed = 6;
        private new Collider2D collider;

        private int currentLadder;

        [Header("Movement")][SerializeField] private AudioClip deathSound;

        private LineRenderer grappleRopeRenderer;
        private DistanceJoint2D grapplingJoint;
        private float gravity;
        [SerializeField] private float groundCheckRayLength = 0.2f;

        [Header("Jumping")][SerializeField] private LayerMask groundLayers;

        [SerializeField][Range(2, 12)] private int groundRayCount = 4;

        private float groundRaySpacing;
        private bool isClimbing;


        private bool isOnGround;
        private bool jump;
        [SerializeField] private float jumpForce = 600;

        [Header("Grappling")][SerializeField] private float maximumRopeLength = 15f;

        [SerializeField] private float movementSpeed = 6;
        private RaycastOrigins2D raycastOrigins;
        private Rigidbody2D rbody;
        [SerializeField] private Canvas speechBubble;

        [Header("Interaction")][SerializeField][Range(0, 5)]
        private float speechDuration = 1.5f;

        private SpriteRenderer spriteRenderer;

        private bool IsGrappled => grapplingJoint.isActiveAndEnabled;

        public bool Killable { get; set; } = true;

        private void Awake() {
            GetComponents();
            gravity = rbody.gravityScale;
            Clickable.OnMouseDownClickable += OnMouseDownClickable;
            InventoryIcon.OnMouseDownClickable += OnMouseDownClickable;
            CalculateRaySpacing();
        }

        private void GetComponents() {
            animator = GetComponent<Animator>();
            rbody = GetComponent<Rigidbody2D>();
            collider = GetComponent<Collider2D>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            grapplingJoint = GetComponent<DistanceJoint2D>();
            grappleRopeRenderer = GetComponent<LineRenderer>();

            if (speechBubble != null) {
                bubbleText = speechBubble?.GetComponentInChildren<Text>();
                if (bubbleText != null) {
                    bubbleText.text = "";
                }
            }
        }

        private void Update() {
            jump = Input.GetButtonDown("Jump");

            //! Debug
            DebugCommands();
        }

        private void FixedUpdate() {
            var horizontal = Input.GetAxisRaw("Horizontal");
            var vertical = Input.GetAxisRaw("Vertical");

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
            if (Killable) {
                Killable = false;
                StopGrappling();
                StopClimbing();
                AudioPlayer.audioPlayer.PlayGlobalSound(deathSound);
                LevelManager.levelManager.ReturnToCheckpoint(delay);
            }
        }

        #region Freeze
        [ContextMenu("Freeze")]
        public void Freeze()
        {
            //rbody.isKinematic = true;
            //gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
            rbody.constraints = RigidbodyConstraints2D.FreezeAll;
            //animator.StopPlayback();
        }

        [ContextMenu("Unfreeze")]
        public void UnFreeze()
        {
            //rbody.isKinematic = false;
            rbody.constraints = RigidbodyConstraints2D.FreezeRotation;
            //animator.StartPlayback();
        }
        #endregion

        #region Debug
        private void DebugCommands() {
            if (Input.GetKeyDown(KeyCode.R)) {
                // DEBUG: Restart
                Kill();
            }

            if (Input.GetKeyDown(KeyCode.KeypadPlus)) {
                // DEBUG: Go to the next room
                var checkpointContainer = FindObjectOfType<CheckpointContainer>();
                var nextCheckpoint = checkpointContainer.GetNext();
                if (nextCheckpoint) {
                    transform.position = nextCheckpoint.transform.position;
                    checkpointContainer.SetReached(nextCheckpoint);
                }
            }

            if (Input.GetKeyDown(KeyCode.KeypadMinus)) {
                // DEBUG: Go to the previous room
                var checkpointContainer = FindObjectOfType<CheckpointContainer>();
                var previousCheckpoint = checkpointContainer.GetPrevious();
                if (previousCheckpoint) {
                    transform.position = previousCheckpoint.transform.position;
                    checkpointContainer.SetReached(previousCheckpoint);
                }
            }

            if (Input.GetKeyDown(KeyCode.KeypadMultiply)) {
                LevelManager.LoadNextLevel();
            }
        }
        #endregion

        // TODO: Break into components

        #region Move
        private void Moving(float horizontal) {
            var hSpeed = horizontal * movementSpeed;

            animator.SetBool("Running", isOnGround && !Mathf.Approximately(hSpeed, 0f));
            animator.SetFloat("RunningSpeed", Mathf.Abs(hSpeed));

            if (!isClimbing && !IsGrappled) {
                // Left and right movement on the ground
                rbody.velocity = new Vector2(hSpeed, rbody.velocity.y);
            }
        }

        private void Flipping() {
            if (rbody.velocity.x > 0 && spriteRenderer.flipX || rbody.velocity.x < 0 && !spriteRenderer.flipX) {
                // Look the other way (doesn't affect child objects and colliders)
                spriteRenderer.flipX = !spriteRenderer.flipX;
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

            var rayOrigin = raycastOrigins.bottomLeft;
            for (var i = 0; i < groundRayCount; i++) {
                var hit = Physics2D.Raycast(rayOrigin, Vector2.down, rayLength, layerMask);
                // Debug.DrawRay(rayOrigin, Vector2.down * rayLength, Color.red);
                if (hit.collider) {
                    return true;
                }

                rayOrigin.x += groundRaySpacing;
            }

            return false;
        }

        public void CalculateRaySpacing() {
            var bounds = collider.bounds;
            bounds.Expand(Constants.RaycastBoundsShrinkage);

            groundRaySpacing = bounds.size.x / (groundRayCount - 1);
        }

        private void UpdateRaycastOrigins() {
            var bounds = collider.bounds;
            bounds.Expand(Constants.RaycastBoundsShrinkage);

            raycastOrigins.bottomLeft = new Vector2(bounds.min.x, bounds.min.y);
            raycastOrigins.bottomRight = new Vector2(bounds.max.x, bounds.min.y);
        }
        #endregion

        #region Climb
        private void Climbing(float vertical, float horizontal) {
            if (!isClimbing && currentLadder > 0 && Mathf.Abs(vertical) > Mathf.Epsilon) {
                isClimbing = true;
                rbody.gravityScale = 0;

                IgnoreCollision(true);

                animator.SetBool("Climbing", true);
                animator.SetFloat("ClimbingSpeed", 0);
            }

            if (isClimbing) {
                var vSpeed = vertical * climbingSpeed;
                var hSpeed = horizontal * climbingSpeed;
                rbody.velocity = new Vector2(hSpeed, vSpeed);
                animator.SetFloat("ClimbingSpeed", vSpeed);
            }
        }

        private void StopClimbing(float vSpeed = 1f) {
            isClimbing = false;
            rbody.gravityScale = gravity;
            if (gameObject.activeSelf) {
                animator.SetBool("Climbing", false);
            }
        }

        private void IgnoreCollision(bool ignore) {
            Methods.IgnoreLayerMaskCollision(Constants.Layer.Player, groundLayers, ignore);
        }
        #endregion

        #region Grapple
        public void GrappleTo(Vector3 position) {
            grapplingJoint.connectedAnchor = position;
            grapplingJoint.enabled = true;
            grapplingJoint.distance = Vector2.Distance(transform.position, position);
            grappleRopeRenderer.enabled = true;
            grappleRopeRenderer.SetPositions(new[] { transform.position, position });
        }

        private void Grappling(float vertical, float horizontal) {
            if (!IsGrappled) {
                return;
            }

            rbody.velocity = new Vector2(rbody.velocity.x + horizontal * 0.2f, rbody.velocity.y);
            rbody.velocity -= new Vector2(rbody.velocity.x * 0.01f, rbody.velocity.y * 0.01f);
            grapplingJoint.distance = Math.Min(grapplingJoint.distance - vertical * 0.1f, maximumRopeLength);
            grappleRopeRenderer.SetPosition(0, transform.position);
        }

        public event Action OnStoppedGrappling;

        public void StopGrappling() {
            grapplingJoint.enabled = false;
            grappleRopeRenderer.enabled = false;
            OnStoppedGrappling?.Invoke();
        }
        #endregion

        #region MouseEvents
        private void OnMouseDownClickable(Clickable target, Constants.MouseButton button) {
            if (!target.isWithinReach) {
                return;
            }
            if (button == Constants.MouseButton.Left) {
                switch (target.CursorType) {
                case Cursors.Generic:
                    // No specific cursor -> no action
                    break;
                case Cursors.Pick:
                    var pickable = target as Pickable;
                    pickable?.Pick();
                    break;
                case Cursors.Interact:
                    var interactable = target as Interactable.Interactable; 
                    interactable?.Interact();
                    break;
                case Cursors.Talk:
                    var talkable = target as Talkable;
                    talkable?.OnTalk();
                    break;
                default:
                    Debug.LogWarning($"Unhandled clickable type: {target.CursorType}");
                    break;
                }
            } else if (button == Constants.MouseButton.Right) {
                var description = target.GetDescription();
                if (description != string.Empty) {
                    ShowDescriptionBubble(target.GetDescription(), speechDuration);
                }
            }
        }

        /// <summary>Shows a text in Randolph's speech bubble for a given time.</summary>
        /// <param name="text">Text to show.</param>
        /// <param name="duration">Duration in seconds.</param>
        public async void ShowDescriptionBubble(string text, float duration) {
            // TODO: Autoscroll long text (/ duration)

            speechBubble.gameObject.SetActive(true);
            bubbleText.text = text;
            await Task.Delay(Mathf.RoundToInt(duration * 100 * text.Length));
            if (bubbleText.text != text) {
                // Randolph is describing other item
                return;
            }
            speechBubble.gameObject.SetActive(false);
        }

        public void HideDescriptionBubble(string description) {
            if (!string.IsNullOrEmpty(description) && bubbleText.text != description) {
                // Randolph is describing other item
                return;
            }

            bubbleText.text = string.Empty;
            speechBubble.gameObject.SetActive(false);
        }
        #endregion
    }
}
