using System.Collections.Generic;
using Randolph.Interactable;
using UnityEngine;

//TODO: Moonstone is used to extract seed from the nut
public class Moonstone : Interactable {
    private readonly List<GameObject> seeds = new List<GameObject>();

    [SerializeField]
    private InventoryItem seedPrefab;

    [SerializeField]
    private GameObject spawnPoint;

    public void Cut(GameObject target) {
        Debug.Log("Cutting");
        if (target.GetComponent<Pit>() != null) {
            Destroy(target);
            var newSeed = Instantiate(seedPrefab, spawnPoint.transform.position, Quaternion.identity).gameObject;
            newSeed.transform.parent = gameObject.transform.parent;
            seeds.Add(newSeed);
        }
    }

    public override void Restart() {
        base.Restart();
        seeds.ForEach(y => Destroy(y));
    }
}
