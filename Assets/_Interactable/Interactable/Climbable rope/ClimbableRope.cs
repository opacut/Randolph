using Randolph.Core;
using UnityEngine;

namespace Randolph.Interactable {
    [RequireComponent(typeof(LineRenderer))]
    public class ClimbableRope : Interactable {
        [SerializeField]
        private Hook hook;

        private LineRenderer lineRenderer;


        protected override void Awake() {
            base.Awake();
            lineRenderer = GetComponent<LineRenderer>();
            lineRenderer.SetPositions(new[] { transform.position, hook.transform.position });
            Constants.Randolph.OnStoppedGrappling += OnStoppedGrappling;
            hook.OnPick += OnHookPicked;
        }


        public override void Interact() {
            base.Interact();
            Constants.Randolph.GrappleTo(hook.transform.position);
            gameObject.SetActive(false);
        }

        private void OnStoppedGrappling() => gameObject.SetActive(true);

        private void OnHookPicked() {
            gameObject.SetActive(false);
            Constants.Randolph.OnStoppedGrappling -= OnStoppedGrappling;
        }

        private void OnDrawGizmos() {
            Gizmos.color = GetComponent<LineRenderer>().startColor;
            Gizmos.DrawLine(transform.position, hook.transform.position);
        }

        #region IRestartable
        public override void Restart() {
            base.Restart();
            Constants.Randolph.OnStoppedGrappling -= OnStoppedGrappling;
            Constants.Randolph.OnStoppedGrappling += OnStoppedGrappling;
            hook.OnPick -= OnHookPicked;
            hook.OnPick += OnHookPicked;
        }
        #endregion
    }
}
