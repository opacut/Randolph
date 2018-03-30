using UnityEngine;

namespace Randolph.Characters {
    public class Projectile : MonoBehaviour {

        [SerializeField] float speed = 8.5f;

        Rigidbody2D rbody;

        void Awake() {
            rbody = GetComponent<Rigidbody2D>();
        }

        void Update() {
            rbody.velocity = new Vector2(speed, GetComponent<Rigidbody2D>().velocity.y);
        }

        void OnTriggerEnter2D(Collider2D other) {
            if (other.tag == "Player") {
                other.gameObject.GetComponent<PlayerController>().Kill();
            }

            if (other.tag != "Ladder") {
                Destroy(gameObject);
            }
        }

    }
}