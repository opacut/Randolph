using UnityEditor;

using UnityEngine;

namespace Randolph.Environment {
    [ExecuteInEditMode]
    [RequireComponent(typeof(Collider2D))]
    public class Ladder : MonoBehaviour {

        [Help("Move the ladder to a platform to automatically assign a collider.", MessageType.Info)]
        public Collider2D attachedPlatform; // TODO: Convert to one-sided collider

        new Collider2D collider;
        Vector3 ladderTop;

        void Awake() {
            collider = GetComponent<Collider2D>();
            ladderTop = RecalculateLadderTop();
        }

        void Update() {
            if (transform.hasChanged) {
                if (!collider) collider = GetComponent<Collider2D>();
                ladderTop = RecalculateLadderTop();
                attachedPlatform = GetAttachedPlatform();
            }
        }

        private Vector3 RecalculateLadderTop() {
            return collider.bounds.center + (collider.bounds.size.y / 2f) * Vector3.up;
        }

        private Collider2D GetAttachedPlatform() {
            var overlappingColliders = new Collider2D[10];
            if (collider.OverlapCollider(new ContactFilter2D() {layerMask = Core.Constants.GroundLayer}, // TODO: Put "Ground" LayerMask somewhere else
                    overlappingColliders)
                > 0) {
                foreach (Collider2D col in overlappingColliders) {
                    if (col && col.OverlapPoint(ladderTop)) return col;
                }

                return null;
            } else return null;
        }

        void OnDrawGizmos() {
            float dotRadius = 0.25f;
            Vector3 startPoint = ladderTop;

            if (!attachedPlatform) {
                Gizmos.color = Color.red;
                Gizmos.DrawSphere(startPoint, dotRadius);
                return;
            } else {
                Gizmos.color = Color.green;
                Vector3 endPoint = attachedPlatform.transform.position;

                Gizmos.DrawSphere(startPoint, dotRadius);
                Gizmos.DrawLine(startPoint, endPoint);
                Gizmos.DrawSphere(endPoint, dotRadius);
            }
        }

    }
}
