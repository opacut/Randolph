using UnityEngine;
using UnityEngine.UI;
using Randolph.Core;
using Randolph.Levels;

namespace Randolph.UI {
    [RequireComponent(typeof(Collider2D))]
    public class Cue : MonoBehaviour, IRestartable {
        [SerializeField] private Image textBox;
        [SerializeField] private Text textElement;
        [SerializeField, TextArea] private string updateText;

        [Header("Cancellation")]
        [SerializeField] private bool isManual;
        [SerializeField] private MouseButton cancellationButton = MouseButton.None;
        [SerializeField] private string cancellationKeyName;
        [SerializeField] private bool cancelOnAreaExit;

        private bool wasActivated;

        private void Start() => SaveState();

        private void OnTriggerEnter2D(Collider2D collision) {
            if (collision.tag == "Player") {
                textBox.enabled = true;
                textElement.text = updateText;
                wasActivated = true;
            }
        }

        private void OnTriggerExit2D(Collider2D other) {
            if (!wasActivated || isManual || !cancelOnAreaExit) {
                return;
            }
            Disable();
        }

        private void Update() {
            if (wasActivated && !isManual
                && (cancellationKeyName != string.Empty && Input.GetAxis(cancellationKeyName) != 0
                    || cancellationButton != MouseButton.None && Input.GetMouseButtonDown((int)cancellationButton))) {
                Disable();
            }
        }

        public void Disable() {
            textBox.enabled = false;
            textElement.text = "";
            gameObject.SetActive(false);
        }

        #region IRestartable
        protected bool savedActiveState { get; private set; }

        public void SaveState() {
            savedActiveState = gameObject.activeSelf;
        }

        public void Restart() {
            Disable();
            wasActivated = false;
            gameObject.SetActive(savedActiveState);
        }
        #endregion
    }
}
