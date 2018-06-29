using System;
using UnityEngine;

namespace Randolph.Core {
    [RequireComponent(typeof(BoxCollider2D))]
    public class CameraTransition : MonoBehaviour {
        [SerializeField] private TransitionDirection _direction;
        [SerializeField] private int positiveRoomId;
        [SerializeField] private int negativeRoomId;

        private void OnTriggerStay2D(Collider2D other) {
            if (!other.CompareTag(Constants.Tag.Player)) {
                return;
            }

            float transitionDirection;
            switch (_direction) {
            case TransitionDirection.Horizontal:
                transitionDirection = other.attachedRigidbody.velocity.x;
                break;
            case TransitionDirection.Vertical:
                transitionDirection = other.attachedRigidbody.velocity.y;
                break;
            default:
                throw new ArgumentOutOfRangeException();
            }
            
            if (transitionDirection > 0.1) {
                Constants.Camera.rooms.EnterRoom(positiveRoomId);
            } else if (transitionDirection < -0.1) {
                Constants.Camera.rooms.EnterRoom(negativeRoomId);
            }
        }

        private void OnTriggerExit2D(Collider2D other) {
            if (!other.CompareTag(Constants.Tag.Player)) {
                return;
            }

            bool enteredPositiveRoom;
            switch (_direction) {
            case TransitionDirection.Horizontal:
                enteredPositiveRoom = other.transform.position.x > transform.position.x;
                break;
            case TransitionDirection.Vertical:
                enteredPositiveRoom = other.transform.position.y > transform.position.y;
                break;
            default:
                throw new ArgumentOutOfRangeException();
            }

            Constants.Camera.rooms.EnterRoom(enteredPositiveRoom ? positiveRoomId : negativeRoomId);
        }

        private enum TransitionDirection {
            Horizontal,
            Vertical
        }

        private void OnDrawGizmos() {
            var collider = GetComponent<BoxCollider2D>();
            Gizmos.color = new Color(1f, 0.65f, 0f);
            Gizmos.DrawWireCube(transform.position, collider.size);
            Gizmos.color = new Color(1f, 0.65f, 0f, 0.25f);
            Gizmos.DrawCube(transform.position, collider.size);
        }

        private void OnDrawGizmosSelected() {
            var positiveRoom = Constants.Camera.rooms.GetRoom(positiveRoomId.ToString())?.Dimensions;
            var negativeRoom = Constants.Camera.rooms.GetRoom(negativeRoomId.ToString())?.Dimensions;

            if (positiveRoom.HasValue) {
                Gizmos.color = new Color(0f, 1f, 0f, 0.2f);
                Gizmos.DrawCube(positiveRoom.Value.position, positiveRoom.Value.size);
            }
            if (negativeRoom.HasValue) {
                Gizmos.color = new Color(1f, 0f, 0f, 0.2f);
                Gizmos.DrawCube(negativeRoom.Value.position, negativeRoom.Value.size);
            }
        }
    }
}
