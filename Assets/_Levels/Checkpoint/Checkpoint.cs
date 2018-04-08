using System.Collections.Generic;
using Randolph.Interactable;
using Randolph.UI;
using UnityEngine;

namespace Randolph.Levels {
    [RequireComponent(typeof(Collider2D))]
    [AddComponentMenu("Randolph/Levels/Checkpoint", 30)]
    public class Checkpoint : MonoBehaviour {

        [Help("An area represents all objects which will be reverted back to their initial states after a restart.")]
        [SerializeField] Area area;

        Inventory inventory;
        CheckpointContainer container;
        readonly List<IRestartable> restartables = new List<IRestartable>();
        List<InventoryItem> inventoryState = new List<InventoryItem>();

        void Awake() {
            inventory = FindObjectOfType<Inventory>();
            container = transform.parent?.GetComponent<CheckpointContainer>();

            if (container == null) {
                Debug.LogWarning("Checkpoints should be made children of a <b>CheckpointContainter</b>, otherwise they won't work properly.", gameObject);
            }

            Debug.Assert(area != null, "The checkpoint isn't linked to any area of the level – therefore is useless.", gameObject);
        }

        void OnTriggerEnter2D(Collider2D other) {
            if (other.tag == "Player") {
                container.CheckpointReached(this);
                SaveState();
            }
        }

        void SaveState() {
            restartables.Clear();
            restartables.AddRange(area.transform.GetComponentsInChildren<IRestartable>());

            inventoryState = inventory.Items;
        }

        public void RestoreState() {
            inventory.Items = inventoryState;

            foreach (IRestartable restartable in restartables) {
                restartable.Restart();
            }
        }

    }
}