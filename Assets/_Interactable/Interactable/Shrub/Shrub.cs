using System.Collections.Generic;
using UnityEngine;

namespace Randolph.Interactable {
    public class Shrub : Interactable, ISlashable {
        [SerializeField] private GameObject spawnPoint;
        [SerializeField] public Transform twig;

        private readonly List<GameObject> twigs = new List<GameObject>();

        public void Slash() {
            var newTwig = Instantiate(twig, spawnPoint.transform.position, Quaternion.identity).gameObject;
            newTwig.transform.parent = gameObject.transform.parent;
            twigs.Add(newTwig);
        }

        public override void Restart() {
            base.Restart();
            twigs.ForEach(y => Destroy(y));
        }
    }
}
