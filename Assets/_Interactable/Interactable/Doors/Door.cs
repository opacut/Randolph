using System;
using System.Threading.Tasks;
using Randolph.Characters;
using Randolph.Core;
using UnityEngine;

namespace Randolph.Interactable {
    [RequireComponent(typeof(BoxCollider2D))]
    public class Door : Interactable {
        private AudioSource audioSource; // TODO: Make Interactables all have sound
        public bool isLocked;
        public Door linkedDoor;

        [SerializeField]
        private AudioClip lockedSound;

        public int roomIndex;

        protected override void Start() {
            base.Start();
            audioSource = AudioPlayer.audioPlayer.AddAudioSource(gameObject);
        }

        public override async void Interact() {
            base.Interact();
            if (!linkedDoor || isLocked) {
                //! Play locked door sound
                AudioPlayer.audioPlayer.PlayLocalSound(audioSource, lockedSound);
                return;
            }

            Constants.Randolph.Freeze();
            Constants.Camera.transition.TransitionExit();
            await Task.Delay(TimeSpan.FromSeconds(Constants.Camera.transition.DurationExit));
            
            Constants.Randolph.transform.position = linkedDoor.transform.position;
            Constants.Randolph.transform.AlignToGround();
            Constants.Camera.rooms.EnterRoom(linkedDoor.roomIndex, false);

            Constants.Camera.transition.TransitionEnter();
            await Task.Delay(TimeSpan.FromSeconds(Constants.Camera.transition.DurationEnter));
            Constants.Randolph.UnFreeze();
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
