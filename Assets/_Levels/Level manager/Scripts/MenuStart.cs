using UnityEngine;

public class MenuStart : MonoBehaviour {
    [SerializeField, Range(0, 3)] float holdKeyToQuitTime = 1.5f;
    float quitTimer = 0f;   

    void Update() {        
        if (Input.GetButton("Cancel")) {
            quitTimer += Time.deltaTime;

            if (quitTimer >= holdKeyToQuitTime) {
                quitTimer = 0f;
                Debug.Log("Quitting");
                Application.Quit();
            }
        } else if (Input.anyKeyDown) {
            LevelManager.LoadNextLevel();
        } else {            
            quitTimer = 0f;
        }
    }
}
