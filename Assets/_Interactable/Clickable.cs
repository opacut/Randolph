﻿using cakeslice;
using Randolph.Core;
using Randolph.Levels;
using Randolph.UI;
using UnityEngine;

namespace Randolph.Interactable {
    [RequireComponent(typeof(Outline))]
    [RequireComponent(typeof(SpriteRenderer))]
    public abstract class Clickable : RestartableBase {
        [Tooltip("Randolph's comment - keep empty if none.")]
        [SerializeField, TextArea]
        private string description;

        protected Outline outline;
        protected bool shouldOutline;

        protected SpriteRenderer spriteRenderer;

        public virtual bool isWithinReach => Vector2.Distance(transform.position, Constants.Randolph.transform.position) <= Inventory.inventory.ApplicableDistance;

        /// <summary>Type of cursor to use. Override in a derived class.</summary>
        public abstract Cursors CursorType { get; protected set; }

        protected virtual void Awake() {
            outline = GetComponent<Outline>();
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        protected override void Start() {
            base.Start();
            outline.enabled = false;
            shouldOutline = false;
        }

        protected virtual void Update() {
            if (!PauseMenu.IsPaused) {
                outline.enabled = spriteRenderer.enabled && (Input.GetAxis("Highlight") != 0.0f || shouldOutline);
            }
        }

        private void OnDisable() {
            // Mouse Up + Mouse Exit when deleted
            ResetCursor();
        }

        protected void ResetCursor() => CursorManager.cursorManager.SetCursorDefault();

        /// <summary>Randolph's comment.</summary>
        public string GetDescription() => description.Trim();

        #region MouseEvents
        public delegate void MouseClickable(Clickable target);

        public delegate void MouseClickableButton(Clickable target, Constants.MouseButton button);

        public static event MouseClickable OnMouseEnterClickable;
        public static event MouseClickable OnMouseExitClickable;
        public static event MouseClickableButton OnMouseDownClickable;
        public static event MouseClickableButton OnMouseUpClickable;

        private void OnMouseEnter() {
            OnMouseEnterClickable?.Invoke(this);
            shouldOutline = true;
        }

        private void OnMouseExit() {
            OnMouseExitClickable?.Invoke(this);
            shouldOutline = false;
        }

        private void OnMouseOver() {
            for (var i = 0; i <= 2; i++) {
                // Check for all mouse buttons
                if (Input.GetMouseButtonDown(i)) {
                    OnMouseDownClickable?.Invoke(this, (Constants.MouseButton) i);
                } else if (Input.GetMouseButtonUp(i)) {
                    OnMouseUpClickable?.Invoke(this, (Constants.MouseButton) i);
                }
            }
        }
        #endregion

        #region IRestartable
        public override void Restart() {
            base.Restart();
            shouldOutline = false;
        }
        #endregion
    }
}
