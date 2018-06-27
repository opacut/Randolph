using System.Collections;
using Assets.Core.Scenario;
using Randolph.Interactable;
using Randolph.UI;
using UnityEngine;

namespace Assets.Levels.Mountains {
    public class CaptainMountainsScenario : ScenarioManager {
        [SerializeField] private Talkable captainsSpeechBubble;

        [Header("Responses")]
        [SerializeField, TextArea]
        private string firstResponse;

        [Header("Items")]
        [SerializeField] private Sabre knifePrefab;
        [SerializeField] private Lighter lighterPrefab;
        [SerializeField] private Whiskey whiskeyPrefab;

        protected override IEnumerable Scenario() {
            captainsSpeechBubble.OnStoppedSpeaking -= Iterate;
            captainsSpeechBubble.OnStoppedSpeaking += Iterate;
            yield return null;
            captainsSpeechBubble.OnStoppedSpeaking -= Iterate;

            captainsSpeechBubble.fullText = firstResponse;
            captainsSpeechBubble.Speak();

            var knife = Instantiate(knifePrefab);
            knife.Pick();
            var lighter = Instantiate(lighterPrefab);
            lighter.Pick();
            var whiskey = Instantiate(whiskeyPrefab);
            whiskey.Pick();
        }
    }
}
