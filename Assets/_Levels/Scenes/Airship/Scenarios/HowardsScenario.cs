﻿using System.Collections;
using Assets.Core.Scenario;
using Randolph.Interactable;
using Randolph.UI;
using UnityEngine;

namespace Assets.Levels.Airship {
    public class HowardsScenario : ScenarioManager {
        [SerializeField] private Talkable howardsSpeechBubble;

        [Header("Responses")]
        [SerializeField, TextArea] private string firstResponse;
        [SerializeField, TextArea] private string secondResponse;
        [SerializeField, TextArea] private string thirdResponse;
        [SerializeField, TextArea] private string fourthResponse;
        [SerializeField, TextArea] private string fifthResponse;

        [Header("Items")]
        [SerializeField] private Transform storageKeySpawn;
        [SerializeField] private Key storageKeyPrefab;
        [SerializeField] private Bandage bandage;
        [SerializeField] private Alcohol alcohol;
        private CleanedBandage cleanedBandage;
        [SerializeField] private Cue highlightCue;
        [SerializeField] private Cue pickUpCue;
        [SerializeField] private Cue useCue;
        [SerializeField] private Door deckDoor;
        [SerializeField] private RandolphTalkTrigger deckExitTalkTrigger;

        protected override IEnumerable Scenario() {
            howardsSpeechBubble.OnStoppedSpeaking -= Iterate;
            howardsSpeechBubble.OnStoppedSpeaking += Iterate;
            yield return null;
            howardsSpeechBubble.OnStoppedSpeaking -= Iterate;

            howardsSpeechBubble.fullText = firstResponse;
            howardsSpeechBubble.Speak();

            var storageKey = Instantiate(storageKeyPrefab, storageKeySpawn);
            highlightCue.gameObject.SetActive(true);
            pickUpCue.gameObject.SetActive(true);
            
            storageKey.OnPick -= Iterate;
            storageKey.OnPick += Iterate;
            yield return null;
            storageKey.OnPick -= Iterate;
            
            highlightCue.Disable();
            pickUpCue.Disable();
            howardsSpeechBubble.fullText = secondResponse;
            useCue.gameObject.SetActive(true);
            
            storageKey.OnApply -= Iterate;
            storageKey.OnApply += Iterate;
            yield return null;
            storageKey.OnApply -= Iterate;

            useCue.Disable();
            
            bandage.OnPick -= Iterate;
            alcohol.OnPick -= Iterate;
            bandage.OnPick += Iterate;
            alcohol.OnPick += Iterate;
            yield return null;
            yield return null;
            bandage.OnPick -= Iterate;
            alcohol.OnPick -= Iterate;
            
            howardsSpeechBubble.fullText = thirdResponse;
            
            bandage.OnCombined -= BandageCleaned;
            alcohol.OnCombined -= BandageCleaned;
            bandage.OnCombined += BandageCleaned;
            alcohol.OnCombined += BandageCleaned;
            yield return null;
            bandage.OnCombined -= BandageCleaned;
            alcohol.OnCombined -= BandageCleaned;
            
            howardsSpeechBubble.fullText = fourthResponse;
            
            cleanedBandage.OnPick -= Iterate;
            cleanedBandage.OnApply += Iterate;
            yield return null;
            cleanedBandage.OnPick -= Iterate;

            howardsSpeechBubble.fullText = fifthResponse;
            deckDoor.isLocked = false;
            deckExitTalkTrigger.gameObject.SetActive(false);
        }

        private void BandageCleaned(InventoryItem bandage) {
            cleanedBandage = bandage as CleanedBandage;
            Iterate();
        }
    }

    #region IRestartable

    #endregion
}
