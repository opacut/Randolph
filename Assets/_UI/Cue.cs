using UnityEngine;
using UnityEngine.UI;
using Randolph.Core;
using Randolph.Levels;

namespace Randolph.UI {
    [RequireComponent(typeof(Collider2D))]
    public class Cue : RestartableBase {
        [SerializeField] private Image textBox;
        [SerializeField] private Text textElement;
        [SerializeField, TextArea] private string updateText;

        [Header("Cancellation")]
        [SerializeField] private bool isManual;
        [SerializeField] private MouseButton cancellationButton = MouseButton.None;
        [SerializeField] private string cancellationKeyName;
        [SerializeField] private bool cancelOnAreaExit;

        private bool wasActivated;

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
        public override void Restart() {
            base.Restart();

            Disable();
            wasActivated = false;
        }
        #endregion
    }
}
