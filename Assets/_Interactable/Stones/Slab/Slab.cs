using Randolph.UI;

namespace Randolph.Interactable {
    public class Slab : Clickable {
        public override Cursors CursorType { get; protected set; } = Cursors.Inspect;
    }
}
