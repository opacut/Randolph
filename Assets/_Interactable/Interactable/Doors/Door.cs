using System;
using System.Threading.Tasks;
using Randolph.Core;
using UnityEngine;

namespace Randolph.Interactable {
	[RequireComponent(typeof(BoxCollider2D))]
	public class Door : Interactable {
		public Door linkedDoor;
		public int roomIndex;
        public bool isLocked;

        public override async void Interact() {
            base.Interact();
            if (!linkedDoor || isLocked) {
                // TODO: Play locked door sound
                return;
            }

            var randolph = GameObject.FindGameObjectWithTag(Constants.Tag.Player);
            
            Constants.Camera.transition.TransitionExit();
            await Task.Delay(TimeSpan.FromSeconds(Constants.Camera.transition.DurationExit));
            
            randolph.transform.position = linkedDoor.transform.position;
            Constants.Camera.rooms.EnterRoom(linkedDoor.roomIndex, false);

            Constants.Camera.transition.TransitionEnter();
            await Task.Delay(TimeSpan.FromSeconds(Constants.Camera.transition.DurationEnter));
        }

        private void OnDrawGizmosSelected() {
            if (!linkedDoor) {
                return;
            }
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, linkedDoor.transform.position);
        }
    }
}