using System.Collections;
using Randolph.Interactable;
using Randolph.UI;
using UnityEngine;

namespace Assets.Core.Scenario {
    public class TutorialScenario : ScenarioManager {
        [SerializeField] private SpeechBubble howardsSpeechBubble;

        [SerializeField, TextArea] private string firstResponse;
        [SerializeField, TextArea] private string secondResponse;
        [SerializeField, TextArea] private string thirdResponse;

        [SerializeField] private Key storageKey;
        [SerializeField] private Bandage bandage;
        [SerializeField] private Alcohol alcohol;
        [SerializeField] private Cleanedbandage cleanedBandage;

        protected override IEnumerable Scenario() {
            howardsSpeechBubble.OnStoppedSpeaking += Iterate;
            yield return null;
            howardsSpeechBubble.OnStoppedSpeaking -= Iterate;

            howardsSpeechBubble.fullText = firstResponse;
            howardsSpeechBubble.Speak();
            storageKey.gameObject.SetActive(true);

            storageKey.OnPick += Iterate;
            yield return null;
            storageKey.OnPick -= Iterate;
            
            howardsSpeechBubble.fullText = secondResponse;

            bandage.OnPick += Iterate;
            alcohol.OnPick += Iterate;
            yield return null;
            yield return null;
            bandage.OnPick -= Iterate;
            alcohol.OnPick -= Iterate;
            
            howardsSpeechBubble.fullText = thirdResponse;
        }
    }
}
