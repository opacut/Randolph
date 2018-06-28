using System.Collections;
using Randolph.Core;
using Randolph.Levels;
using UnityEngine;
using Randolph.Interactable;
using Randolph.UI;

namespace Randolph.Characters {
    public class Spitter : Clickable {

        [SerializeField] GameObject shot;
        [SerializeField] Transform shotSpawn;
        [SerializeField] AudioClip shotSound;
        [SerializeField] float fireRate;
        [SerializeField] float initialDelay;
        AudioSource audioSource;
        Transform currentArea;

        Coroutine shootingCO;

        public override Cursors CursorType { get; protected set; } = Cursors.Inspect;

        protected override void Start() {
            base.Start();
            audioSource = AudioPlayer.audioPlayer.AddAudioSource(gameObject);
            currentArea = GetComponentInParent<Area>()?.transform;
            outline.color = 2;
        }

        void Fire() {
            AudioPlayer.audioPlayer.PlayLocalSound(audioSource, shotSound);
            Instantiate(shot, shotSpawn.position, shotSpawn.rotation, currentArea);
        }

        IEnumerator KeepShooting(float fireRate, float initialDelay) {
            yield return new WaitForSeconds(initialDelay);
            while (true) {
                Fire();
                yield return new WaitForSeconds(fireRate);
            }
        }

        public void Kill() { }

        void OnBecameVisible() {
            // Shoot only when being onscreen
            shootingCO = StartCoroutine(KeepShooting(fireRate, initialDelay));
        }

        void OnBecameInvisible() {
            if (shootingCO != null) StopCoroutine(shootingCO);
        }

    }
}
