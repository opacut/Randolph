using System.Collections.Generic;
using Randolph.Core;
using Randolph.Levels;
using UnityEngine;

#pragma warning disable 414

namespace Randolph.Characters {
    [RequireComponent(typeof(Collider2D))]
    public class Glider : MonoBehaviour, IRestartable {
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

        private void Start() {
            SaveState();
            if (movesFromStart) {
                destinations.Insert(0, initialPosition);
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
                //transform.localScale.x = 1;
            }
        }

        private void OnCollisionEnter2D(Collision2D collision) {
            if (collision.gameObject.tag == Constants.Tag.Player && (!Disturbed || !continuous)) {
                OnPlayerDisturbed?.Invoke();
                spriteRenderer.flipX = !spriteRenderer.flipX;
                StartMoving();
            }
        }

        private void OnTriggerEnter2D(Collider2D other) {
            if (other.tag == Constants.Tag.Player && (!Disturbed || !continuous)) {
                OnPlayerDisturbed?.Invoke();
                spriteRenderer.flipX = !spriteRenderer.flipX;
                StartMoving();
            }
        }

        private void StartMoving() {
            Disturbed = true;
            animator.SetBool("Flying", true);
            SetNextDestination();
        }

        public void Kill() { gameObject.SetActive(false); }

        private void CreateDestinationQueue() {
            // Add all destinations to queue            
            destinationQueue = new Queue<Vector2>(destinations);

            // Set the destination to be the object's initial position so it will not start off moving            
            SetDestination(initialPosition);
        }

        /// <summary>Moves the object one step towards its destination.</summary>
        private void IncrementPosition() {
            // Calculate the next position
            var delta = speed * Time.deltaTime;

            // Move the object to the next position
            transform.position = Vector2.MoveTowards(transform.position, currentDestination, delta);
        }

        /// <summary>Set the destination to cause the object to smoothly glide to the specified location.</summary>
        private void SetDestination(Vector2 value) { currentDestination = value; }

        /// <summary>Sets the destination to the next one.</summary>
        private void SetNextDestination() {
            if (destinationQueue.Count > 0) {
                SetDestination(destinationQueue.Dequeue());
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
        private Vector3 initialPosition;
        private Quaternion initialRotation;
        
        public void SaveState() {
            initialPosition = transform.position;
            initialRotation = transform.rotation;
        }

        public void Restart() {
            transform.position = initialPosition;
            transform.rotation = initialRotation;

            CreateDestinationQueue();
            Disturbed = false;
            if (movesFromStart) {
                StartMoving();
            }
            gameObject.SetActive(true);
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
