using Assets._Interactable;
using Randolph.Interactable;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reed : Interactable, ISlashable
{
    [SerializeField]
    public Transform grass;

    public override void OnInteract()
    {
        Debug.Log("Tuft clicked");
    }

    public void Slash()
    {
        Instantiate(grass, transform.position, Quaternion.identity);
    }
}
