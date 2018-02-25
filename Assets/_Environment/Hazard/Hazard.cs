using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hazard : MonoBehaviour, IRestartable
{
    public Vector3 InitialPosition;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.GetComponent<PlayerController>().Kill();
        }
    }


    void Awake()
    {
        InitialPosition = gameObject.transform.position;
    }

    public void Restart()
    {
        gameObject.transform.position = InitialPosition;
    }
}
