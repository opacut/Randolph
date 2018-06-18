using System;
using Randolph.Core;
using Randolph.Levels;
using Randolph.UI;

namespace Randolph.Interactable {
    public abstract class Pickable : Clickable, IPickable {

        public override Cursors CursorType { get; protected set; } = Cursors.Pick;

        public abstract bool IsSingleUse { get; }
        
        public bool IsPickedUp { get; private set; }

        protected override void Start() {
            base.Start();
            gameObject.tag = Constants.Tag.Pickable;
        }

        /// <summary>What should happen when the object is picked. The "Mouse Exit" event is invoked in the base class.</summary>
        public virtual void Pick() {
            if (IsPickedUp) {
                return;
            }
            IsPickedUp = true;

            ResetCursor();
            shouldOutline = false;
            outline.enabled = false;
            OnPick?.Invoke();
        }
        public event Action OnPick;
        
        public override void Restart() {
            base.Restart();
            IsPickedUp = false;
        }
    }
}
