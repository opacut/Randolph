using System.Collections;
using UnityEngine;

public class Spitter : MonoBehaviour, IRestartable {
    [SerializeField] GameObject shot;
    [SerializeField] Transform shotSpawn;
    [SerializeField] float fireRate;
    [SerializeField] float initialDelay;

    Coroutine shootingCO;

    void Fire() {
        Instantiate(shot, shotSpawn.position, shotSpawn.rotation);
    }

    IEnumerator KeepShooting(float fireRate, float initialDelay) {
        yield return new WaitForSeconds(initialDelay);
        while (true) {
            Fire();
            yield return new WaitForSeconds(fireRate);
        }
    }

    public void Restart() {
        return;
    }

    void OnBecameVisible() {
        // Shoot only when being onscreen
        shootingCO = StartCoroutine(KeepShooting(fireRate, initialDelay));
    }

    void OnBecameInvisible() {
        if (shootingCO != null) StopCoroutine(shootingCO);
    }
}