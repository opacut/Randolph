using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Randolph.Characters;
using Randolph.Core;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Randolph.Levels {
    [ExecuteInEditMode]
    [AddComponentMenu("Randolph/Levels/Checkpoint Container", 20)]
    public class CheckpointContainer : MonoBehaviour {

        [SerializeField, ReadonlyField] Checkpoint reached;

        [SerializeField, Tooltip("Aligns the player's position to the first checkpoint in the list.")]
        bool alignPlayer = true;

        [SerializeField, ReadonlyField] List<Checkpoint> checkpoints = new List<Checkpoint>();
        public const string CheckpointKey = "ReachedCheckpoint";
        PlayerController player;

        void Awake() {
            LevelManager.OnNewLevel += OnNewLevel;
        }

        /// <summary>Setup the checkpoints and (optionally) move the player to the first one.</summary>        
        async void OnNewLevel(Scene scene, PlayerController playerController) {
            Debug.Assert(FindObjectsOfType(GetType()).Length == 1, "There is always supposed to be only one <b>CheckpointContainer</b> in a level.", gameObject);
            player = playerController;
            RefreshCheckpointList();
            Debug.Assert(checkpoints.Any(), "There are no checkpoints in the container!", gameObject);

            reached = checkpoints.First();
            PlayerPrefs.SetInt(CheckpointKey, checkpoints.IndexOf(reached));
            if (alignPlayer) {
                player.transform.position = reached.transform.position;
                player.transform.AlignToGround();
            }
            await Task.Delay(TimeSpan.FromSeconds(1));
            Constants.Camera.transition.TransitionEnter();
        }

        void Update() {
            if (transform.hasChanged) {
                RefreshCheckpointList();
            }
        }

        void RefreshCheckpointList() {
            checkpoints.Clear();
            GetComponentsInChildren(checkpoints);
        }

        public async void ReturnToCheckpoint(float delay) {
            if (delay > 0) {
                await Task.Delay(TimeSpan.FromSeconds(delay));
            }
            reached.RestoreState();

            Constants.Camera.transition.TransitionExit();
            await Task.Delay(TimeSpan.FromSeconds(Constants.Camera.transition.DurationExit));
            player.transform.position = reached.transform.position;
            Constants.Camera.transition.TransitionEnter();
            await Task.Delay(TimeSpan.FromSeconds(Constants.Camera.transition.DurationEnter));
        }

        public void CheckpointReached(Checkpoint checkpoint) {
            if (!IsCheckpointVisited(checkpoint)) {
                reached = checkpoint;
                PlayerPrefs.SetInt(CheckpointKey, checkpoints.IndexOf(reached));
            }
        }

        public Checkpoint GetNext() {
            return checkpoints.GetNextItem(reached);
        }

        public Checkpoint GetPrevious() {
            return checkpoints.GetPreviousItem(reached);
        }

        public void SetReached(Checkpoint checkpoint) {
            if (checkpoint == null) return;
            else reached = checkpoint;
        }

        bool IsCheckpointVisited(Checkpoint checkpoint) {
            return reached && checkpoints.IndexOf(checkpoint) <= checkpoints.IndexOf(reached);
        }

        void OnDrawGizmosSelected() {
            if (checkpoints.Count < 1) return;
            Gizmos.color = Color.green;

            Vector3 startPoint = checkpoints[0].transform.position;
            for (int i = 1; i < checkpoints.Count; i++) {
                Vector3 endPoint = checkpoints[i].transform.position;
                Gizmos.DrawLine(startPoint, endPoint);
                startPoint = endPoint;
            }

            Vector3? levelExit = FindObjectOfType<LevelExit>()?.transform.position;
            if (levelExit.HasValue) {
                Gizmos.DrawLine(startPoint, levelExit.Value);
            }
        }

    }
}