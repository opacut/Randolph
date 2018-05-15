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

            if (!PlayerPrefs.HasKey(LevelManager.LevelKey) || PlayerPrefs.GetInt(LevelManager.LevelKey) <= 1) {
                // No saved level or the first level
                thisButton.interactable = false;
                buttonText.color /= 1.5f;
            } else {
                int levelToContinueFrom = PlayerPrefs.GetInt(LevelManager.LevelKey);
                thisButton.onClick.AddListener(delegate { SceneManager.LoadScene(levelToContinueFrom); });
            }
        }
    }

}