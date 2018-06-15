using Randolph.Interactable;
using Randolph.Levels;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//TODO: Moonstone is used to extract seed from the nut
public class Moonstone : Interactable, IRestartable
{
    [SerializeField] GameObject spawnPoint;
    [SerializeField] InventoryItem seedPrefab;

    public void Cut(GameObject target)
    {
        Debug.Log("Cutting");
        if (target.GetComponent<Pit>() != null)
        {
            Destroy(target);
            Instantiate(seedPrefab, spawnPoint.transform.position, Quaternion.identity);
        }
    }
}
