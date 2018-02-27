using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Squasher : MonoBehaviour, IRestartable
{
    public Vector3 InitialPosition;
    public Quaternion InitialRotation;

    void Awake()
    {
        InitialPosition = gameObject.transform.position;
        InitialRotation = gameObject.transform.rotation;
    }

    public void Restart()
    {
        gameObject.transform.position = InitialPosition;
        gameObject.transform.rotation = InitialRotation;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy")
        {
            other.gameObject.GetComponent<Flytrap>().Kill();
        }
    }
}
