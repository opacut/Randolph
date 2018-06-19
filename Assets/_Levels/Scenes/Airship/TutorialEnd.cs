using System;
using System.Threading.Tasks;
using Com.LuisPedroFonseca.ProCamera2D;
using Randolph.Core;
using Randolph.Interactable;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Randolph.Levels.Airship {
    [RequireComponent(typeof(BoxCollider2D))]
    public class TutorialEnd : MonoBehaviour, IRestartable {
        [SerializeField] private TiedRope frontMastRope;
        [SerializeField] private TiedRope backMastRope;

        private void Start() => SaveState();

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
            Constants.Camera.rooms.EnterRoom(7);
            Constants.Camera.transition.TransitionEnter();
            
            await Task.Delay(TimeSpan.FromSeconds(5));
            SceneManager.LoadScene("Level 1");
        }

        #region IRestartable
        private bool savedActiveState;

        public virtual void SaveState() {
            savedActiveState = gameObject.activeSelf;
        }

        public virtual void Restart() {
            gameObject.SetActive(savedActiveState);
        }
        #endregion
    }
}
