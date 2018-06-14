using System.Collections;
using Randolph.Interactable;
using Randolph.UI;
using UnityEngine;

namespace Assets.Core.Scenario {
    public class InteractionScenario : ScenarioManager {
        [SerializeField] private Cue interactCue;
        [SerializeField] private Cue speakCue;

        [SerializeField] private Door quartersDoor;
        [SerializeField] private SpeechBubble howardsSpeechBubble;

        protected override IEnumerable Scenario() {
            quartersDoor.OnInteract += Iterate;
            yield return null;
            quartersDoor.OnInteract -= Iterate;

            interactCue.Disable();

            howardsSpeechBubble.OnStartedSpeaking += Iterate;
            yield return null;
            howardsSpeechBubble.OnStartedSpeaking -= Iterate;

            speakCue.Disable();
        }
    }
}
