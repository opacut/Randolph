using System.Collections;
using Randolph.Interactable;
using Randolph.UI;
using UnityEngine;

namespace Assets.Core.Scenario {
    public class HowardsScenario : ScenarioManager {
        [SerializeField] private SpeechBubble howardsSpeechBubble;

        [Header("Responses")]
        [SerializeField, TextArea] private string firstResponse;
        [SerializeField, TextArea] private string secondResponse;
        [SerializeField, TextArea] private string thirdResponse;
        [SerializeField, TextArea] private string fourthResponse;
        [SerializeField, TextArea] private string fifthResponse;

        [Header("Items")]
        [SerializeField] private Key storageKey;
        [SerializeField] private Bandage bandage;
        [SerializeField] private Alcohol alcohol;
        [SerializeField] private Cleanedbandage cleanedBandage;

        [Header("Deck exit")]
        [SerializeField] private Door deckDoor;
        [SerializeField] private RandolphTalkTrigger deckExitTalkTrigger;

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

            cleanedBandage.OnPick += Iterate;
            yield return null;
            cleanedBandage.OnPick -= Iterate;
            
            howardsSpeechBubble.fullText = fourthResponse;

            cleanedBandage.OnApply += Iterate;
            yield return null;
            cleanedBandage.OnPick -= Iterate;

            howardsSpeechBubble.fullText = fifthResponse;
            deckDoor.isLocked = false;
            Destroy(deckExitTalkTrigger);
        }
    }
}
