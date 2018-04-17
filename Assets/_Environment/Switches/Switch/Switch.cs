using System.Collections.Generic;
using System.Linq;
using Randolph.Core;
using Randolph.Levels;
using UnityEngine;

// TODO: Derive from abstract class, IRestartable
namespace Randolph.Environment {
    [RequireComponent(typeof(SpriteRenderer))]
    public class Switch : MonoBehaviour, IRestartable {

        [Header("Switch")]
        [SerializeField] List<SpikeTrap> Spikes;
        [SerializeField] bool Powered;
        [SerializeField] AudioClip switchSound;
        AudioSource audioSource;
        bool initialPowered;

        [Header("Sprites")]
        [SerializeField] Sprite activeSprite;
        [SerializeField] Sprite inactiveSprite;

        SpriteRenderer spriteRederer;

        void Awake() {
            spriteRederer = GetComponent<SpriteRenderer>();
            spriteRederer.sprite = (Powered) ? activeSprite : inactiveSprite;
            initialPowered = Powered;
            audioSource = AudioPlayer.audioPlayer.AddAudioSource(gameObject);
        }

        public void Restart() {
            Powered = initialPowered;
        }

        void OnTriggerEnter2D(Collider2D other) {
            // Powered = !Powered;

            // spriteRederer.sprite = Powered ? inactiveSprite : activeSprite;
            // Spikes.ForEach(s => s.Toggle());
            Flip(Powered);
        }

        void Flip(bool active) {
            Powered = !active;

            spriteRederer.sprite = (active) ? activeSprite : inactiveSprite;
            AudioPlayer.audioPlayer.PlayLocalSound(audioSource, switchSound, volume: Constants.Audio.BackgroundVolume);
            Spikes.ForEach(s => s.Toggle());
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
