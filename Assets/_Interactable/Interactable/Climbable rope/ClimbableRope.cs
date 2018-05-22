using Randolph.Characters;
using UnityEngine;

namespace Randolph.Interactable {
    [RequireComponent(typeof(LineRenderer))]
    public class ClimbableRope : Interactable {
        [SerializeField] private Transform tiePosition;
        private LineRenderer lineRenderer;

        protected override void Awake() {
            base.Awake();
            lineRenderer = GetComponent<LineRenderer>();
            lineRenderer.SetPositions(new Vector3[] {transform.position, tiePosition.position});
            FindObjectOfType<PlayerController>().OnStoppedGrappling += OnStoppedGrappling;
        }

        public override void OnInteract() {
            FindObjectOfType<PlayerController>().GrappleTo(tiePosition.position);
            gameObject.SetActive(false);
        }

        private void OnStoppedGrappling() {
            gameObject.SetActive(true);
        }

        private void OnDestroy() {
            FindObjectOfType<PlayerController>().OnStoppedGrappling -= OnStoppedGrappling;
        }
    }
}
