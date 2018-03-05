using UnityEngine;
using UnityEngine.UI;

// using Core.Managers;
namespace Randolph.UI {
    /// <summary>The switch changing between the Czech and English language.</summary>
    public class LanguageSwitch : MenuSwitch {
        public override bool Active { get; protected set; } // Czech

        protected override void Start() {
            // Active = (TextManager.textManager.Language == Languages.Česky); // Check current language
            base.Start();
        }

        protected override void OnMouseDown() {
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
            SpriteSwap(Active);            
            base.OnMouseDown();
        }

    }
}
