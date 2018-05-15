using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Randolph.UI.Cues {
    public class AreaCue : Cue {
        private void OnTriggerExit2D(Collider2D other) {
            if (!wasActivated) {
                return;
            }
            tutorialText.text = "";
            Destroy(gameObject);
        }
    }
}