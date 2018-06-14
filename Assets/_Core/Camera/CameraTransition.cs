using System;
using UnityEngine;

namespace Randolph.Core {
    [RequireComponent(typeof(BoxCollider2D))]
    public class CameraTransition : MonoBehaviour {
        [SerializeField] private TransitionDirection _direction;
        [SerializeField] private int _positiveRoomId;
        [SerializeField] private int _negativeRoomId;

        private void OnTriggerStay2D(Collider2D other) {
            if (other.tag != Constants.Tag.Player) {
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
                Constants.Camera.rooms.EnterRoom(_positiveRoomId);
            } else if (transitionDirection < -0.1) {
                Constants.Camera.rooms.EnterRoom(_negativeRoomId);
            }
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
            var positiveRoom = Constants.Camera.rooms.GetRoom(_positiveRoomId.ToString())?.Dimensions;
            var negativeRoom = Constants.Camera.rooms.GetRoom(_negativeRoomId.ToString())?.Dimensions;

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
