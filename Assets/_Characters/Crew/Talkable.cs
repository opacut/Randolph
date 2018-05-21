using Randolph.Interactable;
using Randolph.UI;

namespace Randolph.Characters {
    public class Talkable : Clickable {
        protected override void Start() {
            base.Start();
            outline.color = 1;
        }

        public override Cursors CursorType { get; protected set; } = Cursors.Talk;

        public virtual void OnTalk() { }

    }
}
