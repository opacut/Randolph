using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Randolph.UI.Cues {
    public class KeyCue : Cue {
        public string cancellationKeyName;
        private void Update() {
            if (wasActivated && Input.GetAxis(cancellationKeyName) != 0) {
                tutorialText.text = "";
                Destroy(gameObject);
            }
        }
    }
}