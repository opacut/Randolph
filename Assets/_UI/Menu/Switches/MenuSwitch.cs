using Randolph.Core;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Randolph.UI {
    /// <summary>Base class for all UI toggles.</summary>
    [RequireComponent(typeof(Image)), RequireComponent(typeof(Collider2D))]
    public abstract class MenuSwitch : MonoBehaviour, IPointerDownHandler {

        protected Image image;
        /// <summary>Is the switch currently turned on?</summary>
        public abstract bool Active { get; protected set; }
        /// <summary>The <see cref="PlayerPrefs"/> key used to load last value even after the game has been shut down.</summary>
        // public virtual string SaveKey { get { return "";} }

        /// <summary>The sound to play when the switch is clicked.</summary>
        [SerializeField] protected AudioClip soundOnClick;

        /// <summary>The sprite to use when the switch is turned on.</summary>
        [SerializeField] protected Sprite activeSprite;
        /// <summary>The sprite to use when the switch is turned off.</summary>
        [SerializeField] protected Sprite inactiveSprite;

        /// <summary>Caches the image component and sets its sprite according to the <see cref="Active"/> property.</summary>
        protected virtual void Start() {
            image = transform.GetComponentInChildren<Image>();
            SpriteSwap();
            // Active = LoadPlayerPrefs();
        }

        /// <summary>Plays a sound when the switch is clicked. Set <see cref="P:Randolph.UI.MenuSwitch.Active" /> and call <see cref="!:SpriteSwap" /> as necessary.</summary>
        public virtual void OnPointerDown(PointerEventData pointerEventData) {
            AudioPlayer.audioPlayer?.PlayGlobalSound(soundOnClick);
        }

        /// <summary>Sets the switch's sprite.</summary>
        /// <param name="active">Use the <see cref="activeSprite"/> or the <see cref="inactiveSprite"/> sprite?</param>
        protected void SpriteSwap(bool active) {
            image.sprite = (active) ? activeSprite : inactiveSprite;
        }
        /// <summary>Sets the switch's sprite in accordance with the <see cref="Active"/> property.</summary>
        protected void SpriteSwap() {
            SpriteSwap(Active);
        }
        /*
         bool LoadPlayerPrefs() {
             if (!string.IsNullOrEmpty(SaveKey) && PlayerPrefs.HasKey(SaveKey)) {
                 bool wasActive = (Mathf.Approximately(PlayerPrefs.GetInt(SaveKey), 1));
                 return wasActive;
             } else return false;
         }
         */
    }

}