using Randolph.Characters;
using Randolph.Core;
using Randolph.Levels;
using UnityEngine;

namespace Randolph.Environment {
    [RequireComponent(typeof(SpriteRenderer))]
    public class SpikeTrap : RestartableBase {

        [SerializeField] private bool isUp;
        [SerializeField] private Sprite activeSprite;
        [SerializeField] private Sprite inactiveSprite;

        private SpriteRenderer spriteRenderer;

        private void Awake() {
            spriteRenderer = GetComponent<SpriteRenderer>();
            spriteRenderer.sprite = isUp ? activeSprite : inactiveSprite;
        }

        public void OnTriggerStay2D(Collider2D other) {
            if (isUp && (other.CompareTag(Constants.Tag.Player))) {
                other.GetComponent<PlayerController>().Kill();
            }
        }

        public void Toggle() => Toggle(!isUp);

        public void Toggle(bool active) {
            isUp = active;
            spriteRenderer.sprite = active ? activeSprite : inactiveSprite;
        }

        #region IRestartable
        private bool savedIsUp;

        public override void SaveState() {
            base.SaveState();
            savedIsUp = isUp;
        }

        public override void Restart() {
            base.Restart();
            Toggle(savedIsUp);
        }
        #endregion
    }
}
