using System;
using System.Threading.Tasks;
using cakeslice;
using Randolph.Core;
using UnityEngine;

namespace Randolph.Interactable {
	[RequireComponent(typeof(BoxCollider2D))]
	public class Door : Interactable {
		public Door linkedDoor;
		public int roomIndex;

        public override async void OnInteract() {
            if (!linkedDoor) {
                return;
            }

            var randolph = GameObject.FindGameObjectWithTag(Constants.Tag.Player);

            // Constants.Camera.rooms.AutomaticRoomActivation = false;
            Constants.Camera.transition.TransitionExit();
            await Task.Delay(TimeSpan.FromSeconds(Constants.Camera.transition.DurationExit));

            var deltaY = randolph.transform.position.y - transform.position.y;
            randolph.transform.position = linkedDoor.transform.position;
            randolph.transform.Translate(0, deltaY, 0);
            Constants.Camera.rooms.EnterRoom(linkedDoor.roomIndex, false);

            Constants.Camera.transition.TransitionEnter();
            await Task.Delay(TimeSpan.FromSeconds(Constants.Camera.transition.DurationEnter));
            // Constants.Camera.rooms.AutomaticRoomActivation = true;
        }
	}
}