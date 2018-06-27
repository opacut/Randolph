using System;
using System.Collections;
using Randolph.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Randolph.Interactable {
    public class Talkable: Clickable {
        [Header("Speech Bubble")]
        [SerializeField] private Text bubbleText;
        [SerializeField] private Canvas bubbleCanvas;
        [SerializeField] private Color characterColor = Color.white;
        [SerializeField, TextArea] public string fullText;

        private string currentText;
        private float delay = 0.05f;
        private CanvasScaler scaler;

        private bool isSpeaking;

        public override Cursors CursorType { get; protected set; } = Cursors.Talk;

        protected override void Awake() {
            base.Awake();
            scaler = bubbleCanvas.GetComponent<CanvasScaler>();
        }

        protected override void Start() {
            base.Start();

            outline.color = 1;

            scaler.enabled = false;
            bubbleCanvas.enabled = false;

            bubbleText.text = string.Empty;
            bubbleText.color = characterColor;
            SaveState();
        }

        public virtual void OnTalk() {
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
            bubbleCanvas.enabled = true;
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
            bubbleCanvas.enabled = false;
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

        public override void SaveState() {
            base.SaveState();
            savedText = fullText;
        }

        public override void Restart() {
            base.Restart();

            StopAllCoroutines();
            fullText = savedText;
            scaler.enabled = false;
            bubbleCanvas.enabled = false;
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
