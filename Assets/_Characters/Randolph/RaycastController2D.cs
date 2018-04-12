using UnityEngine;

namespace Randolph.Characters {
    [RequireComponent(typeof(Collider2D))]
    public class RaycastController2D : MonoBehaviour {

        [SerializeField] LayerMask collisionMask;

        const float skinWidth = 0.015f;
        [SerializeField] int horizontalRayCount = 4;
        [SerializeField] int verticalRayCount = 4;

        float horizontalRaySpacing;
        float verticalRaySpacing;

        new Collider2D collider;
        RaycastOrigins2D raycastOrigins;

        private CollisionInfo _collisions;
        public CollisionInfo Collisions {
            get { return _collisions; }
            private set { _collisions = value; }
        }

        void Start() {
            collider = GetComponent<BoxCollider2D>();
            CalculateRaySpacing();
        }

        public void Move(Vector3 velocity) {
            UpdateRaycastOrigins();
            Collisions.Reset();

            if (!Mathf.Approximately(velocity.x, 0)) {
                HorizontalCollisions(ref velocity);
            }
            if (!Mathf.Approximately(velocity.y, 0)) {
                VerticalCollisions(ref velocity);
            }

            transform.Translate(velocity);
        }

        void HorizontalCollisions(ref Vector3 velocity) {
            float directionX = Mathf.Sign(velocity.x);
            float rayLength = Mathf.Abs(velocity.x) + skinWidth;

            for (int i = 0; i < horizontalRayCount; i++) {
                Vector2 rayOrigin = (Mathf.Approximately(directionX, -1)) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight;
                rayOrigin += Vector2.up * (horizontalRaySpacing * i);
                RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, collisionMask);

                Debug.DrawRay(rayOrigin, Vector2.right * directionX * rayLength, Color.red);

                if (hit) {
                    velocity.x = (hit.distance - skinWidth) * directionX;
                    rayLength = hit.distance;

                    _collisions.left = Mathf.Approximately(directionX, -1);
                    _collisions.right = Mathf.Approximately(directionX, 1);
                }
            }
        }

        void VerticalCollisions(ref Vector3 velocity) {
            float directionY = Mathf.Sign(velocity.y);
            float rayLength = Mathf.Abs(velocity.y) + skinWidth;

            for (int i = 0; i < verticalRayCount; i++) {
                Vector2 rayOrigin = (Mathf.Approximately(directionY, -1)) ? raycastOrigins.bottomLeft : raycastOrigins.topLeft;
                rayOrigin += Vector2.right * (verticalRaySpacing * i + velocity.x);
                RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, collisionMask);

                Debug.DrawRay(rayOrigin, Vector2.up * directionY * rayLength, Color.red);

                if (hit) {
                    velocity.y = (hit.distance - skinWidth) * directionY;
                    rayLength = hit.distance;

                    _collisions.below = (Mathf.Approximately(directionY, -1));
                    _collisions.above = Mathf.Approximately(directionY, 1);
                }
            }
        }

        void UpdateRaycastOrigins() {
            Bounds bounds = collider.bounds;
            bounds.Expand(skinWidth * -2);

            raycastOrigins.bottomLeft = new Vector2(bounds.min.x, bounds.min.y);
            raycastOrigins.bottomRight = new Vector2(bounds.max.x, bounds.min.y);
            raycastOrigins.topLeft = new Vector2(bounds.min.x, bounds.max.y);
            raycastOrigins.topRight = new Vector2(bounds.max.x, bounds.max.y);
        }

        void CalculateRaySpacing() {
            Bounds bounds = collider.bounds;
            bounds.Expand(skinWidth * -2);

            horizontalRayCount = Mathf.Clamp(horizontalRayCount, 2, int.MaxValue);
            verticalRayCount = Mathf.Clamp(verticalRayCount, 2, int.MaxValue);

            horizontalRaySpacing = bounds.size.y / (horizontalRayCount - 1);
            verticalRaySpacing = bounds.size.x / (verticalRayCount - 1);
        }

        public struct RaycastOrigins2D {

            public Vector2 topRight;
            public Vector2 topLeft;
            public Vector2 bottomRight;
            public Vector2 bottomLeft;

        }

        public struct CollisionInfo {

            public bool above;
            public bool below;
            public bool left;
            public bool right;

            public void Reset() {
                above = false;
                below = false;
                left = false;
                right = false;
            }
        }

    }
}
