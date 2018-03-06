using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {

    public static LevelManager levelManager;
    /// <summary>The <see cref="PlayerPrefs"/> key containing the number of the last level with a player in it.</summary>
    public const string levelKey = "LastLevel";

    [SerializeField] PlayerController player;
    [SerializeField] List<Checkpoint> checkpoints = new List<Checkpoint>();

    Checkpoint reached;

    void Awake() {
        //! Singleton pattern (pass Level Manager between levels; destroy excess ones)
        DontDestroyOnLoad(this);

        if (FindObjectsOfType(GetType()).Length > 1) {
            Destroy(gameObject);
        } else levelManager = this;

        //! Each level
        SceneManager.sceneLoaded += InitializeLevel;
    }

    void InitializeLevel(Scene scene, LoadSceneMode loadSceneMode) {
        if (!player) player = FindObjectOfType<PlayerController>();
        if (!player) Debug.LogWarning($"There is no player in scene <b>{SceneManager.GetActiveScene().name}</b>.");
        else {
            // TODO: Loading checkpoints
            Debug.Assert(checkpoints.Any());
            reached = checkpoints.First();

            player.transform.position = reached.transform.position;
            PlayerPrefs.SetInt(levelKey, SceneManager.GetActiveScene().buildIndex);
        }
    }

    public void CheckpointReached(Checkpoint checkpoint) {
        reached = (IsCheckpointVisited(checkpoint)) ? reached : checkpoint;
    }

    bool IsCheckpointVisited(Checkpoint checkpoint) {
        return reached && checkpoints.IndexOf(checkpoint) < checkpoints.IndexOf(reached);
    }

    public void RestartLevel() {
        reached.RestoreState();
        player.gameObject.SetActive(true);
        player.transform.position = reached.transform.position;
    }

    public static void ReloadLevel() {
        //? Back to checkpoint
        int currentLevelIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentLevelIndex);
    }

    public static void LoadNextLevel() {
        int nextLevelIndex = SceneManager.GetActiveScene().buildIndex + 1;

        if (SceneManager.sceneCountInBuildSettings > nextLevelIndex) {
            SceneManager.LoadScene(nextLevelIndex);
            // TODO: Pass inventory state between levels
        } else {
            string s = (SceneManager.sceneCountInBuildSettings > 1) ? "s" : "";
            Debug.LogError($"Trying to load <color=orange>scene #{nextLevelIndex + 1}</color>, but there's only {SceneManager.sceneCount} scene{s} in total.");
        }
    }

    public void LoadLevel(string level) {
        SceneManager.LoadScene(level);
        // + additional logic, e.g. PlayerPrefs
    }
}
