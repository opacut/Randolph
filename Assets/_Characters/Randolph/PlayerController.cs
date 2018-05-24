using System.Threading.Tasks;
using Randolph.Core;
﻿using System;
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

        [Header("Movement")]
        [SerializeField] AudioClip deathSound;
        [SerializeField] float climbingSpeed = 6;
        [SerializeField] float movementSpeed = 6;
        [SerializeField] float jumpForce = 600;

        [Header("Jumping")]
        [SerializeField] LayerMask groundLayers;
        [SerializeField, Range(2, 12)] int groundRayCount = 4;
        [SerializeField] float groundCheckRayLength = 0.2f;
        
        [Header("Grappling")]
        [SerializeField] float maximumRopeLength = 15f;

        float groundRaySpacing;
        const float SkinWidth = 0.015f; // overlapping tolerance    
        RaycastOrigins2D raycastOrigins;
        new Collider2D collider;

        [Header("Interaction")] [SerializeField, Range(0, 5)] float speechDuration = 1.5f;
        [SerializeField] Canvas speechBubble;
        [SerializeField, ReadonlyField] Text bubbleText;
        bool isDisplaying = false;


        bool isOnGround = false;
        bool jump = false;
        float gravity = 0;

        int currentLadder = 0;
        bool isClimbing = false;

        Animator animator;
        Rigidbody2D rbody;
        SpriteRenderer spriteRenderer;
        DistanceJoint2D grapplingJoint;
        LineRenderer grappleRopeRenderer;

        bool IsGrappled => grapplingJoint.isActiveAndEnabled;

        public bool Killable { get; set; } = true;

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
            spriteRenderer = GetComponent<SpriteRenderer>();
            grapplingJoint = GetComponent<DistanceJoint2D>();
            grappleRopeRenderer = GetComponent<LineRenderer>();

            if (speechBubble != null) {
                bubbleText = speechBubble?.GetComponentInChildren<Text>();
                if (bubbleText != null) bubbleText.text = "";
            }
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

            /*
            if (other.tag == Constants.Tag.Pickable) {
                // TODO: Remove to make items only on click
                var pickable = other.GetComponent<Pickable>();
                Debug.Assert(pickable, "An object with a Pickable tag doesn't have a Pickable script attached", other.gameObject);
                pickable.OnPick();
            }
            */
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

        void Flipping() {
            if ((rbody.velocity.x > 0 && spriteRenderer.flipX) || (rbody.velocity.x < 0 && !spriteRenderer.flipX)) {
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

        public void GrappleTo(Vector3 position) {
            grapplingJoint.connectedAnchor = position;
            grapplingJoint.enabled = true;
            grapplingJoint.distance = Vector2.Distance(transform.position, position);
            grappleRopeRenderer.enabled = true;
            grappleRopeRenderer.SetPositions(new Vector3[] {transform.position, position});
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

        void OnMouseDownClickable(Clickable target, Constants.MouseButton button) {
            bool withinDistance = Vector2.Distance(transform.position, target.transform.position) <= Inventory.inventory.ApplicableDistance;

            if (withinDistance) {
                if (button == Constants.MouseButton.Left) {
                    switch (target.CursorType) {
                        case Cursors.Generic:
                            // No specific cursor -> no action
                            break;
                        case Cursors.Pick:
                            var pickable = (Pickable) target;
                            pickable.OnPick();
                            break;
                        case Cursors.Interact:
                            var interactable = (Interactable.Interactable) target;
                            interactable.OnInteract();
                            break;
                        case Cursors.Talk:
                            var talkable = (Talkable) target;
                            talkable.OnTalk();
                            break;
                        default:
                            Debug.LogWarning($"Unhandled clickable type: {target.CursorType}");
                            break;
                    }
                } else if (button == Constants.MouseButton.Right) {
                    if (!isDisplaying) {
                        isDisplaying = true;
                        string description = target.GetDescription();
                        if (description != string.Empty) ShowDescriptionBubble(target.GetDescription(), speechDuration);
                    }
                }
            }
        }

        /// <summary>Shows a text in Randolph's speech bubble for a given time.</summary>
        /// <param name="text">Text to show.</param>
        /// <param name="duration">Duration in seconds.</param>
        async void ShowDescriptionBubble(string text, float duration) {
            // TODO: Move the bubble within screen bounds
            Vector2 originalOffset = speechBubble.transform.position;
            // TODO: Autoscroll long text (/ duration)

            speechBubble.gameObject.SetActive(true);
            bubbleText.text = text;
            await Task.Delay(Mathf.RoundToInt(duration * 1000));
            speechBubble.gameObject.SetActive(false);
            isDisplaying = false;

            speechBubble.transform.position = originalOffset;
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

            if (Input.GetKeyDown(KeyCode.KeypadMultiply)) {
                LevelManager.LoadNextLevel();
            }
        }

        #endregion

    }
}
