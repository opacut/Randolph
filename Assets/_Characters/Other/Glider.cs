using System.Collections.Generic;
using Randolph.Core;
using Randolph.Levels;
using UnityEngine;

#pragma warning disable 414

namespace Randolph.Characters {
    [RequireComponent(typeof(Collider2D))]
    public class Glider : RestartableBase {
        // TODO: Replace queue with iterator, refactor

        public delegate void DestinationChange(Vector2 position, Vector2 nextDestination);

        public delegate void GlidingEnd(Vector2 position);

        public delegate void PlayerDisturbed();

        private Animator animator;
        [SerializeField] private bool continuous;
        private Vector2 currentDestination;
        private Queue<Vector2> destinationQueue = new Queue<Vector2>();
        [SerializeField] private List<Vector2> destinations = new List<Vector2>();
        [SerializeField] private bool loop;

        [SerializeField] private bool movesFromStart;

        [SerializeField] private float speed = 20;
        private SpriteRenderer spriteRenderer;
        [SerializeField] private bool straightLines;
        public bool Disturbed { get; private set; }

        public event GlidingEnd OnGlidingEnd;
        public event DestinationChange OnDestinationChange;
        public event PlayerDisturbed OnPlayerDisturbed;

        private void Awake() {
            animator = GetComponent<Animator>();
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        protected override void Start() {
            base.Start();

            if (movesFromStart) {
                destinations.Insert(0, transform.position);
            }

            CreateDestinationQueue();
            if (movesFromStart) {
                StartMoving();
            }
        }

        private void Update() {
            // If the object is not at the target destination
            if ((Vector2) transform.position != currentDestination) {
                // Move towards the destination each frame until the object reaches it
                IncrementPosition();
            } else if (continuous && Disturbed) {
                SetNextDestination();
            } else {
                animator.SetBool("Flying", false);
            }
        }

        private void OnCollisionEnter2D(Collision2D collision) {
            // TODO: Move out of Glider script
            if (collision.gameObject.tag == Constants.Tag.Player && (!Disturbed || !continuous)) {
                OnPlayerDisturbed?.Invoke();
                spriteRenderer.flipX = !spriteRenderer.flipX;
                StartMoving();
            }
        }

        private void OnTriggerEnter2D(Collider2D other) {
            // TODO: Move out of Glider script
            if (other.tag == Constants.Tag.Player && (!Disturbed || !continuous)) {
                OnPlayerDisturbed?.Invoke();
                spriteRenderer.flipX = !spriteRenderer.flipX;
                StartMoving();
            }
        }

        private void StartMoving() {
            Disturbed = true;
            animator.SetBool("Move", true);
            SetNextDestination();
        }

        public void Kill() { gameObject.SetActive(false); }

        private void CreateDestinationQueue() {
            // Add all destinations to queue            
            destinationQueue = new Queue<Vector2>(destinations);

            // Set the destination to be the object's initial position so it will not start off moving            
            currentDestination = transform.position;
        }

        /// <summary>Moves the object one step towards its destination.</summary>
        private void IncrementPosition() {
            // Calculate the next position
            var delta = speed * Time.deltaTime;

            // Move the object to the next position
            transform.position = Vector2.MoveTowards(transform.position, currentDestination, delta);
        }

        /// <summary>Sets the destination to the next one.</summary>
        private void SetNextDestination() {
            if (destinationQueue.Count > 0) {
                currentDestination = destinationQueue.Dequeue();
                OnDestinationChange?.Invoke(transform.position, currentDestination);
            } else if (loop) {
                CreateDestinationQueue();
                OnDestinationChange?.Invoke(transform.position, currentDestination);
            } else {
                // Invoke the event of ended gliding
                OnGlidingEnd?.Invoke(transform.position);
            }
        }

        #region IRestartable
        public override void Restart() {
            base.Restart();

            // TODO: Test if working properly
            CreateDestinationQueue();
            Disturbed = false;
            if (movesFromStart) {
                StartMoving();
            }
        }
        #endregion

        private void OnDrawGizmosSelected() {
            var queueCopy = new Queue<Vector2>(destinations);
            if (loop) {
                queueCopy.Enqueue(transform.position);
            }

            if (queueCopy.Count > 0) {
                Gizmos.color = Color.cyan;

                Vector2 startPoint = transform.position;
                while (queueCopy.Count > 0) {
                    var destination = queueCopy.Dequeue();

                    Gizmos.DrawSphere(startPoint, Constants.GizmoSphereRadius);
                    Gizmos.DrawLine(startPoint, destination);
                    Gizmos.DrawSphere(destination, Constants.GizmoSphereRadius);

                    startPoint = destination;
                }
            }
        }
    }
}
