using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour {
    [SerializeField] private float skin = 1.3f;
    [SerializeField] private float climbingSpeed = 6;
    [SerializeField] private float movementSpeed = 6;
    [SerializeField] private float jumpForce = 800;
    [SerializeField] private float fallForce = 50;
    [SerializeField] private LayerMask groundLayer;

    public LevelManager levelManager;

    private const string ladderTag = "Ladder";
    private const string pickableTag = "Pickable";

    private Animator anim;
    private Rigidbody2D rb;
    private float gravity = 0;
    private bool jump = false;
    private bool climbing = false;
    private bool onGround = false;
    private Ladder onLadder = null;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        gravity = rb.gravityScale;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == ladderTag)
        {
            onLadder = collision.GetComponent<Ladder>();
        }

        if (collision.tag == pickableTag)
        {
            Debug.Assert(collision.GetComponent<Pickable>());
            collision.GetComponent<Pickable>().OnPick();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == ladderTag)
        {
            IgnorePlatformCollision(false);
            onLadder = null;
        }
    }

    private void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            jump = true;
        }
    }

    private void FixedUpdate()
    {
        GroundCheck();

        float vertical = Input.GetAxisRaw("Vertical");
        float horizontal = Input.GetAxisRaw("Horizontal");

        Moving(horizontal);
        Jumping(vertical);
        Climbing(vertical);
        Flipping();
    }

    private void Moving(float horizontal)
    {
        float hSpeed = horizontal * movementSpeed;

        anim.SetBool("Running", onGround && Mathf.Abs(hSpeed) > .001f);
        anim.SetFloat("RunningSpeed", Mathf.Abs(hSpeed));

        if (onGround || !climbing)
        {
            rb.velocity = new Vector2(hSpeed, rb.velocity.y);
        }
    }

    private void Jumping(float vertical)
    {
        if (jump && onGround && Mathf.Abs(rb.velocity.y) <= .001f)
        {
            jump = false;
            rb.AddForce(Vector2.up * jumpForce);
            anim.SetTrigger("Jump");
        }
        if (!onGround && !climbing && rb.velocity.y < 0)
        {
            rb.AddForce(Vector2.down * fallForce);
        }
    }

    private void Climbing(float vertical)
    {
        if (!climbing && onLadder && Mathf.Abs(vertical) > .001f)
        {
            climbing = true;
            rb.gravityScale = 0;

            IgnorePlatformCollision(true);

            anim.SetBool("Climbing", true);
            anim.SetFloat("ClimbingSpeed", 0);
        }
        if (climbing)
        {
            float vSpeed = vertical * climbingSpeed;

            rb.velocity = new Vector2(0, vSpeed);

            anim.SetFloat("ClimbingSpeed", vSpeed);
        }
        if (climbing && (!onLadder || onGround))
        {
            climbing = false;
            rb.gravityScale = gravity;

            anim.SetBool("Climbing", false);
        }
    }

    private void Flipping()
    {
        if ((rb.velocity.x > 0 && transform.localScale.x < 0) || (rb.velocity.x < 0 && transform.localScale.x > 0))
        {
            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
        }
    }

    private void GroundCheck()
    {
        Debug.DrawRay(transform.position, Vector2.down * skin, Color.green);
        Collider2D collider = Physics2D.Raycast(transform.position, Vector2.down, skin, groundLayer).collider;
        onGround = collider && rb.IsTouching(collider);
    }

    private void IgnorePlatformCollision(bool ignore)
    {
        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), onLadder.attachedPlatform, ignore);
    }

    public void Kill(float delay = 0)
    {
        gameObject.SetActive(false);
        levelManager.Invoke("RestartLevel", delay);
    }
}
