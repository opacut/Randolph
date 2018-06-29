using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Randolph.Core;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Randolph.Levels {
    [ExecuteInEditMode]
    [AddComponentMenu("Randolph/Levels/Checkpoint Container", 20)]
    public class CheckpointContainer : MonoBehaviour {
        public const string CheckpointKey = "ReachedCheckpoint";

        [SerializeField, Tooltip("Aligns the player's position to the first checkpoint in the list.")]
        private bool alignPlayer = true;

        [SerializeField, ReadonlyField]
        private List<Checkpoint> checkpoints = new List<Checkpoint>();

        [SerializeField, ReadonlyField]
        private Checkpoint reached;

        private void Awake() {
            LevelManager.OnNewLevel += OnNewLevel;
        }

        /// <summary>Setup the checkpoints and (optionally) move the player to the first one.</summary>
        private async void OnNewLevel(Scene scene) {
            Debug.Assert(FindObjectsOfType(GetType()).Length == 1, "There is always supposed to be only one <b>CheckpointContainer</b> in a level.", gameObject);
            RefreshCheckpointList();
            Debug.Assert(checkpoints.Any(), "There are no checkpoints in the container!", gameObject);

            if (reached == null) {
                // Next level, no continue
                reached = checkpoints.First();
                reached.SaveState();

                PlayerPrefs.SetInt(CheckpointKey, checkpoints.IndexOf(reached));
                if (alignPlayer) {
                    Constants.Randolph.transform.position = reached.transform.position;
                    Constants.Randolph.transform.AlignToGround();
                }
            }

            await Task.Delay(TimeSpan.FromSeconds(1));
            Constants.Camera.transition.TransitionEnter();
        }

        private void Update() {
            if (transform.hasChanged) {
                RefreshCheckpointList();
            }
        }

        private void RefreshCheckpointList() {
            checkpoints.Clear();
            GetComponentsInChildren(checkpoints);
        }

        public async void ReturnToCheckpoint(float delay) {
            Constants.Randolph.Freeze();
            if (delay > 0) {
                await Task.Delay(TimeSpan.FromSeconds(delay));
            }

            Constants.Camera.transition.TransitionExit();
            await Task.Delay(TimeSpan.FromSeconds(Constants.Camera.transition.DurationExit));

            Constants.Randolph.transform.position = reached.transform.position;
            Constants.Randolph.transform.AlignToGround();
            Constants.Randolph.Killable = true;
            reached.RestoreState();

            Constants.Camera.rooms.EnterRoom(reached.Area.MatchingCameraRoom.ID, false);
            Constants.Camera.transition.TransitionEnter();
            await Task.Delay(TimeSpan.FromSeconds(Constants.Camera.transition.DurationEnter));
            Constants.Randolph.UnFreeze();
        }

        public void CheckpointReached(Checkpoint checkpoint) {
            reached = checkpoint;
            reached.SaveState();
            PlayerPrefs.SetInt(CheckpointKey, checkpoints.IndexOf(reached));
        }

        public Checkpoint GetNext() => checkpoints.GetNextItem(reached);

        public Checkpoint GetPrevious() => checkpoints.GetPreviousItem(reached);

        public void SetReached(Checkpoint checkpoint, bool movePlayer = false) {
            if (checkpoint == null) {
                return;
            }
            if (movePlayer) {
                Constants.Randolph.transform.position = checkpoint.transform.position;
                if (alignPlayer) {
                    Constants.Randolph.transform.AlignToGround();
                }
            }
            CheckpointReached(checkpoint);
        }

        public void SetReached(int checkpointIndex, bool movePlayer = false) {
            var checkpoint = checkpoints[checkpointIndex];
            SetReached(checkpoint, movePlayer);
        }

        public bool IsCheckpointVisited(Checkpoint checkpoint) => reached && checkpoints.IndexOf(checkpoint) <= checkpoints.IndexOf(reached);

        private void OnDrawGizmosSelected() {
            if (checkpoints.Count < 1) {
                return;
            }
            Gizmos.color = Color.green;

            var startPoint = checkpoints[0].transform.position;
            for (var i = 1; i < checkpoints.Count; i++) {
                var endPoint = checkpoints[i].transform.position;
                Gizmos.DrawLine(startPoint, endPoint);
                startPoint = endPoint;
            }

            var levelExit = FindObjectOfType<LevelExit>()?.transform.position;
            if (levelExit.HasValue) {
                Gizmos.DrawLine(startPoint, levelExit.Value);
            }
        }

        private void OnDestroy() {
            LevelManager.OnNewLevel -= OnNewLevel;
        }
    }
}
