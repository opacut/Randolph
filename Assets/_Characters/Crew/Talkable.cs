using Randolph.Interactable;
using Randolph.UI;

namespace Randolph.Characters {
    public class Talkable : Clickable {

        public override Cursors CursorType { get; protected set; } = Cursors.Talk;

        public virtual void OnTalk() { }

    }
}
