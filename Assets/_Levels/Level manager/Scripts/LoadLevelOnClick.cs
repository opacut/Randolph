using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Randolph.UI {

    /// <summary>Handles buttons which pass between scenes.</summary>
    [RequireComponent(typeof(Button))]
    public class LoadLevelOnClick : MonoBehaviour {

        Button thisButton;
        [SerializeField] public string sceneToLoad;

        void Start() {
            thisButton = GetComponent<Button>();
            if (sceneToLoad != "") {
                thisButton.onClick.AddListener(delegate { SceneManager.LoadScene(sceneToLoad); });
            }
        }
    }
}