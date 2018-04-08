using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Randolph.Characters;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Randolph.Levels {
    [ExecuteInEditMode]
    [AddComponentMenu("Randolph/Levels/Checkpoint Container", 20)]
    public class CheckpointContainer : MonoBehaviour {
        [SerializeField, ReadonlyField] List<Checkpoint> checkpoints = new List<Checkpoint>();
        public const string checkpointKey = "ReachedCheckpointIndex";

        PlayerController player;
        Checkpoint reached;

        void Awake() {
            Debug.Assert(FindObjectsOfType(GetType()).Length == 1, "There is always supposed to be only one <b>CheckpointContainer</b> in a level.", gameObject);

            player = FindObjectOfType<PlayerController>();
            if (!player) Debug.LogError($"There is no player in scene <b>{SceneManager.GetActiveScene().name}</b>.");
            else {
                Debug.Assert(checkpoints.Any(), "There are no checkpoints in the container!", gameObject);
                reached = checkpoints.First();
                PlayerPrefs.SetInt(checkpointKey, checkpoints.IndexOf(reached));
                player.transform.position = reached.transform.position;
            }
        }

        void Update() {
            if (transform.hasChanged) {
                checkpoints.Clear();
                foreach (Transform child in transform) {
                    var checkpoint = child.GetComponent<Checkpoint>();
                    if (checkpoint) checkpoints.Add(checkpoint);
                }
            }
        }

        public IEnumerator ReturnToCheckpoint(float delay) {
            yield return new WaitForSeconds(delay);
            ReturnToCheckpoint();
        }

        public void ReturnToCheckpoint() {
            reached.RestoreState();
            player.gameObject.SetActive(true);
            player.transform.position = reached.transform.position;
        }

        public void CheckpointReached(Checkpoint checkpoint) {
            if (!IsCheckpointVisited(checkpoint)) {
                reached = checkpoint;
                PlayerPrefs.SetInt(checkpointKey, checkpoints.IndexOf(reached));
            }
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