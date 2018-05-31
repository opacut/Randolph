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
    }
}
