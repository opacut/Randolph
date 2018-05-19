using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Randolph.Environment {
	public class CloudEmitter : MonoBehaviour {
		public GameObject cloudPrefab;
		public Sprite[] cloudSprites;

		public float spawnSpeed;

		public float cloudSpeed;

		public string sortingLayer;
		public int distanceMin;
		public int distanceMax;

		public float endX;
		public float minY;
		public float maxY;

		public Color cloudColor = Color.white;

		private void Start() {
			for (int i = 0; i < 30; ++i) {
				var cloud = SpawnCloud();
				cloud.transform.Translate(-(cloudSpeed * i), 0, 0);
			}
			InvokeRepeating("SpawnCloud", 0.0f, spawnSpeed);
		}

		private Cloud SpawnCloud() {
			var cloud = Instantiate(cloudPrefab, transform).GetComponent<Cloud>();
			cloud.transform.Translate(0, Random.Range(minY, maxY), 0);
			cloud.sprite = cloudSprites[Random.Range(0, cloudSprites.Length)];
			cloud.spriteRenderer.sortingLayerName = sortingLayer;
			cloud.spriteRenderer.sortingOrder = Random.Range(distanceMin, distanceMax);
			cloud.endX = endX;
			cloud.color = cloudColor;
			cloud.speed = cloudSpeed;
			return cloud;
		}
	}
}