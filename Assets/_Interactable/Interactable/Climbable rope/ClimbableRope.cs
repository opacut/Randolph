using Randolph.Characters;
using Randolph.Levels;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Randolph.Interactable {
    [RequireComponent(typeof(LineRenderer))]
    public class ClimbableRope : Interactable {
        [SerializeField] Transform tiePosition;
        PlayerController player;
        LineRenderer lineRenderer;
        

        protected override void Awake() {
            base.Awake();
            lineRenderer = GetComponent<LineRenderer>();
            lineRenderer.SetPositions(new[] {transform.position, tiePosition.position});
            LevelManager.OnNewLevel += OnNewLevel;            
        }

        void OnNewLevel(Scene scene, PlayerController player) {
            this.player = player;
            this.player.OnStoppedGrappling += OnStoppedGrappling;
        }

        public override void Interact() {
            base.Interact();
            player.GrappleTo(tiePosition.position);
            gameObject.SetActive(false);
        }

        void OnStoppedGrappling() {
            gameObject.SetActive(true);
        }

        void OnDestroy() {
            LevelManager.OnNewLevel -= OnNewLevel;
            player.OnStoppedGrappling -= OnStoppedGrappling;
        }
    }
}
