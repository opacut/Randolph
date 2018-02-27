using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flytrap : MonoBehaviour, IRestartable
{
    public bool Active;
    public Sprite Sleeping;
    public Sprite Squashed;
    public Sprite Alive;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (Active && (other.tag == "Player"))
        {
            Deactivate();
            other.gameObject.GetComponent<PlayerController>().Kill(1);
        }
    }

    public void Deactivate()
    {
        gameObject.GetComponent<SpriteRenderer>().sprite = Sleeping;
        Active = false;
    }

    public void Kill()
    {
        gameObject.GetComponent<SpriteRenderer>().sprite = Squashed;
        Active = false;
    }

    public void Restart()
    {
        Active = true;
        gameObject.GetComponent<SpriteRenderer>().sprite = Alive;
        
    }
}
