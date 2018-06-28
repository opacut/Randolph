using System.Collections.Generic;
using Randolph.Core;
using Randolph.Interactable;
using Randolph.UI;
using UnityEngine;

namespace Randolph.Levels {
    [RequireComponent(typeof(Collider2D))]
    [AddComponentMenu("Randolph/Levels/Checkpoint", 30)]
    public class Checkpoint : MonoBehaviour {


        [Help("An area represents all objects which will be reverted back to their initial states after a restart.")]
        [SerializeField] private Area area;
        public Area Area => area;

        private Inventory inventory;
        private CheckpointContainer container;
        private IEnumerable<IRestartable> restartables => area.transform.GetComponentsInChildren<IRestartable>(true);
        private List<InventoryItem> inventoryState = new List<InventoryItem>();

        void Awake() {
            inventory = FindObjectOfType<Inventory>();
            container = transform.parent.GetComponent<CheckpointContainer>();

            Debug.Assert(container, "Checkpoints should be made children of a <b>CheckpointContainter</b>, otherwise they won't work properly.", gameObject);
            Debug.Assert(area, "The checkpoint isn't linked to any area of the level – therefore is useless.", gameObject);            
        }

        void OnTriggerEnter2D(Collider2D other) {
            if (!other.CompareTag(Constants.Tag.Player) || container.IsCheckpointVisited(this)) {
                return;
            }

            container.CheckpointReached(this);
            SaveState();
        }

        public void SaveState() {
            inventory.SaveStateToPrefs(inventory.Items);
            inventoryState = inventory.Items;

            foreach (var restartable in restartables) {
                restartable.SaveState();
            }
        }

        public void RestoreState() {
            inventory.Items = inventoryState;

            foreach (var restartable in restartables) {
                restartable.Restart();
            }
        }
    }
}