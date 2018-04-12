using UnityEngine;
using UnityEngine.SceneManagement;
using Randolph.Characters;

namespace Randolph.Levels {
    [AddComponentMenu("Randolph/Levels/Level Manager", 10)]
    public class LevelManager : MonoBehaviour {

        public static LevelManager levelManager;

        /// <summary>The <see cref="PlayerPrefs"/> key containing the number of the last level with a player in it.</summary>
        public const string levelKey = "Level";

        public PlayerController Player { get; private set; }
        public CheckpointContainer Checkpoints { get; private set; }

        public delegate void NewLevel(Scene scene, PlayerController player);
        /// <summary>An event invoked at the start of each level containing a <see cref="PlayerController"/>.</summary>
        public static event NewLevel OnNewLevel;


        void Awake() {
            //! Pass Level Manager between levels; destroy excess ones
            DontDestroyOnLoad(this);

            if (FindObjectsOfType(GetType()).Length > 1) {
                Destroy(gameObject);
            } else levelManager = this;

            SceneManager.sceneLoaded += InitializeLevel;
        }

        void InitializeLevel(Scene scene, LoadSceneMode loadSceneMode) {
            Player = FindObjectOfType<PlayerController>();
            if (Player) {
                PlayerPrefs.SetInt(levelKey, SceneManager.GetActiveScene().buildIndex);
                Checkpoints = FindObjectOfType<CheckpointContainer>();
                OnNewLevel?.Invoke(scene, Player);
            }
        }

        public void ReturnToCheckpoint(float delay = 0.25f) {
            StartCoroutine(Checkpoints.ReturnToCheckpoint(delay));
        }

        public static void ReloadLevel() {
            //? After "reset" button – back to checkpoint?
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
}
