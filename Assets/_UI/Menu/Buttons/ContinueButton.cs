using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ContinueButton : MonoBehaviour {

    Button thisButton;
    Text buttonText;
	
    void Start () {
        thisButton = GetComponent<Button>();
        buttonText = gameObject.GetComponentInChildren<Text>();

        if (!PlayerPrefs.HasKey(LevelManager.levelKey) || PlayerPrefs.GetInt(LevelManager.levelKey) <= 1) {
            // No saved level or the first level
            thisButton.interactable = false;
            buttonText.color /= 1.5f;
        } else {
            int levelToContinueFrom = PlayerPrefs.GetInt(LevelManager.levelKey);
            thisButton.onClick.AddListener(delegate { SceneManager.LoadScene(levelToContinueFrom); });
        }
    }
	
}
