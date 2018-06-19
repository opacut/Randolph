using System;
using System.Threading.Tasks;
using Randolph.Core;
using UnityEngine;
using Randolph.Characters;

namespace Randolph.Interactable {
    [RequireComponent(typeof(BoxCollider2D))]
    public class Door : Interactable {
        public bool isLocked;
        public Door linkedDoor;
        public int roomIndex;

        public override async void Interact() {
            base.Interact();
            if (!linkedDoor || isLocked) {
                // TODO: Play locked door sound
                return;
            }

            var randolph = GameObject.FindGameObjectWithTag(Constants.Tag.Player);

            randolph.GetComponent<PlayerController>().Freeze();
            Constants.Camera.transition.TransitionExit();
            randolph.transform.position = linkedDoor.transform.position;
            await Task.Delay(TimeSpan.FromSeconds(Constants.Camera.transition.DurationExit));
            
            Constants.Camera.rooms.EnterRoom(linkedDoor.roomIndex, false);

            Constants.Camera.transition.TransitionEnter();
            await Task.Delay(TimeSpan.FromSeconds(Constants.Camera.transition.DurationEnter));
            randolph.GetComponent<PlayerController>().UnFreeze();
        }

        private void OnDrawGizmosSelected() {
            if (!linkedDoor) {
                return;
            }
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, linkedDoor.transform.position);
        }

        #region IRestartable
        private bool initialLocked;

        public override void SaveState() {
            base.SaveState();
            initialLocked = isLocked;
        }

        public override void Restart() {
            base.Restart();
            isLocked = initialLocked;
        }
        #endregion
    }
}
