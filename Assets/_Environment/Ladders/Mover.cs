using UnityEngine;

public class Mover : MonoBehaviour {
    public float speed;

    void Update() {
        GetComponent<Rigidbody2D>().velocity = new Vector2(speed, GetComponent<Rigidbody2D>().velocity.y);
    }
	
    void OnTriggerEnter2D(Collider2D other) {
        if (other.tag != "Ladder") {
            Destroy(gameObject);
        }
    }
}
