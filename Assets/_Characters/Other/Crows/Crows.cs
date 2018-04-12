using Randolph.Core;
using Randolph.Levels;
using UnityEngine;

namespace Randolph.Characters {
    [RequireComponent(typeof(Glider))]
    public class Crows : MonoBehaviour, IRestartable {

        [SerializeField] AudioClip cawingSound;
        Animator animator;
        Glider glider;
        
        private void Awake() {
            animator = GetComponent<Animator>();
            glider = GetComponent<Glider>();

            glider.OnPlayerDisturbed += OnPlayerDisturbed;
        }

        public void Restart() {
            animator.SetBool("Move", false);
        }

        void OnPlayerDisturbed(PlayerController player) {
            animator.SetBool("Move", true);
            AudioSource.PlayClipAtPoint(cawingSound, Constants.Audio.AudioListener);
        }

    }
}
