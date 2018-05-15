using UnityEngine;
using UnityEngine.UI;

namespace Randolph.UI {
    public class ClickToQuit : MonoBehaviour {

        Button button;

        void Start() {
            button = GetComponent<Button>();
            button.onClick.AddListener(QuitGame);

            if (!CanQuitGame()) {
                gameObject.SetActive(false);
            }
        }

        void QuitGame() {
            Application.Quit();
            button.gameObject.SetActive(false);
        }

        /// <summary>Checks whether pressing a quit button would make sense.</summary>
        /// <returns>True if pressing the quit button quits the application.</returns>
        bool CanQuitGame() {
            return !(Application.platform == RuntimePlatform.WebGLPlayer || Application.isEditor);
        }

    }
}
