using Assets._Interactable;
using Randolph.Interactable;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shrub : Interactable, ISlashable
{
    [SerializeField]
    public Transform twig;
    [SerializeField]
    private GameObject spawnPoint;

    public override void Interact()
    {
        Debug.Log("Shrubbery clicked");
    }

    public void Slash()
    {
        Instantiate(twig, spawnPoint.transform.position, Quaternion.identity);
    }
}
