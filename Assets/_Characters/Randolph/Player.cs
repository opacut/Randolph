using UnityEngine;

namespace Randolph.Characters {
    [RequireComponent(typeof(RaycastController2D))]
    public class Player : MonoBehaviour {

        public float jumpHeight = 4;
        public float timeToJumpApex = .4f;
        float accelerationTimeAirborne = .2f;
        float accelerationTimeGrounded = .1f;
        float moveSpeed = 6;

        float gravity;
        float jumpVelocity;
        Vector3 velocity;
        float velocityXSmoothing;

        RaycastController2D controller;

        void Start() {
            controller = GetComponent<RaycastController2D>();

            gravity = -(2 * jumpHeight) / Mathf.Pow(timeToJumpApex, 2);
            jumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
            print("Gravity: " + gravity + "  Jump Velocity: " + jumpVelocity);
        }

        void Update() {
            if (controller.Collisions.above || controller.Collisions.below) {
                velocity.y = 0;
            }

            Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

            if (Input.GetKeyDown(KeyCode.Space) && controller.Collisions.below) {
                velocity.y = jumpVelocity;
            }

            float targetVelocityX = input.x * moveSpeed;
            velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.Collisions.below) ? accelerationTimeGrounded : accelerationTimeAirborne);
            velocity.y += gravity * Time.deltaTime;
            controller.Move(velocity * Time.deltaTime);
        }

    }
}
