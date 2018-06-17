using System;
using System.Collections.Generic;
using System.Linq;
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

    Animator animator;
    const string PauseTrigger = "Pause";
    //const string UnpauseTrigger = "Unpause";
    [HideInInspector] public int toolbarIdx;

    //! Pause
    public GameObject pauseMenuControls;
    public GameObject pauseUI;
    [Scene] public string menuScene;
    float oldTimeScale = 1f;

    //! Settings
    public GameObject settingsUI;

    //! Map
    public GameObject mapUI;

    void Awake() {
        Reset();
        if (pauseMenuControls.activeSelf) {
            // Accidentaly open menu
            pauseMenuControls.SetActive(false);
        }

        if (!LevelManager.CanQuitGame()) {
            //! Disable quit button if it's pointless
            GetButtonWithClickMethod(QuitGame)?.gameObject.SetActive(false);
        }

        animator = GetComponent<Animator>();
    }

    void Update() {
        if (Input.GetButtonDown(PauseKey)) {
            if (!IsPaused) PauseGame();
            else ResumeGame();
        }
    }

    public void ResumeGame() {
        if (settingsUI.activeSelf) CloseSettings();
        pauseMenuControls.SetActive(false);

        Time.timeScale = oldTimeScale;
        AudioPlayer.ResumeGlobalVolume();
        //animator.SetTrigger(UnpauseTrigger);

        IsPaused = false;
    }

    public void PauseGame() {
        OnGamePaused?.Invoke();

        pauseMenuControls.SetActive(true);

        oldTimeScale = Time.timeScale;
        Time.timeScale = 0f;
        AudioPlayer.PauseGlobalVolume();
        animator.SetTrigger(PauseTrigger);

        IsPaused = true;
    }

    public void LoadMenu() {
        ResumeGame();
        SceneManager.LoadScene(menuScene);
    }

    Button GetButtonWithClickMethod(Action action) {
        Button quitButton = pauseUI.GetComponentsInChildren<Button>().FirstOrDefault(button => {
            int count = button.onClick.GetPersistentEventCount();
            for (int i = 0; i < count; i++) {
                if (button.onClick.GetPersistentMethodName(i) == nameof(action)) return true;
            }
            return false;
        });
        return quitButton;
    }

    public void QuitGame() {
        if (Application.isEditor) Debug.Log("Quitting game…");
        Application.Quit();
    }

    // TODO: ↓ Split to other classes

    public void OpenSettings() {
        pauseUI.SetActive(false);
        settingsUI.SetActive(true);
    }

    public void CloseSettings() {
        pauseUI.SetActive(true);
        settingsUI.SetActive(false);
    }

    public void OpenMap() {
        pauseUI.SetActive(false);
        mapUI.SetActive(true);
    }

    public void CloseMap() {
        pauseUI.SetActive(true);
        mapUI.SetActive(false);
    }

    /// <summary>Called when adding components for the first time or clicking 'Reset' context menu.</summary>
    void Reset() {
        if (toolbarIdx == 1 && pauseUI && settingsUI) CloseSettings();
        else if (toolbarIdx == 2 && pauseUI && mapUI) CloseMap();
        toolbarIdx = 0;
    }

}
