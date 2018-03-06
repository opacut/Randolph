using UnityEngine;
using UnityEngine.EventSystems;

namespace Randolph.UI {
    public class MuteSwitch : MenuSwitch {

        public const string MuteKey = "Mute";
        public override bool Active { get; protected set; }

        protected override void Start() {
            // Save/load previous mute state
            if (PlayerPrefs.HasKey(MuteKey)) {
                AudioListener.volume = PlayerPrefs.GetInt(MuteKey);
                Active = Mathf.Approximately(AudioListener.volume, 0);
            }

            base.Start();
        }

        public override void OnPointerDown(PointerEventData pointerEventData) {
            FlipMute();
            base.OnPointerDown(pointerEventData);
        }

        /// <summary>Turns the sound ON if it was OFF, and turns the sound OFF if it was ON.</summary>
        void FlipMute() {
            AudioListener.volume = (Active) ? 1 : 0; // swap values
            PlayerPrefs.SetInt(MuteKey, (Active) ? 1 : 0); // remember the state            
            Active = !Active;
            SpriteSwap();
        }
    }
}