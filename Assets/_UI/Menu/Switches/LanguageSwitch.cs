using UnityEngine;
using UnityEngine.EventSystems;

namespace Randolph.UI {
    /// <summary>The switch changing between the two languages.</summary>
    public class LanguageSwitch : MenuSwitch {
        public override bool Active { get; protected set; } // Czech

        protected override void Start() {
            // Active = (TextManager.textManager.Language == Languages.Česky); // Check current language
            base.Start();
        }

        public override void OnPointerDown(PointerEventData pointerEventData) {
            // TODO: Language Switch
           /*
            if (TextManager.textManager.Language == Languages.English) {
                TextManager.textManager.Language = Languages.Česky;
                    #if ARTICY
                    ArticyDatabase.Localization.Language = "cs";
                    #endif
            } else if (TextManager.textManager.Language == Languages.Česky) {
                TextManager.textManager.Language = Languages.English;
                    #if ARTICY
                    ArticyDatabase.Localization.Language = "en";
                    #endif
            }
            Active = (TextManager.textManager.Language == Languages.Česky);
            */
            Active = !Active;
            SpriteSwap(Active);        
            base.OnPointerDown(pointerEventData);
        }

    }
}
