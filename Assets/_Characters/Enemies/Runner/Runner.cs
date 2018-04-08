using Randolph.Characters;
using Randolph.Interactable;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Randolph.Levels;

public class Runner : MonoBehaviour, IEnemy
{
    public float speed;
    public int goingRight;
    public Sprite dead;
    public Animator animator;

    bool alive = true;

    Vector3 initialPosition;
    Quaternion initialRotation;

    SpriteRenderer spriteRenderer;

    void Awake()
    {
        initialPosition = gameObject.transform.position;
        initialRotation = gameObject.transform.rotation;
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    void Start ()
    {
        goingRight = 1;
	}
	
	void Update ()
    {
        transform.Translate(goingRight * Vector2.right * speed * Time.deltaTime);
	}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Collider2D collider = collision.collider;
        Vector3 contactPoint = collision.contacts[0].point;
        Vector3 center = collider.bounds.center;

        if (alive)
        {
            if (collider.tag == "Player")
            {
                collider.gameObject.GetComponent<PlayerController>().Kill(1);
            }
            else if ((contactPoint.y > center.y) && (collider.gameObject.GetComponent<Squasher>() != null))
            {
                Kill();
            }
            else if (contactPoint.y > center.y - 5)
            {
                Turn();
            }
        }
    }

    void Turn()
    {
        Debug.Log("Turning");
        goingRight *= -1;

        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    public void Kill()
    {
        alive = false;
        animator.SetBool("Alive", false);
        spriteRenderer.sprite = dead;
        speed = 0;
    }

    public void Restart()
    {
        gameObject.transform.position = initialPosition;
        gameObject.transform.rotation = initialRotation;
        alive = true;
        animator.SetBool("Alive", true);
    }
}
