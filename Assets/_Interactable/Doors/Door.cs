using System;
using System.Threading.Tasks;
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
		public int roomIndex;

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

		private async void OnMouseOver() {
			if (Input.GetMouseButtonDown(0) && linkedDoor) {
				var randolph = GameObject.FindGameObjectWithTag(Constants.Tag.Player);

				Constants.Camera.rooms.AutomaticRoomActivation = false;
				Constants.Camera.transition.TransitionExit();
				await Task.Delay(TimeSpan.FromSeconds(Constants.Camera.transition.DurationExit));

				var deltaY = randolph.transform.position.y - transform.position.y;
				randolph.transform.position = linkedDoor.transform.position;
				randolph.transform.Translate(0, deltaY, 0);
				Constants.Camera.rooms.EnterRoom(linkedDoor.roomIndex, false);

				Constants.Camera.transition.TransitionEnter();
				await Task.Delay(TimeSpan.FromSeconds(Constants.Camera.transition.DurationEnter));
				Constants.Camera.rooms.AutomaticRoomActivation = true;
			}
		}
	}
}