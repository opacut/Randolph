using Randolph.Levels;
using UnityEngine;
using UnityEngine.UI;

namespace Randolph.UI {
    public class ClickToQuit : MonoBehaviour {

        Button button;

        void Start() {
            button = GetComponent<Button>();
            button.onClick.AddListener(QuitGame);

            if (!LevelManager.CanQuitGame()) {
                gameObject.SetActive(false);
            }
        }

        void QuitGame() {
            Application.Quit();
            button.gameObject.SetActive(false);
        }

    }
}