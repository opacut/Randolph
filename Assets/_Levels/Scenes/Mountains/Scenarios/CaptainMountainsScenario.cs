using Assets.Core.Scenario;
using Randolph.Interactable;
using Randolph.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Levels.Mountains
{
    public class CaptainMountainsScenario : ScenarioManager
    {
        [SerializeField] private SpeechBubble captainsSpeechBubble;

        [Header("Responses")]
        [SerializeField, TextArea] private string firstResponse;

        [Header("Items")]
        [SerializeField] private Lighter lighter;
        [SerializeField] private Sabre knife;
        [SerializeField] private Whiskey whiskey;

        protected override IEnumerable Scenario()
        {
            captainsSpeechBubble.OnStoppedSpeaking += Iterate;
            yield return null;
            captainsSpeechBubble.OnStoppedSpeaking -= Iterate;

            captainsSpeechBubble.fullText = firstResponse;
            captainsSpeechBubble.Speak();
            lighter.gameObject.SetActive(true);
            lighter.Pick();
            knife.gameObject.SetActive(true);
            knife.Pick();
            whiskey.gameObject.SetActive(true);
            whiskey.Pick();
        }
    }
}