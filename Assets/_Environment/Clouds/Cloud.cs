using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Rigidbody2D))]
public class Cloud : MonoBehaviour {
	private SpriteRenderer renderer;
	private Rigidbody2D rbody;
	public Sprite cloud {
		get { return renderer.sprite; }
		set { renderer.sprite = value; }
	}
	public float endX = 0;
	public float speed {
		set {
			rbody.velocity = new Vector2(-value, 0);
		}
	}

	// Use this for initialization
	void Start() {
		renderer = GetComponent<SpriteRenderer>();
		rbody = GetComponent<Rigidbody2D>();
	}

	// Update is called once per frame
	void Update() {
		if (transform.position.x < endX) {
			Destroy(gameObject);
		}
	}
}