using Randolph.Core;
using UnityEngine;

namespace Randolph.Interactable {
    [RequireComponent(typeof(LineRenderer))]
    public class ClimbableRope : Interactable {
        private LineRenderer lineRenderer;

        [SerializeField]
        private Transform tiePosition;


        protected override void Awake() {
            base.Awake();
            lineRenderer = GetComponent<LineRenderer>();
            lineRenderer.SetPositions(new[] { transform.position, tiePosition.position });
            Constants.Randolph.OnStoppedGrappling += OnStoppedGrappling;
        }

        public override void Interact() {
            base.Interact();
            Constants.Randolph.GrappleTo(tiePosition.position);
            gameObject.SetActive(false);
        }

        private void OnStoppedGrappling() {
            gameObject.SetActive(true);
        }

        private void OnDisable() {
            Constants.Randolph.OnStoppedGrappling -= OnStoppedGrappling;
        }

        private void OnDrawGizmos() {
            Gizmos.color = GetComponent<LineRenderer>().startColor;
            Gizmos.DrawLine(transform.position, tiePosition.position);
        }
    }
}
