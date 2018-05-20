using Randolph.Levels;
using Randolph.UI;
using UnityEngine;

namespace Randolph.Interactable {
    public abstract class Pickable : Clickable, IPickable {

        public override Cursors CursorType { get; protected set; } = Cursors.Pick;

        public abstract bool IsSingleUse { get; }
        
        /// <summary>What should happen when the object is picked. The "Mouse Exit" event is invoked in the base class.</summary>
        public virtual void OnPick() {
            ResetCursor();
        }       

    }
}
