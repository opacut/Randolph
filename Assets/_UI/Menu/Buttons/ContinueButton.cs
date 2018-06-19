using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Randolph.Levels;

namespace Randolph.UI {
    [RequireComponent(typeof(Button))]
    public class ContinueButton : MonoBehaviour {

        // [Scene] public string saveFromLevel;
        Button thisButton;
        Text buttonText;

        static bool Disabled {
            get {
                // Scene scene = SceneManager.GetSceneByName(saveFromLevel);
                int lastLevel = PlayerPrefs.GetInt(LevelManager.LevelKey, 0);
                int minLevelToSave = 2;

                return  lastLevel < minLevelToSave;
            }
        }

        Color originalColor;
        Color dimColor;

        void Start() {
            thisButton = GetComponent<Button>();
            buttonText = gameObject.GetComponentInChildren<Text>();
            originalColor = buttonText.color;
            dimColor = originalColor / 1.5f;

            thisButton.onClick.AddListener(delegate { LevelManager.levelManager.Continue(); });
        }

        void LateUpdate() {
            if (Disabled) {
                // No saved level or the first level
                thisButton.interactable = false;
                buttonText.color = dimColor;
            } else {
                thisButton.interactable = true;
                buttonText.color = originalColor;
            }
        }

    }
}
