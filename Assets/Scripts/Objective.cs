using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Objective : Pickable, IRestartable
{
    [SerializeField] private Animator anim;

    public bool IsCompleted { get; internal set; }

    public override void OnPick()
    {
        IsCompleted = true;
        gameObject.SetActive(false);

        anim.SetTrigger("ObjectiveFound");
    }

    public override void Restart()
    {
        IsCompleted = false;
        base.Restart();
    }
}
