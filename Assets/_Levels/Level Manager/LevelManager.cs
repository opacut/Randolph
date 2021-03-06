using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Randolph.Characters;
using Randolph.Core;
using Randolph.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Randolph.Levels {
    [AddComponentMenu("Randolph/Levels/Level Manager", 10)]
    public class LevelManager : MonoBehaviour {

        public static LevelManager levelManager;

        /// <summary>The <see cref="PlayerPrefs"/> key containing the number of the last level with a player in it.</summary>
        public const string LevelKey = "Level";
        
        public CheckpointContainer Checkpoints { get; private set; }
        public Inventory Inventory { get; private set; }

        public Area[] Areas {
            get {
                if (levelAreas == null || levelAreas.Length == 0) levelAreas = GetLevelAreas();
                return levelAreas;
            }
            private set { levelAreas = value; }
        }

        Area[] levelAreas;

        bool ContinueMode { get; set; }

        public delegate void NewLevel(Scene scene);

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
            if (Constants.Randolph) {
                PlayerPrefs.SetInt(LevelKey, SceneManager.GetActiveScene().buildIndex);
                Checkpoints = FindObjectOfType<CheckpointContainer>();                
                Areas = GetLevelAreas();
                Inventory = FindObjectOfType<Inventory>();
               
                if (ContinueMode) {
                    // Restore state of all
                    RestoreFromContinue();
                    ContinueMode = false;
                }

                OnNewLevel?.Invoke(scene);
            }
        }

        public void ReturnToCheckpoint(float delay = 0.25f) {
            Checkpoints.ReturnToCheckpoint(delay);
        }

        public static void ReloadLevel() {
            //? After "reset" button � back to checkpoint?
            int currentLevelIndex = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(currentLevelIndex);
        }

        public static async void LoadNextLevel() {
            int nextLevelIndex = SceneManager.GetActiveScene().buildIndex + 1;

            if (SceneManager.sceneCountInBuildSettings > nextLevelIndex) {
                Constants.Camera.transition.TransitionExit();
                await Task.Delay(TimeSpan.FromSeconds(Constants.Camera.transition.DurationExit));
                SceneManager.LoadSceneAsync(nextLevelIndex);
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

        Area[] GetLevelAreas() {
            // Get only properly named areas (may contain more linking to the same room)
            Area[] areas = FindObjectsOfType<Area>().Where(area => area.Id != Area.InvalidId).OrderBy(area => area.Id).ToArray();
            List<int> duplicates = (from area in areas group area by area.Id into idGroups where idGroups.Count() > 1 select idGroups.Key).ToList();
            if (duplicates.Count > 0) Debug.LogWarning($"Some areas ({duplicates[0]}) reference the same Camera Room.");
            return areas;
        }

        public void Continue() {
            ContinueMode = true;
            int level = PlayerPrefs.GetInt(LevelKey, defaultValue: 1);
            SceneManager.LoadScene(level);
        }

        public void RestoreFromContinue() {
            Inventory.RestorStateFromPrefs();
            int checkpoint = PlayerPrefs.GetInt(CheckpointContainer.CheckpointKey, defaultValue: 0);
            Checkpoints.SetReached(checkpoint, true);
        }

        /// <summary>Checks whether pressing a quit button would make sense.</summary>
        /// <returns>True if pressing the quit button quits the application.</returns>
        public static bool CanQuitGame() {
            return !(Application.platform == RuntimePlatform.WebGLPlayer || Application.isEditor);
        }

    }
}
