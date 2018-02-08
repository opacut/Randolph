using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spitter : MonoBehaviour, IRestartable
{
    public GameObject shot;
    public Transform shotSpawn;
    public float fireRate;
    public float delay;

	// Use this for initialization
	void Start ()
    {
        InvokeRepeating("Fire", delay, fireRate);	
	}

    void Fire()
    {
        Instantiate(shot, shotSpawn.position, shotSpawn.rotation);
    }

    public void Restart()
    {
        return;
    }
}
