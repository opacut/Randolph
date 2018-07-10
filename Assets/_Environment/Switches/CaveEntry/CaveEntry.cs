using Randolph.Characters;
using Randolph.Core;
using Randolph.Interactable;
using Randolph.Levels;
using Randolph.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaveEntry : MonoBehaviour, IRestartable
{
    public Inventory inventory;
    [TextArea]
    public string description;

    public void Restart()
    {
        gameObject.SetActive(true);
    }

    public void SaveState()
    {
        return;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if ((collision.CompareTag(Constants.Tag.Player)))
        {
            //levelManager.GetComponent<Inventory>();
            if (inventory.Contains<Torchburning>())
            {
                gameObject.SetActive(false);
            }
            else
            {
                collision.gameObject.GetComponent<PlayerController>().ShowDescriptionBubble(description, 1.5f);
            }
        }
    }
}
