using System.Collections.Generic;

using UnityEngine;

using Randolph.Levels;

namespace Randolph.Characters {
    public class Glider : MonoBehaviour, IRestartable {

        [SerializeField] float speed = 20;
        [SerializeField] List<Vector2> destinations = new List<Vector2>();

        Queue<Vector2> destinationQueue = new Queue<Vector2>();
        Vector2 currentDestination;

        Vector3 initialPosition;

        [SerializeField] bool loop = false;
        [SerializeField] bool continuous = false;
        bool disturbed = false;

        public delegate void OnGlidingEnd(Vector3 position);

        public event OnGlidingEnd onGlidingEnd;

        void Start() {
            initialPosition = transform.position;
            CreateDestinationQueue();
        }

        void Update() {
            // If the object is not at the target destination
            if ((Vector2) transform.position != currentDestination) {
                // Move towards the destination each frame until the object reaches it
                IncrementPosition();
            } else if (continuous && disturbed) {
                SetNextDestination();
            }
        }

        void OnTriggerEnter2D(Collider2D other) {
            if (other.tag == "Player") {
                disturbed = true;
                SetNextDestination();
            }
        }

        public void Kill() {
            gameObject.SetActive(false);
        }

        public void Restart() {
            transform.position = initialPosition;
            CreateDestinationQueue();
            disturbed = false;
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
            } else if (loop) {
                CreateDestinationQueue();
            } else {
                // Invoke the event of ended gliding
                onGlidingEnd?.Invoke(transform.position);
            }
        }

        void OnDrawGizmosSelected() {
            initialPosition = transform.position;
            var queueCopy = new Queue<Vector2>(destinations);
            if (loop) queueCopy.Enqueue(initialPosition);

            if (queueCopy.Count > 0) {
                Gizmos.color = Color.cyan;

                Vector2 startPoint = initialPosition;
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
