using UnityEngine;
using UnityEngine.EventSystems;

namespace Randolph.UI {
    /// <summary>The switch changing between the two languages.</summary>
    public class LanguageSwitch : MenuSwitch {

        public static string Language {
            get {
                if (!PlayerPrefs.HasKey(LanguageKey)) PlayerPrefs.SetString(LanguageKey, English);
                return PlayerPrefs.GetString(LanguageKey);
            }
            set { PlayerPrefs.SetString(LanguageKey, value); }
        }

        public const string LanguageKey = "Language";
        const string English = "en";
        const string Czech = "cs";

        public override bool Active { get; protected set; } // Czech

        protected override void Start() {
            // Active = (TextManager.textManager.Language == Languages.Česky); // Check current language
            if (!PlayerPrefs.HasKey(LanguageKey)) {
                PlayerPrefs.SetString(LanguageKey, English);
            }


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
            PlayerPrefs.SetString(LanguageKey, (Active) ? Czech : English);
            SpriteSwap();
            base.OnPointerDown(pointerEventData);
        }

    }
}
