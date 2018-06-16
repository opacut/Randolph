using Randolph.Interactable;
using Randolph.Levels;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitInvisibleSwitch : MonoBehaviour, IRestartable
{
    public bool Activated = false;
    public GameObject fruitHolder;
    private Collider2D fruitHolderCollider;

    public void Restart()
    {
        ToggleActive(true);
    }

    public void ToggleActive(bool value)
    {
        fruitHolderCollider.enabled = value;
    }

    void Start ()
    {
        fruitHolderCollider = fruitHolder.GetComponent<Collider2D>();
	}

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Boulder>())
        {
            ToggleActive(false);
        }
    }
}
