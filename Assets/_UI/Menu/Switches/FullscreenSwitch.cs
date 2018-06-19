using UnityEngine;
using UnityEngine.EventSystems;

namespace Randolph.UI {
    /// <summary>The switch changing between the windowed and the fullscreen mode.</summary>
    public class FullscreenSwitch : MenuSwitch {

        Vector2 screenSize;

        public override bool Active {
            get { return Screen.fullScreen; }
            protected set { Screen.fullScreen = value; }
        }

        /// <summary>Does nothing. The fullscreen state does not need to be refreshed when the game starts.</summary>
        protected override void RefreshState() { }

        protected override void Awake() {
            base.Awake();
            screenSize = new Vector2(Screen.width, Screen.height);
        }

        void Update() {
            var currentScreenSize = new Vector2(Screen.width, Screen.height);
            if (!screenSize.Equals(currentScreenSize)) {
                // Screen size changed
                SpriteSwap(Screen.fullScreen); // swap accordingly to the fullscreen mode
                screenSize = currentScreenSize;
            }
        }

        public override void OnPointerDown(PointerEventData pointerEventData) {
            Screen.fullScreen = !Screen.fullScreen;
            if (Application.isEditor) Debug.Log("Switching fullscreen.");
            // base.OnPointerDown(); ← Don't play any sound
        }

    }
}
