using System.Collections;
using System.Collections.Generic;
using Randolph.Characters;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class SpikeTrap : MonoBehaviour
{
    public bool On;
    public Sprite Active;
    public Sprite Disabled;

    SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = On ? Active : Disabled;
    }

    public void Toggle()
    {
        On = !On;
        spriteRenderer.sprite = On ? Active : Disabled;
    }

    public void OnTriggerStay2D(Collider2D other)
    {
        if (On&&(other.tag == "Player"))
        {
            other.GetComponent<PlayerController>().Kill();
        }
    }
}
