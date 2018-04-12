using Randolph.Characters;
using Randolph.Core;
using UnityEngine;

namespace Randolph.Environment {
    [RequireComponent(typeof(SpriteRenderer))]
    public class SpikeTrap : MonoBehaviour {

        [SerializeField] bool On;
        [SerializeField] Sprite active;
        [SerializeField] Sprite disabled;

        SpriteRenderer spriteRenderer;

        void Awake() {
            spriteRenderer = GetComponent<SpriteRenderer>();
            spriteRenderer.sprite = On ? active : disabled;
        }

        public void Toggle() {
            On = !On;
            spriteRenderer.sprite = On ? active : disabled;
        }

        public void OnTriggerStay2D(Collider2D other) {
            if (On && (other.tag == Constants.Tag.Player)) {
                other.GetComponent<PlayerController>().Kill();
            }
        }

    }
}
