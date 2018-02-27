using UnityEngine;
using UnityEngine.SceneManagement;

public class Restart : MonoBehaviour {
    void Update() {
        if (Input.GetButtonDown("Restart")) {
            int currentLevelIndex = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(currentLevelIndex);
        }
    }

    //? Back to checkpoint
}
