using System;
using System.Collections;
using Assets.Core.Scenario;
using Randolph.Characters;
using UnityEngine;
using UnityEngine.UI;

namespace Randolph.UI {
    [RequireComponent(typeof(Talkable))] // TODO: Temporary (inherit from Talkable)
    public class SpeechBubble : MonoBehaviour {
        [SerializeField] Canvas speechBubble;
        CanvasScaler scaler;

        [SerializeField] Text bubbleText;
        [SerializeField] Color characterColor = Color.white;
        public string fullText;
        string currentText = "";
        bool isSpeaking;

        [SerializeField, ReadonlyField] float delay = 0.05f;

        void Start() {
            scaler = speechBubble.GetComponent<CanvasScaler>();
            scaler.enabled = false;
            speechBubble.enabled = false;
            fullText = bubbleText.text;
            bubbleText.text = "";
            bubbleText.color = characterColor;
        }

        void OnMouseDown() {
            if (!isSpeaking) {
                Speak();
            } else {
                if (currentText != fullText) {
                    // Instantly show full text on first click
                    currentText = fullText;
                    bubbleText.text = currentText;
                    StartCoroutine(Timer());
                } else {
                    // Stop conversation on second click
                    StopSpeaking();
                }
            }
        }

        public void Speak() {
            StopAllCoroutines();
            bubbleText.text = "";
            currentText = "";
            speechBubble.enabled = true;
            scaler.enabled = true;
            isSpeaking = true;
            StartCoroutine(ShowText());
        }

        void StopSpeaking() {
            StopAllCoroutines();
            speechBubble.enabled = false;
            scaler.enabled = false;
            isSpeaking = false;
            OnStoppedSpeaking?.Invoke();
        }

        IEnumerator Timer() {
            yield return new WaitForSeconds(5);
            StopSpeaking();
        }

        public IEnumerator ShowText() {
            while (currentText.Length < fullText.Length) {
                currentText = fullText.Substring(0, currentText.Length + 1);
                bubbleText.text = currentText;
                yield return new WaitForSeconds(delay);
            }
            StartCoroutine(Timer());
        }

        public event Action OnStoppedSpeaking;
    }
}
