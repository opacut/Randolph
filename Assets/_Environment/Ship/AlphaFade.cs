using UnityEngine;

namespace Randolph.Environment {
    [RequireComponent(typeof(SpriteRenderer))]
    public class AlphaFade : MonoBehaviour {
        private SpriteRenderer spriteRenderer;

        private void Start() { spriteRenderer = GetComponent<SpriteRenderer>(); }

        // Update is called once per frame
        private void Update() {
            var t = Mathf.Sin(Time.time * 2) / 2 + 0.5f;
            spriteRenderer.color = new Color(1.0f, 1.0f, 1.0f, t);
        }
    }
}
