using UnityEngine;
using UnityEngine.UI;

namespace Randolph.UI {    
    /// <summary>Base class for all UI toggles.</summary>
    [RequireComponent(typeof(Image)), RequireComponent(typeof(Collider2D))]    
    public abstract class MenuSwitch : MonoBehaviour {

        protected Image image;        
        public abstract bool Active { get; protected set; }

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
        }

        /// <summary>Plays a sound when the switch is clicked. Set <see cref="Active"/> and call <see cref="SpriteSwap"/> as necessary.</summary>
        protected virtual void OnMouseDown() {
            if (soundOnClick) AudioSource.PlayClipAtPoint(soundOnClick, Camera.main.transform.position);
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
    }

}