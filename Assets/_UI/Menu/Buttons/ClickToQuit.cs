using UnityEngine;
using UnityEngine.UI;

public class ClickToQuit : MonoBehaviour {

    Button button;

    void Start() {
        button = GetComponent<Button>();
        button.onClick.AddListener(QuitGame);

        if (!DoesQuittingWork()) {
            gameObject.SetActive(false);
        }
    }

    void QuitGame() {
        Application.Quit();
        button.gameObject.SetActive(false);
    }

    bool DoesQuittingWork() {
        return !(Application.platform == RuntimePlatform.WebGLPlayer || Application.isEditor);
    }

}
