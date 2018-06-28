using System.Collections;
using Assets.Core.Scenario;
using Randolph.Environment;
using Randolph.Interactable;
using Randolph.Levels.Airship;
using Randolph.UI;
using UnityEngine;

namespace Assets.Levels.Airship {
    public class UpperDeckScenario : ScenarioManager {
        [SerializeField] private Talkable captainsSpeechBubble;

        [Header("Responses")]
        [SerializeField, TextArea] private string firstResponse;

        [Header("Items")]
        [SerializeField] private Sabre sabrePrefab;
        [SerializeField] private ClimbableRope climbableRope;
        [SerializeField] private Hook hook;
        [SerializeField] private Cue climbCue;
        [SerializeField] private Cue hookCue;
        [SerializeField] private Sail frontSail;
        [SerializeField] private Sail backSail;
        [SerializeField] private TutorialEnd tutorialEndTrigger;

        protected override IEnumerable Scenario() {
            captainsSpeechBubble.OnStoppedSpeaking -= Iterate;
            captainsSpeechBubble.OnStoppedSpeaking += Iterate;
            yield return null;
            captainsSpeechBubble.OnStoppedSpeaking -= Iterate;

            var sabre = Instantiate(sabrePrefab);
            sabre.Pick();
            captainsSpeechBubble.fullText = firstResponse;
            
            frontSail.OnSlash -= Iterate;
            frontSail.OnSlash += Iterate;
            yield return null;
            yield return null;
            frontSail.OnSlash -= Iterate;

            climbableRope.gameObject.SetActive(true);
            
            climbableRope.OnInteract -= Iterate;
            climbableRope.OnInteract += Iterate;
            yield return null;
            climbableRope.OnInteract -= Iterate;

            climbCue.gameObject.SetActive(true);
            
            hook.OnPick -= Iterate;
            hook.OnPick += Iterate;
            yield return null;
            hook.OnPick -= Iterate;
            
            hookCue.gameObject.SetActive(true);
            tutorialEndTrigger.gameObject.SetActive(true);
        }
    }
}
