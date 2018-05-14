using System.Collections;
using System.Collections.Generic;
using cakeslice;
using Randolph.Core;
using UnityEngine;

namespace Randolph.Environment {
	[RequireComponent(typeof(BoxCollider2D))]
	[RequireComponent(typeof(SpriteRenderer))]
	[RequireComponent(typeof(Outline))]
	public class Door : MonoBehaviour {
		public BoxCollider2D trigger { get; private set; }
		public Outline outline { get; private set; }

		public Door linkedDoor;

		private void Awake() {
			trigger = GetComponent<BoxCollider2D>();
			outline = GetComponent<Outline>();
		}

		private void Start() {
			outline.enabled = false;
		}

		private void OnMouseEnter() {
			outline.enabled = true;
		}

		private void OnMouseExit() {
			outline.enabled = false;
		}

		private void OnMouseOver() {
			if (Input.GetMouseButtonDown(0) && linkedDoor) {
				var randolph = GameObject.FindGameObjectWithTag(Constants.Tag.Player);
				randolph.transform.position = linkedDoor.transform.position;
			}
		}
	}
}