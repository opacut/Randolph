using System.Collections;
using Assets.Core.Scenario;
using Randolph.Interactable;
using Randolph.UI;
using UnityEngine;

namespace Assets.Levels.Mountains {
    public class CaptainMountainsScenario : ScenarioManager {
        [SerializeField] private SpeechBubble captainsSpeechBubble;

        [Header("Responses")]
        [SerializeField, TextArea]
        private string firstResponse;

        [SerializeField] private Sabre knife;

        [Header("Items")]
        [SerializeField]
        private Lighter lighter;

        [SerializeField] private Whiskey whiskey;

        protected override IEnumerable Scenario() {
            captainsSpeechBubble.OnStoppedSpeaking -= Iterate;
            captainsSpeechBubble.OnStoppedSpeaking += Iterate;
            yield return null;
            captainsSpeechBubble.OnStoppedSpeaking -= Iterate;

            captainsSpeechBubble.fullText = firstResponse;
            captainsSpeechBubble.Speak();
            lighter.gameObject.SetActive(true);
            knife.gameObject.SetActive(true);
            whiskey.gameObject.SetActive(true);
            lighter.Pick();
            knife.Pick();
            whiskey.Pick();
        }
    }
}
