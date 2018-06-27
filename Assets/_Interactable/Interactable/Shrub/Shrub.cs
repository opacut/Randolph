using UnityEngine;

namespace Randolph.Interactable {
    public class Shrub : Interactable, ISlashable {
        [SerializeField] public Twig twig;

        public void Slash() {
            var newTwig = Instantiate(twig);
            newTwig.Pick();
            gameObject.SetActive(false);
        }
    }
}
