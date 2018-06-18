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

    private List<GameObject> seeds = new List<GameObject>();

    public void Cut(GameObject target)
    {
        Debug.Log("Cutting");
        if (target.GetComponent<Pit>() != null)
        {
            Destroy(target);
            GameObject newSeed = Instantiate(seedPrefab, spawnPoint.transform.position, Quaternion.identity).gameObject;
            newSeed.transform.parent = gameObject.transform.parent;
            seeds.Add(newSeed);
        }
    }

    public override void Restart()
    {
        base.Restart();
        seeds.ForEach(y => Destroy(y));
    }
}
