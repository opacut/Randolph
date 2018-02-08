using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Checkpoint : MonoBehaviour
{
    public Transform level;

    private Inventory inventory;
    private LevelManager levelManager;
    private List<IRestartable> restartables = new List<IRestartable>();
    private List<InventoryItem> inventoryState = new List<InventoryItem>();

    private void Awake()
    {
        inventory = GameObject.Find("Inventory").GetComponent<Inventory>();
        levelManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();
    }

    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            levelManager.CheckpointReached(this);
            SaveState();
        }
    }

    private void SaveState()
    {
        restartables.Clear();
        restartables.AddRange(level.GetComponentsInChildren<IRestartable>());

        inventoryState = inventory.Items;
    }

    public void RestoreState()
    {
        inventory.Items = inventoryState;

        foreach (IRestartable restartable in restartables)
        {
            restartable.Restart();
        }
    }
}
