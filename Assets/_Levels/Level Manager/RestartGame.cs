using UnityEngine;
using UnityEngine.SceneManagement;

namespace Randolph.Levels {
    public class RestartGame : MonoBehaviour {

        void Update() {
            if (Input.GetButtonDown("Restart")) {
                if (SceneManager.sceneCountInBuildSettings > 1) SceneManager.LoadScene(1);
                else if (SceneManager.sceneCountInBuildSettings == 1) SceneManager.LoadScene(0);
            }
        }
    }

}