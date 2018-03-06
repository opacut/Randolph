using UnityEngine;

public class HoldKeyToQuit : MonoBehaviour {
    [SerializeField, Range(0, 3)] float holdKeyToQuitTime = 1.5f;
    float quitTimer = 0f;

    void Update() {
        if (Input.GetButton("Cancel")) {
            // TODO: Visual response while holding the button
            quitTimer += Time.deltaTime;

            if (quitTimer >= holdKeyToQuitTime) {
                quitTimer = 0f;
                Debug.Log("Quitting");
                Application.Quit();
            }
        } else {
            quitTimer = 0f;
        }
    }
}
