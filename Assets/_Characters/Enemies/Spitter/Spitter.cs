using UnityEngine;

public class Spitter : MonoBehaviour, IRestartable {
    public GameObject shot;
    public Transform shotSpawn;
    public float fireRate;
    public float delay;

    // Use this for initialization
    void Start() {
        InvokeRepeating("Fire", delay, fireRate);   // TODO: OnBecameVisible() / distance to player
    }

    void Fire() {
        Instantiate(shot, shotSpawn.position, shotSpawn.rotation);
    }

    public void Restart() {
        return;
    }
}
