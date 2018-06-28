using System;
using System.Threading.Tasks;
using Com.LuisPedroFonseca.ProCamera2D;
using Randolph.Core;
using Randolph.Interactable;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Randolph.Levels.Airship {
    [RequireComponent(typeof(BoxCollider2D))]
    public class TutorialEnd : RestartableBase {
        [SerializeField] private TiedRope frontMastRope;
        [SerializeField] private TiedRope backMastRope;

        private void OnTriggerEnter2D(Collider2D other) {
            if (!other.CompareTag(Constants.Tag.Player)) {
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

            Constants.Randolph.gameObject.SetActive(false);
            Constants.Camera.rooms.EnterRoom(7, false);
            Constants.Camera.transition.TransitionEnter();
            
            await Task.Delay(TimeSpan.FromSeconds(5));
            SceneManager.LoadScene("Level 1");
        }
    }
}
