using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Randolph.UI.Cues {
    public class MouseCue : Cue {
        public int cancellationButton;
        private void Update() {
            if (wasActivated && Input.GetMouseButtonDown(cancellationButton)) {
                tutorialText.text = "";
                Destroy(gameObject);
            }
        }
    }
}