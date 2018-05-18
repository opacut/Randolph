using UnityEngine;
using UnityEngine.UI;
using Randolph.Core;

namespace Randolph.UI.Cues {
    [RequireComponent(typeof(Collider2D))]
    public class Cue : MonoBehaviour {
        [SerializeField] private Text textElement;
        [SerializeField] private string updateText;

        [Header("Cancellation")]
        [SerializeField] private MouseButton cancellationButton = MouseButton.None;
        [SerializeField] private string cancellationKeyName;
        [SerializeField] private bool cancelOnAreaExit;

        private bool wasActivated;

        private void OnTriggerEnter2D(Collider2D collision) {
            if (collision.tag == "Player") {
                textElement.text = updateText;
                wasActivated = true;
            }
        }

        private void OnTriggerExit2D(Collider2D other) {
            if (!wasActivated || !cancelOnAreaExit) {
                return;
            }
            textElement.text = "";
            Destroy(gameObject);
        }

        private void Update() {
            if (wasActivated
                && (cancellationKeyName != string.Empty && Input.GetAxis(cancellationKeyName) != 0
                    || cancellationButton != MouseButton.None && Input.GetMouseButtonDown((int)cancellationButton))) {
                textElement.text = "";
                Destroy(gameObject);
            }
        }
    }
}
