using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericRestart : MonoBehaviour, IRestartable
{
    public Vector3 InitialPosition;

    void Awake()
    {
        InitialPosition = gameObject.transform.position;
    }

    public void Restart()
    {
        gameObject.transform.position = InitialPosition;
    }
}
