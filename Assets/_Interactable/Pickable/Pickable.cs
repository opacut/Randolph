using System;
using Randolph.Core;
using Randolph.Levels;
using Randolph.UI;

namespace Randolph.Interactable {
    public abstract class Pickable : Clickable, IPickable {
        public override Cursors CursorType { get; protected set; } = Cursors.Pick;

        public bool IsPickedUp { get; private set; }

        public abstract bool IsSingleUse { get; }

        /// <summary>What should happen when the object is picked. The "Mouse Exit" event is invoked in the base class.</summary>
        public virtual void Pick() {
            if (IsPickedUp) {
                return;
            }
            IsPickedUp = true;

            ResetCursor();
            shouldOutline = false;
            OnPick?.Invoke();
        }

        public override void Restart() {
            base.Restart();
            IsPickedUp = false;
        }

        protected override void Start() {
            base.Start();
            gameObject.tag = Constants.Tag.Pickable;
        }

        public event Action OnPick;
    }
}
