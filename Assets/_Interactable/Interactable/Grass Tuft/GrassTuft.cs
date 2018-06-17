using Assets._Interactable;
using Randolph.Interactable;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassTuft : Interactable, ISlashable
{
    [SerializeField]
    public Transform grass;
    [SerializeField]
    private GameObject spawnPoint;
    [SerializeField]
    private Sprite cutGrass;
    private Sprite original;

    private SpriteRenderer spriteRenderer;
    private bool hasGrass = true;

    public void Awake()
    {
        base.Awake();
        spriteRenderer = GetComponent<SpriteRenderer>();
        original = spriteRenderer.sprite;
    }

    public override void Restart()
    {
        base.Restart();
        hasGrass = true;
        spriteRenderer.sprite = original;
    }

    public override void Interact()
    {
        Debug.Log("Tuft clicked");
    }

    public void Slash()
    {
        if (hasGrass)
        {
            hasGrass = false;
            spriteRenderer.sprite = cutGrass;
            Instantiate(grass, spawnPoint.transform.position, Quaternion.identity);
        }        
    }

}
