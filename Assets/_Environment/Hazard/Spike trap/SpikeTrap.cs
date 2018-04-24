using Randolph.Characters;
using Randolph.Core;
using Randolph.Levels;
using UnityEngine;

namespace Randolph.Environment {
    [RequireComponent(typeof(SpriteRenderer))]
    public class SpikeTrap : MonoBehaviour, IRestartable {

        [SerializeField] bool Up;
        [SerializeField] Sprite activeSprite;
        [SerializeField] Sprite inactiveSprite;

        SpriteRenderer spriteRenderer;
        bool initialUp;

        void Awake() {
            initialUp = Up;
            spriteRenderer = GetComponent<SpriteRenderer>();
            spriteRenderer.sprite = Up ? activeSprite : inactiveSprite;
        }

        public void Restart() {
            if (Up != initialUp) Toggle();
        }

        public void Toggle() {
            Toggle(!Up);
        }

        public void Toggle(bool active) {
            Up = active;
            spriteRenderer.sprite = active ? activeSprite : inactiveSprite;
        }

        public void OnTriggerStay2D(Collider2D other) {
            if (Up && (other.tag == Constants.Tag.Player)) {
                other.GetComponent<PlayerController>().Kill();
            }
        }

    }
}
