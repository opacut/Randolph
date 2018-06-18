using Randolph.Characters;
using Randolph.Core;
using Randolph.Levels;
using UnityEngine;

namespace Randolph.Environment {
    [RequireComponent(typeof(SpriteRenderer))]
    public class SpikeTrap : MonoBehaviour, IRestartable {

        [SerializeField] private bool isUp;
        [SerializeField] private Sprite activeSprite;
        [SerializeField] private Sprite inactiveSprite;

        private SpriteRenderer spriteRenderer;

        private void Awake() {
            spriteRenderer = GetComponent<SpriteRenderer>();
            spriteRenderer.sprite = isUp ? activeSprite : inactiveSprite;

            SaveState();
        }

        public void OnTriggerStay2D(Collider2D other) {
            if (isUp && (other.tag == Constants.Tag.Player)) {
                other.GetComponent<PlayerController>().Kill();
            }
        }

        public void Toggle() => Toggle(!isUp);

        public void Toggle(bool active) {
            isUp = active;
            spriteRenderer.sprite = active ? activeSprite : inactiveSprite;
        }

        #region IRestartable
        private bool initialIsUp;

        public void SaveState() {
            initialIsUp = isUp;
        }

        public void Restart() {
            Toggle(initialIsUp);
        }
        #endregion
    }
}
