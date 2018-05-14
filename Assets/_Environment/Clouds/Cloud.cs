using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Randolph.Environment {
	[RequireComponent(typeof(SpriteRenderer))]
	[RequireComponent(typeof(Rigidbody2D))]
	public class Cloud : MonoBehaviour {
		public SpriteRenderer spriteRenderer { get; private set; }
		public Rigidbody2D rbody { get; private set; }

		public Sprite sprite {
			get { return spriteRenderer.sprite; }
			set { spriteRenderer.sprite = value; }
		}
		public Color color { set { spriteRenderer.color = value; } }
		public float speed { set { rbody.velocity = new Vector2(-value, 0); } }

		public float endX = 0;

		private void Awake() {
			spriteRenderer = GetComponent<SpriteRenderer>();
			rbody = GetComponent<Rigidbody2D>();
		}

		private void Update() {
			if (transform.position.x < endX) {
				Destroy(gameObject);
			}
		}
	}
}