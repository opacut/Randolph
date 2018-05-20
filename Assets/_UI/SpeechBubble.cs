using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Randolph.UI {
    public class SpeechBubble : MonoBehaviour {
        [SerializeField] private readonly float delay = 0.05f;
        [SerializeField] private Text bubbleText;

        private string currentText = "";

        [SerializeField] private string fullText;
        private bool isSpeaking;
        [SerializeField] private Canvas speechBubble;

        private void Start() {
            speechBubble.enabled = false;
            bubbleText.text = "";
        }

        private void OnMouseDown() {
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

        private void Speak() {
            StopAllCoroutines();
            bubbleText.text = "";
            currentText = "";
            speechBubble.enabled = true;
            isSpeaking = true;
            StartCoroutine(ShowText());
        }

        private void StopSpeaking() {
            StopAllCoroutines();
            speechBubble.enabled = false;
            isSpeaking = false;
        }

        private IEnumerator Timer() {
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
