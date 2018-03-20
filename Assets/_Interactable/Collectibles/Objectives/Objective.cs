using UnityEngine;

public class Objective : Pickable
{
    [SerializeField] Animator animator;

    public bool IsCompleted { get; internal set; }

    public override void OnPick()
    {
        IsCompleted = true;
        gameObject.SetActive(false);

        animator.SetTrigger("ObjectiveFound");
    }

    public override void Restart()
    {
        IsCompleted = false;
        base.Restart();
    }
}
