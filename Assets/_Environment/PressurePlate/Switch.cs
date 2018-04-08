using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Randolph.Environment {
    [RequireComponent(typeof(SpriteRenderer))]
    public class Switch : MonoBehaviour {
        public bool Powered;
        public List<SpikeTrap> Spikes;
        public Sprite activeSprite;
        public Sprite inactiveSprite;

        private void OnTriggerEnter2D(Collider2D other) {
            Powered = !Powered;

            GetComponent<SpriteRenderer>().sprite = Powered ? inactiveSprite : activeSprite;
            Spikes.ForEach(y => y.Toggle());
        }
    }
}