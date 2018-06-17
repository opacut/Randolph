using Randolph.Core;
using Randolph.Levels;
using UnityEngine;

namespace Randolph.Characters {
    public class Projectile : MonoBehaviour, IRestartable {

        // TODO: Object pool
        
        [SerializeField] float speed = 8.5f;

        SpriteRenderer spriteRenderer;
        Rigidbody2D rbody;
        bool toBeDestroyed;


        private void Awake() {
            spriteRenderer = GetComponent<SpriteRenderer>();
            rbody = GetComponent<Rigidbody2D>();
        }

        public void Restart() {
            if (!toBeDestroyed) spriteRenderer.enabled = false;
            else print("X");
        }

        private void Update() {
            rbody.velocity = new Vector2(speed, GetComponent<Rigidbody2D>().velocity.y);
        }

        private void OnTriggerEnter2D(Collider2D other) {
            if (other.tag == Constants.Tag.Player) {
                other.gameObject.GetComponent<PlayerController>().Kill();
            }
            if (other.tag != Constants.Tag.Ladder) {                
                Destroy(gameObject);
            }
        }

        void OnDestroy() {
            // Prevent destroying the projectile twice
            toBeDestroyed = true;
        }

    }
}