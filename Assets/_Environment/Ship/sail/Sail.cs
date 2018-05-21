using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sail : MonoBehaviour
{
    private Animator animator;
    [SerializeField]
    private float speed = 20f;
    [SerializeField]
    private Vector2 destination = new Vector2(-100f, 50f);

    
	void Start ()
    {
        animator = GetComponent<Animator>();
	}
	
	void Update ()
    {
        if (animator.GetBool("CutOff"))
        {
            IncrementPosition();
        }
	}

    void IncrementPosition()
    {
        float delta = speed * Time.deltaTime;
        
        transform.position = Vector2.MoveTowards(transform.position, destination, delta);
        transform.Rotate(Vector3.forward, speed * delta*delta);
    }

    public void Slash()
    {
        animator.SetBool("CutOff", true);
    }

}
