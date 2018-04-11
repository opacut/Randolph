using System.Collections.Generic;
using System.Linq;

using Randolph.Core;

using UnityEngine;
// TODO: Derive from abstract class
namespace Randolph.Environment {
    [RequireComponent(typeof(SpriteRenderer))]
    public class PressurePlate : MonoBehaviour {
        public bool Powered { get { return holding == 0; } }
        [SerializeField] List<SpikeTrap> Spikes;
        [SerializeField] Sprite activeSprite;
        [SerializeField] Sprite inactiveSprite;

        SpriteRenderer spriteRederer;
        int holding = 0;

        void Awake() {
            spriteRederer = GetComponent<SpriteRenderer>();
        }

        void OnTriggerEnter2D(Collider2D other) {
            if (holding == 0) {
                Spikes.ForEach(s => s.Toggle());
                spriteRederer.sprite = inactiveSprite;
            }
            ++holding;
        }

        void OnTriggerExit2D(Collider2D collision) {
            --holding;
            if (holding == 0) {
                Spikes.ForEach(s => s.Toggle());
                spriteRederer.sprite = activeSprite;
            }
        }

        void OnDrawGizmosSelected() {
            if (Spikes.Count == 0) return;
            float radius = Constants.GizmoSphereRadius;

            //! Switch
            Gizmos.color = (Powered) ? Color.green : Color.red;
            Vector3 lastPosition = transform.position;
            for (int i = 0; i < 3; i++) {
                // Three small circles
                Methods.GizmosDrawCircle(lastPosition, radius * (1 - i * 0.33f));
            }
            Vector3 nearestPosition = transform.GetNearest(Spikes.Select(spike => spike.transform).ToArray()).position;
            Vector3 direction = (lastPosition - nearestPosition).normalized;
            Gizmos.DrawLine(lastPosition - (direction * radius), nearestPosition + (direction * radius));

            //! Spikes
            Gizmos.color = Color.yellow;
            for (int i = 0; i < Spikes.Count; i++) {
                SpikeTrap spikeTrap = Spikes[i];
                if (spikeTrap == null) continue;

                if (i != 0) {
                    direction = (lastPosition - spikeTrap.transform.position).normalized;
                    Gizmos.DrawLine(lastPosition - (direction * radius), spikeTrap.transform.position + (direction * radius));
                }

                lastPosition = spikeTrap.transform.position;
                Methods.GizmosDrawCircle(lastPosition, radius);
            }
        }
    }
}