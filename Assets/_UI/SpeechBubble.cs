﻿using System.Collections;
using Randolph.Characters;
using UnityEngine;
using UnityEngine.UI;

namespace Randolph.UI {
    [RequireComponent(typeof(Talkable))] // TODO: Temporary (inherit from Talkable)
    public class SpeechBubble : MonoBehaviour {
        [SerializeField] readonly float delay = 0.05f;
        [SerializeField] Text bubbleText;

        string currentText = "";

        [SerializeField] string fullText;
        bool isSpeaking;
        [SerializeField] Canvas speechBubble;
        CanvasScaler scaler;

        void Start() {
            scaler = speechBubble.GetComponent<CanvasScaler>();
            scaler.enabled = false;
            speechBubble.enabled = false;
            bubbleText.text = "";
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

        void Speak() {
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
    }
}
