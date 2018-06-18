using UnityEngine;

namespace Randolph.Interactable {
    public class Shrub : Interactable, ISlashable {
        [SerializeField] private GameObject spawnPoint;
        [SerializeField] public Transform twig;

        public void Slash() => Instantiate(twig, spawnPoint.transform.position, Quaternion.identity);
    }
}
