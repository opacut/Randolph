using UnityEngine;

namespace Randolph.UI {
    public class MuteSwitch : MenuSwitch {

        const string MuteKey = "Mute";
        public override bool Active { get; protected set; }

        protected override void Start() {
            // Save/load previous mute state
            if (PlayerPrefs.HasKey(MuteKey)) {
                AudioListener.volume = PlayerPrefs.GetInt(MuteKey);
                Active = Mathf.Approximately(AudioListener.volume, 0);
            }

            base.Start();
        }

        protected override void OnMouseDown() {
            AudioListener.volume = (Active) ? 1 : 0; // swap values
            PlayerPrefs.SetInt(MuteKey, (Active) ? 1 : 0); // remember the state
            SpriteSwap();
            Active = !Active;

            base.OnMouseDown();
        }
    }
}