using System.Collections;
using Assets.Core.Scenario;
using Randolph.Interactable;
using Randolph.UI;
using UnityEngine;

namespace Assets.Levels.Airship {
    public class InteractionScenario : ScenarioManager {
        [SerializeField] private Cue interactCue;
        [SerializeField] private Cue speakCue;

        [SerializeField] private Door quartersDoor;
        [SerializeField] private SpeechBubble howardsSpeechBubble;

        protected override IEnumerable Scenario() {
            quartersDoor.OnInteract -= Iterate;
            quartersDoor.OnInteract += Iterate;
            yield return null;
            quartersDoor.OnInteract -= Iterate;

            interactCue.Disable();
            
            howardsSpeechBubble.OnStartedSpeaking -= Iterate;
            howardsSpeechBubble.OnStartedSpeaking += Iterate;
            yield return null;
            howardsSpeechBubble.OnStartedSpeaking -= Iterate;

            speakCue.Disable();
        }
    }
}
