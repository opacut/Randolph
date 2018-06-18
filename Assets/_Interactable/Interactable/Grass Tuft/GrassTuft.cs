using UnityEngine;

namespace Randolph.Interactable {
    public class GrassTuft : Interactable, ISlashable {
        [SerializeField] public Transform Grass;
        [SerializeField] private Sprite cutGrass;

        private bool hasGrass = true;
        private Sprite original;

        [SerializeField] private GameObject spawnPoint;

        public void Slash() {
            if (hasGrass) {
                hasGrass = false;
                spriteRenderer.sprite = cutGrass;
                Instantiate(Grass, spawnPoint.transform.position, Quaternion.identity);
            }
        }

        protected override void Awake() {
            base.Awake();
            original = spriteRenderer.sprite;
        }

        public override void Restart() {
            base.Restart();
            hasGrass = true;
            spriteRenderer.sprite = original;
        }

        public override void Interact() { Debug.Log("Tuft clicked"); }
    }
}