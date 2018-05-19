using UnityEngine;

namespace Randolph.UI {    
    [System.Serializable]
    public struct GameCursor {

        public string name;
        public Texture2D overCursor;
        public Texture2D disabledCursor;
        public Texture2D pressedCursor;

        public GameCursor(string name = "Cursor", Texture2D overCursor = null, Texture2D disabledCursor = null, Texture2D pressedCursor = null) {
            this.name = name;
            this.overCursor = overCursor;
            this.disabledCursor = disabledCursor;
            this.pressedCursor = pressedCursor;
        }

    }
}
