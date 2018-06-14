using Randolph.Characters;
using Randolph.Core;
using Randolph.Levels;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Randolph.Interactable {
    [RequireComponent(typeof(LineRenderer))]
    public class ClimbableRope : Interactable {
        [SerializeField] Transform tiePosition;
        LineRenderer lineRenderer;
        

        protected override void Awake() {
            base.Awake();
            lineRenderer = GetComponent<LineRenderer>();
            lineRenderer.SetPositions(new[] {transform.position, tiePosition.position});
            Constants.Randolph.OnStoppedGrappling += OnStoppedGrappling;
        }

        public override void Interact() {
            base.Interact();
            Constants.Randolph.GrappleTo(tiePosition.position);
            gameObject.SetActive(false);
        }

        void OnStoppedGrappling() {
            gameObject.SetActive(true);
        }

        void OnDestroy() {
            Constants.Randolph.OnStoppedGrappling -= OnStoppedGrappling;
        }

        private void OnDrawGizmos() {
            Gizmos.color = GetComponent<LineRenderer>().startColor;
            Gizmos.DrawLine(transform.position, tiePosition.position);
        }
    }
}
