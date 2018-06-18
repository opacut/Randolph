using System.Collections.Generic;
using System.Linq;
using Randolph.Core;
using Randolph.Levels;
using UnityEngine;

// TODO: Derive from abstract class, IRestartable
namespace Randolph.Environment {
    [RequireComponent(typeof(SpriteRenderer))]
    public class Switch : MonoBehaviour, IRestartable {
        [Header("Sprites")]
        [SerializeField]
        private Sprite activeSprite;

        private AudioSource audioSource;

        [SerializeField]
        private Sprite inactiveSprite;

        [SerializeField]
        private bool Powered;

        [Header("Switch")]
        [SerializeField]
        private List<SpikeTrap> Spikes;

        private SpriteRenderer spriteRederer;

        [SerializeField]
        private AudioClip switchSound;

        private void Awake() {
            spriteRederer = GetComponent<SpriteRenderer>();
            spriteRederer.sprite = Powered ? activeSprite : inactiveSprite;
            audioSource = AudioPlayer.audioPlayer.AddAudioSource(gameObject);
            SaveState();
        }

        private void OnTriggerEnter2D(Collider2D other) {
            // Powered = !Powered;

            // spriteRederer.sprite = Powered ? inactiveSprite : activeSprite;
            // Spikes.ForEach(s => s.Toggle());
            Flip(Powered);
        }

        private void OnDrawGizmosSelected() {
            if (Spikes.Count == 0) {
                return;
            }
            var radius = Constants.GizmoSphereRadius;

            //! Switch
            Gizmos.color = Powered ? Color.green : Color.red;
            var lastPosition = transform.position;
            for (var i = 0; i < 3; i++) {
                // Three small circles
                Methods.GizmosDrawCircle(lastPosition, radius * (1 - i * 0.33f));
            }
            var nearestPosition = transform.GetNearest(Spikes.Select(spike => spike.transform).ToArray()).position;
            var direction = (lastPosition - nearestPosition).normalized;
            Gizmos.DrawLine(lastPosition - direction * radius, nearestPosition + direction * radius);

            //! Spikes
            Gizmos.color = Color.yellow;
            for (var i = 0; i < Spikes.Count; i++) {
                var spikeTrap = Spikes[i];
                if (spikeTrap == null) {
                    continue;
                }

                if (i != 0) {
                    direction = (lastPosition - spikeTrap.transform.position).normalized;
                    Gizmos.DrawLine(lastPosition - direction * radius, spikeTrap.transform.position + direction * radius);
                }

                lastPosition = spikeTrap.transform.position;
                Methods.GizmosDrawCircle(lastPosition, radius);
            }
        }

        private void Flip(bool active) {
            Powered = !active;

            spriteRederer.sprite = active ? activeSprite : inactiveSprite;
            AudioPlayer.audioPlayer.PlayLocalSound(audioSource, switchSound, volume: Constants.Audio.BackgroundVolume);
            Spikes.ForEach(s => s.Toggle());
        }

        #region IRestartable
        private bool initialPowered;

        public void SaveState() {
            initialPowered = Powered;
        }

        public void Restart() {
            Powered = initialPowered;
        }
        #endregion
    }
}
