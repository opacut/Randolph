using System.Collections.Generic;
using UnityEngine;
using Randolph.Core;
using Randolph.Levels;

#pragma warning disable 414

namespace Randolph.Characters {
    [RequireComponent(typeof(Collider2D))]
    public class Glider : MonoBehaviour, IRestartable {

        [SerializeField] float speed = 20;
        [SerializeField] List<Vector2> destinations = new List<Vector2>();
        [SerializeField] bool straightLines = false;

        Animator animator;
        Queue<Vector2> destinationQueue = new Queue<Vector2>();
        Vector2 currentDestination;
        SpriteRenderer spriteRenderer;
        Vector3 initialPosition;        

        [SerializeField] bool movesFromStart = false;
        [SerializeField] bool loop = false;
        [SerializeField] bool continuous = false;
        public bool Disturbed { get; private set; } = false;

        public delegate void GlidingEnd(Vector2 position);
        public event GlidingEnd OnGlidingEnd;

        public delegate void DestinationChange(Vector2 position, Vector2 nextDestination);
        public event DestinationChange OnDestinationChange;

        public delegate void PlayerDisturbed(PlayerController player);
        public event PlayerDisturbed OnPlayerDisturbed;

        private void Awake()
        {
            animator = GetComponent<Animator>();
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        void Start() {
            initialPosition = transform.position;
            if (movesFromStart) destinations.Insert(0, initialPosition);

            CreateDestinationQueue();
            if (movesFromStart) StartMoving();
        }

        void Update() {
            // If the object is not at the target destination
            if ((Vector2) transform.position != currentDestination) {
                // Move towards the destination each frame until the object reaches it
                IncrementPosition();
            } else if (continuous && Disturbed) {
                SetNextDestination();
            }
            else
            {
                animator.SetBool("Flying", false);
                //transform.localScale.x = 1;
            }
        }

        void OnCollisionEnter2D(Collision2D collision) {
            if ((collision.gameObject.tag == Constants.Tag.Player) && (!Disturbed || !continuous)) {
                OnPlayerDisturbed?.Invoke(collision.gameObject.GetComponent<PlayerController>());
                spriteRenderer.flipX = !spriteRenderer.flipX;
                StartMoving();
            }
        }

        void OnTriggerEnter2D(Collider2D other) {
            if ((other.tag == Constants.Tag.Player) && (!Disturbed || !continuous)) {
                OnPlayerDisturbed?.Invoke(other.GetComponent<PlayerController>());
                spriteRenderer.flipX = !spriteRenderer.flipX;
                StartMoving();
            }
        }

        void StartMoving() {
            Disturbed = true;
            animator.SetBool("Flying", true);
            SetNextDestination();
        }

        public void Kill() {
            gameObject.SetActive(false);
        }

        public void Restart() {
            transform.position = initialPosition;
            CreateDestinationQueue();
            Disturbed = false;
            if (movesFromStart) StartMoving();
            gameObject.SetActive(true);
        }

        void CreateDestinationQueue() {
            // Add all destinations to queue            
            destinationQueue = new Queue<Vector2>(destinations);

            // Set the destination to be the object's initial position so it will not start off moving            
            SetDestination(initialPosition);
        }

        /// <summary>Moves the object one step towards its destination.</summary>
        void IncrementPosition() {
            // Calculate the next position
            float delta = speed * Time.deltaTime;

            // Move the object to the next position
            transform.position = Vector2.MoveTowards(transform.position, currentDestination, delta);

        }

        /// <summary>Set the destination to cause the object to smoothly glide to the specified location.</summary>
        void SetDestination(Vector2 value) {
            currentDestination = value;
        }

        /// <summary>Sets the destination to the next one.</summary>
        void SetNextDestination() {
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

        void OnDrawGizmosSelected() {
            var queueCopy = new Queue<Vector2>(destinations);
            if (loop) queueCopy.Enqueue(transform.position);

            if (queueCopy.Count > 0) {
                Gizmos.color = Color.cyan;

                Vector2 startPoint = transform.position;
                while (queueCopy.Count > 0) {
                    Vector2 destination = queueCopy.Dequeue();

                    Gizmos.DrawSphere(startPoint, Core.Constants.GizmoSphereRadius);
                    Gizmos.DrawLine(startPoint, destination);
                    Gizmos.DrawSphere(destination, Core.Constants.GizmoSphereRadius);

                    startPoint = destination;
                }
            }
        }

    }
}
