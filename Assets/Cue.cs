using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cue : MonoBehaviour
{
    [SerializeField]
    Text tutorialText;

    [SerializeField]
    string updateText;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            tutorialText.text = updateText;
        }    
    }
}
