using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {

    public static LevelManager levelManager;

    [SerializeField] PlayerController player;
    [SerializeField] List<Checkpoint> checkpoints = new List<Checkpoint>();

    private Checkpoint reached;

    /*
     TODO: Singleton pattern

         void Awake() {
            DontDestroyOnLoad(this);

            if (FindObjectsOfType(GetType()).Length > 1) {
                Destroy(gameObject);
            } else levelManager = this;

         }
    */

    private void Awake() {
        //! Singleton pattern (pass Level Manager between levels; destroy excess ones)
        DontDestroyOnLoad(this);

        if (FindObjectsOfType(GetType()).Length > 1) {
            Destroy(gameObject);
        } else levelManager = this;

        //! Each level
        SceneManager.sceneLoaded += InitializeLevel;        
    }

    void Update() {
        if (Input.GetKey("escape")) {
            Application.Quit(); // TODO: Overkill; pause, THEN quit (or quit after holding the key)
        }
    }

    void Start() {
        player.transform.position = reached.transform.position;
    }

    void InitializeLevel(Scene scene, LoadSceneMode loadSceneMode) {
        Debug.Assert(checkpoints.Any());

        if (!player) player = FindObjectOfType<PlayerController>();
        reached = checkpoints.First();
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
}
