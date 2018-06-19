using System;
using System.Collections;
using Randolph.Characters;
using Randolph.Levels;
using UnityEngine;
using UnityEngine.UI;

namespace Randolph.UI {
    [RequireComponent(typeof(Talkable))] // TODO: Temporary (inherit from Talkable)
    public class SpeechBubble : MonoBehaviour, IRestartable {
        [SerializeField] private Text bubbleText;
        [SerializeField] private Color characterColor = Color.white;

        private string currentText;

        [SerializeField, ReadonlyField]
        private float delay = 0.05f;

        [SerializeField, TextArea] public string fullText;
        private bool isSpeaking;
        private CanvasScaler scaler;

        [SerializeField] private Canvas speechBubble;

        private void Awake() {
            scaler = speechBubble.GetComponent<CanvasScaler>();
        }

        private void Start() {
            scaler.enabled = false;
            speechBubble.enabled = false;

            bubbleText.text = string.Empty;
            bubbleText.color = characterColor;
            SaveState();
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

        public void Speak() {
            StopAllCoroutines();
            bubbleText.text = string.Empty;
            currentText = string.Empty;
            speechBubble.enabled = true;
            scaler.enabled = true;
            isSpeaking = true;
            if (GetComponent<Animator>()) {
                GetComponent<Animator>().SetBool("Speaking", true);
            }

            OnStartedSpeaking?.Invoke();
            StartCoroutine(ShowText());
        }

        private void StopSpeaking() {
            StopAllCoroutines();
            speechBubble.enabled = false;
            scaler.enabled = false;
            isSpeaking = false;
            if (GetComponent<Animator>()) {
                GetComponent<Animator>().SetBool("Speaking", false);
            }

            OnStoppedSpeaking?.Invoke();
        }

        private IEnumerator Timer() {
            yield return new WaitForSeconds(5);
            StopSpeaking();
        }

        private IEnumerator ShowText() {
            while (currentText.Length < fullText.Length) {
                currentText = fullText.Substring(0, currentText.Length + 1);
                bubbleText.text = currentText;
                yield return new WaitForSeconds(delay);
            }
            StartCoroutine(Timer());
        }

        public event Action OnStartedSpeaking;
        public event Action OnStoppedSpeaking;

        #region Implementation of IRestartable
        private string savedText;

        public void SaveState() {
            savedText = fullText;
        }

        public void Restart() {
            StopAllCoroutines();
            fullText = savedText;
            scaler.enabled = false;
            speechBubble.enabled = false;
            bubbleText.text = string.Empty;
            bubbleText.color = characterColor;
            isSpeaking = false;
            if (GetComponent<Animator>()) {
                GetComponent<Animator>().SetBool("Speaking", false);
            }
        }
        #endregion
    }
}
