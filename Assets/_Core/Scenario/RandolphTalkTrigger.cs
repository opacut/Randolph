using System.Collections;
using System.Collections.Generic;
using Randolph.Characters;
using Randolph.Core;
using UnityEngine;

namespace Assets.Core.Scenario {
    [RequireComponent(typeof(Collider2D))]
    public class RandolphTalkTrigger : MonoBehaviour {
        [SerializeField, TextArea] private string _displayText;
        [SerializeField] private float _duration;
        [SerializeField] private bool _hideOnExit;

        private void OnTriggerEnter2D(Collider2D other) {
            if (other.CompareTag(Constants.Tag.Player)) {
                other.GetComponent<PlayerController>().ShowDescriptionBubble(_displayText, _duration);
            }
        }

        private void OnTriggerExit2D(Collider2D other) {
            if (_hideOnExit && other.CompareTag(Constants.Tag.Player)) {
                other.GetComponent<PlayerController>().HideDescriptionBubble(_displayText);
            }
        }
    }
}