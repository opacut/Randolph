using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Randolph.Levels;

namespace Randolph.UI {
    [RequireComponent(typeof(Button))]
    public class ContinueButton : MonoBehaviour {

        Button thisButton;
        Text buttonText;

        void Start() {
            thisButton = GetComponent<Button>();
            buttonText = gameObject.GetComponentInChildren<Text>();

            bool disabled = PlayerPrefs.GetInt(LevelManager.LevelKey, 0) <= 1 && 
                            PlayerPrefs.GetInt(CheckpointContainer.CheckpointKey, 0) <= 0;

            if (disabled) {
                // No saved level or the first level
                thisButton.interactable = false;
                buttonText.color /= 1.5f;
            } else {                
                thisButton.onClick.AddListener(delegate { LevelManager.levelManager.Continue(); });
            }
        }
    }

}