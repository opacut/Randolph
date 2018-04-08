using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Randolph.Environment {
    [RequireComponent(typeof(SpriteRenderer))]
    public class PressurePlate : MonoBehaviour {
        public bool Powered { get { return holding == 0; } }
        public List<SpikeTrap> Spikes;
        public Sprite activeSprite;
        public Sprite inactiveSprite;

        private int holding = 0;

        private void OnTriggerEnter2D(Collider2D other) {
            if (holding == 0) {
                Spikes.ForEach(s => s.Toggle());
                GetComponent<SpriteRenderer>().sprite = inactiveSprite;
            }
            ++holding;
        }

        private void OnTriggerExit2D(Collider2D collision) {
            --holding;
            if (holding == 0) {
                Spikes.ForEach(s => s.Toggle());
                GetComponent<SpriteRenderer>().sprite = activeSprite;
            }
        }
    }
}