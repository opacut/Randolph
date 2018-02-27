using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Checkpoint : MonoBehaviour {
    public Transform level;

    Inventory inventory;
    LevelManager levelManager;
    readonly List<IRestartable> restartables = new List<IRestartable>();
    List<InventoryItem> inventoryState = new List<InventoryItem>();

    void Awake() {
        inventory = FindObjectOfType<Inventory>();
        levelManager = FindObjectOfType<LevelManager>();
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Player") {
            levelManager.CheckpointReached(this);
            SaveState();
        }
    }

    void SaveState() {
        restartables.Clear();
        restartables.AddRange(level.GetComponentsInChildren<IRestartable>());

        inventoryState = inventory.Items;
    }

    public void RestoreState() {
        inventory.Items = inventoryState;

        foreach (IRestartable restartable in restartables) {
            restartable.Restart();
        }
    }
}
