using UnityEngine;
using UnityEngine.SceneManagement;

using Randolph.Characters;

namespace Randolph.Levels
{
    [AddComponentMenu("Randolph/Levels/Level Manager", 10)]
    public class LevelManager : MonoBehaviour
    {
        public static LevelManager levelManager;

        /// <summary>The <see cref="PlayerPrefs"/> key containing the number of the last level with a player in it.</summary>
        public const string levelKey = "LastLevel";

        PlayerController player;
        CheckpointContainer checkpoints;


        void Awake()
        {
            //! Singleton pattern (pass Level Manager between levels; destroy excess ones)
            DontDestroyOnLoad(this);

            if (FindObjectsOfType(GetType()).Length > 1)
            {
                Destroy(gameObject);
            }
            else levelManager = this;

            //! Each level
            SceneManager.sceneLoaded += InitializeLevel;
        }

        void InitializeLevel(Scene scene, LoadSceneMode loadSceneMode)
        {
            player = FindObjectOfType<PlayerController>();
            if (player)
            {
                PlayerPrefs.SetInt(levelKey, SceneManager.GetActiveScene().buildIndex);
                checkpoints = FindObjectOfType<CheckpointContainer>();
            }
        }

        public void ReturnToCheckpoint(float delay = 0.25f)
        {
            StartCoroutine(checkpoints.ReturnToCheckpoint(delay));
        }

        public static void ReloadLevel()
        {
            //? Back to checkpoint
            int currentLevelIndex = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(currentLevelIndex);
        }

        public static void LoadNextLevel()
        {
            int nextLevelIndex = SceneManager.GetActiveScene().buildIndex + 1;

            if (SceneManager.sceneCountInBuildSettings > nextLevelIndex)
            {
                SceneManager.LoadScene(nextLevelIndex);
                // TODO: Pass inventory state between levels
            }
            else
            {
                string s = (SceneManager.sceneCountInBuildSettings > 1) ? "s" : "";
                Debug.LogError($"Trying to load <color=orange>scene #{nextLevelIndex + 1}</color>, but there's only {SceneManager.sceneCount} scene{s} in total.");
            }
        }

        public void LoadLevel(string level)
        {
            SceneManager.LoadScene(level);
            // + additional logic, e.g. PlayerPrefs
        }
    }
}
