using System.Collections;
using System.Collections.Generic;
using Randolph.Characters;
using Randolph.Core;
using Randolph.Levels;
using UnityEngine;

namespace Assets.Core.Scenario {
    [RequireComponent(typeof(Collider2D))]
    public class RandolphTalkTrigger : RestartableBase {
        [SerializeField, TextArea] private string displayText;
        [SerializeField] private float duration;
        [SerializeField] private bool hideOnExit;

        private void OnTriggerEnter2D(Collider2D other) {
            if (other.CompareTag(Constants.Tag.Player)) {
                other.GetComponent<PlayerController>().ShowDescriptionBubble(displayText, duration);
            }
        }

        private void OnTriggerExit2D(Collider2D other) {
            if (hideOnExit && other.CompareTag(Constants.Tag.Player)) {
                other.GetComponent<PlayerController>().HideDescriptionBubble(displayText);
            }
        }
    }
}