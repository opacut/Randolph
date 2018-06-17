using Randolph.Core;
using Randolph.Levels;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour {

    public static bool IsPaused { get; private set; }
    const string PauseKey = "Pause";

    public delegate void GamePaused();

    public static event GamePaused OnGamePaused;

    [SerializeField] GameObject pauseMenuUI;
    float oldTimeScale = 1f;

    [SerializeField] [Scene] string menuScene;

    void Awake() {
        if (pauseMenuUI.activeSelf && IsPaused == false) {
            // Accidentaly open menu
            pauseMenuUI.SetActive(false);
        }
    }

    void Update() {
        if (Input.GetButtonDown(PauseKey)) {
            if (!IsPaused) PauseGame();
            else ResumeGame();
        }
    }

    public void ResumeGame() {
        pauseMenuUI.SetActive(false);
        Time.timeScale = oldTimeScale;
        AudioPlayer.ResumeGlobalVolume();

        IsPaused = false;
    }

    void PauseGame() {
        OnGamePaused?.Invoke();

        pauseMenuUI.SetActive(true);
        oldTimeScale = Time.timeScale;
        Time.timeScale = 0f;
        AudioPlayer.PauseGlobalVolume();

        IsPaused = true;
    }

    public void LoadMenu() {
        ResumeGame();
        SceneManager.LoadScene(menuScene);
    }

    public void QuitGame() {
        if (Application.isEditor) Debug.Log("Quitting game…");
        Application.Quit();
    }

    public void OpenSettings() {
        Debug.Log("Opening settings…");
    }

}
