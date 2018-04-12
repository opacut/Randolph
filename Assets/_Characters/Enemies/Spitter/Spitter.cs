using System.Collections;
using Randolph.Core;
using Randolph.Levels;
using UnityEngine;

namespace Randolph.Characters {
    public class Spitter : MonoBehaviour, IRestartable {

        [SerializeField] GameObject shot;
        [SerializeField] Transform shotSpawn;
        [SerializeField] AudioClip shotSound;
        [SerializeField] float fireRate;
        [SerializeField] float initialDelay;
        AudioSource audioSource;

        Coroutine shootingCO;

        void Start() {
            audioSource = AudioPlayer.audioPlayer.AddLocalAudioSource(gameObject);
        }

        void Fire() {
            AudioPlayer.audioPlayer.PlayLocalSound(audioSource, shotSound);
            Instantiate(shot, shotSpawn.position, shotSpawn.rotation);
        }


        IEnumerator KeepShooting(float fireRate, float initialDelay) {
            yield return new WaitForSeconds(initialDelay);
            while (true) {
                Fire();
                yield return new WaitForSeconds(fireRate);
            }
        }

        public void Restart() { }

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
