using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crows : MonoBehaviour
{
    public bool SecondPosition;
    public bool FinalPosition;
    private Animator anim;
    private Transform tr;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if ((!SecondPosition)&&(collision.gameObject.tag == "Player"))
        {
            anim.SetBool("FirstTouch", true);
            //gameObject.transform.position = new Transform;
            SecondPosition = true;
        }

        else if ((!FinalPosition)&&(collision.gameObject.tag == "Player"))
        {
            anim.SetBool("SecondTouch", true);
            FinalPosition = true;
        }

        else if (collision.gameObject.tag == "Enemy")
        {
            Destroy(gameObject);
        }
    }
    
}
