using System.Collections.Generic;
using UnityEngine;

namespace Randolph.Interactable {
    public class GrassTuft : Interactable, ISlashable {
        [SerializeField] private Sprite cutGrass;
        [SerializeField] public Transform Grass;

        private readonly List<GameObject> grasses = new List<GameObject>();

        private bool hasGrass = true;
        private Sprite original;

        [SerializeField] private GameObject spawnPoint;

        public void Slash() {
            if (hasGrass) {
                hasGrass = false;
                spriteRenderer.sprite = cutGrass;
                var newGrass = Instantiate(Grass, spawnPoint.transform.position, Quaternion.identity).gameObject;
                newGrass.transform.parent = gameObject.transform.parent;
                grasses.Add(newGrass);
            }
        }

        protected override void Awake() {
            base.Awake();
            original = spriteRenderer.sprite;
        }

        public override void Restart() {
            base.Restart();
            grasses.ForEach(y => Destroy(y));
            hasGrass = true;
            spriteRenderer.sprite = original;
        }
    }
}
