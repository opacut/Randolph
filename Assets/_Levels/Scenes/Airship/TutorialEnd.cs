using System;
using System.Threading.Tasks;
using Com.LuisPedroFonseca.ProCamera2D;
using Randolph.Core;
using Randolph.Interactable;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Randolph.Levels.Airship {
    [RequireComponent(typeof(BoxCollider2D))]
    public class TutorialEnd : MonoBehaviour {
        [SerializeField] private TiedRope frontMastRope;
        [SerializeField] private TiedRope backMastRope;

        private void OnTriggerEnter2D(Collider2D other) {
            if (other.tag != Constants.Tag.Player) {
                return;
            }

            if (frontMastRope != null || backMastRope != null) {
                // Airship goin' down
                Transition();
            }
        }

        private async void Transition() {
            Constants.Camera.transition.TransitionExit();
            await Task.Delay(TimeSpan.FromSeconds(Constants.Camera.transition.DurationExit));
            SceneManager.LoadScene("Level 1");
        }
    }
}
