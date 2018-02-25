using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public abstract class Pickable : MonoBehaviour, IRestartable
{
    private Vector3 initialPosition;

    private void Awake()
    {
        initialPosition = transform.position;
    }

    public virtual void Restart()
    {
        gameObject.transform.position = initialPosition;
        gameObject.SetActive(true);
    }

    public abstract void OnPick();
}
