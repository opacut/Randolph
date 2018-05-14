using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudEmitter : MonoBehaviour {
	public GameObject cloudPrefab;
	public Sprite[] cloudSprites;

	public float minCloudSpeed;
	public float maxCloudSpeed;
	public float endX;
	public float minY;
	public float maxY;

	private void Start() {
		InvokeRepeating("LaunchProjectile", 2.0f, 0.3f);
	}
}