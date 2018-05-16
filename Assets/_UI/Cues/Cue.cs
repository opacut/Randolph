using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Randolph.UI.Cues {
    [RequireComponent(typeof(Collider2D))]
    public class Cue : MonoBehaviour {
        [SerializeField]
        protected Text tutorialText;

        [SerializeField]
        protected string updateText;

        protected bool wasActivated;

        private void OnTriggerEnter2D(Collider2D collision) {
            if (collision.tag == "Player") {
                tutorialText.text = updateText;
                wasActivated = true;
            }
        }
    }
}