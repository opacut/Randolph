﻿using Randolph.Core;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Randolph.UI {
    public class MuteSwitch : MenuSwitch {

        public const string MuteKey = "Mute";
        public override bool Active { get; protected set; }

        const int On = 1;
        const int Off = 0;        

        /// <inheritdoc cref="MenuSwitch.RefreshState"/>
        protected override void RefreshState() {            
            if (!PlayerPrefs.HasKey(MuteKey)) {
                PlayerPrefs.SetInt(MuteKey, Off);
            }
            Active = PlayerPrefs.GetInt(MuteKey) == On;
            AudioListener.volume = (Active) ? 0 : AudioPlayer.GlobalVolume;
        }

        public override void OnPointerDown(PointerEventData pointerEventData) {
            FlipMute();
            base.OnPointerDown(pointerEventData);
        }

        /// <summary>Turns the sound ON if it was OFF, and turns the sound OFF if it was ON.</summary>
        void FlipMute() {
            Active = !Active;
            PlayerPrefs.SetInt(MuteKey, (Active) ? On : Off);
            AudioListener.volume = (Active) ? 0 : AudioPlayer.GlobalVolume; // swap values
            SpriteSwap();
        }

    }
}
