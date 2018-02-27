using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hazard : MonoBehaviour, IRestartable {
    public Vector3 initialPosition;

    public void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Player") {
            other.GetComponent<PlayerController>().Kill();
        }
    }


    void Awake() {
        initialPosition = gameObject.transform.position;
    }

    public void Restart() {
        gameObject.transform.position = initialPosition;
    }
}
