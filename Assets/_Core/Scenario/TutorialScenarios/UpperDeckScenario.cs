using System;
using System.Collections;
using System.Collections.Generic;
using Randolph.Environment;
using Randolph.Interactable;
using Randolph.UI;
using UnityEngine;

namespace Assets.Core.Scenario {
    public class UpperDeckScenario : ScenarioManager {
        [SerializeField] private SpeechBubble captainsSpeechBubble;

        [Header("Responses")]
        [SerializeField, TextArea] private string firstResponse;

        [Header("Items")]
        [SerializeField] private Sabre sabre;
        [SerializeField] private ClimbableRope climbableRope;
        [SerializeField] private Hook hook;
        [SerializeField] private Cue climbCue;
        [SerializeField] private Cue hookCue;
        [SerializeField] private Sail frontSail;
        [SerializeField] private Sail backSail;

        protected override IEnumerable Scenario() {
            captainsSpeechBubble.OnStoppedSpeaking += Iterate;
            yield return null;
            captainsSpeechBubble.OnStoppedSpeaking -= Iterate;
            
            sabre.gameObject.SetActive(true);
            sabre.Pick();
            captainsSpeechBubble.fullText = firstResponse;

            frontSail.OnSlash += Iterate;
            yield return null;
            yield return null;
            frontSail.OnSlash -= Iterate;

            climbableRope.gameObject.SetActive(true);

            climbableRope.OnInteract += Iterate;
            yield return null;
            climbableRope.OnInteract -= Iterate;

            climbCue.gameObject.SetActive(true);

            hook.OnPick += Iterate;
            yield return null;
            hook.OnPick -= Iterate;
            
            hookCue.gameObject.SetActive(true);
            
        }
    }
}
