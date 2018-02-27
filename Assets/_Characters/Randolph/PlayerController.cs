using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour {
    [SerializeField] float skin = 1.3f;
    [SerializeField] float climbingSpeed = 6;
    [SerializeField] float movementSpeed = 6;
    [SerializeField] float jumpForce = 800;
    [SerializeField] float fallForce = 50;
    [SerializeField] LayerMask groundLayer;

    // public LevelManager levelManager;

    const string ladderTag = "Ladder";
    const string pickableTag = "Pickable";

    Animator animator;
    Rigidbody2D rbody;
    float gravity = 0;
    bool jump = false;
    bool climbing = false;
    bool onGround = false;
    Ladder onLadder = null;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rbody = GetComponent<Rigidbody2D>();
        gravity = rbody.gravityScale;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == ladderTag)
        {
            onLadder = other.GetComponent<Ladder>();
        }

        if (other.tag == pickableTag)
        {
            Debug.Assert(other.GetComponent<Pickable>());
            other.GetComponent<Pickable>().OnPick();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == ladderTag)
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

        animator.SetBool("Running", onGround && Mathf.Abs(hSpeed) > 0.001f);
        animator.SetFloat("RunningSpeed", Mathf.Abs(hSpeed));

        if (onGround || !climbing)
        {
            rbody.velocity = new Vector2(hSpeed, rbody.velocity.y);
        }
    }

    private void Jumping(float vertical)
    {
        if (jump && onGround && Mathf.Abs(rbody.velocity.y) <= 0.001f)
        {
            jump = false;
            rbody.AddForce(Vector2.up * jumpForce);
            animator.SetTrigger("Jump");
        }
        if (!onGround && !climbing && rbody.velocity.y < 0)
        {
            rbody.AddForce(Vector2.down * fallForce);
        }
    }

    private void Climbing(float vertical)
    {
		// TODO: Move from ladder sideways + jump
		
        if (!climbing && onLadder && Mathf.Abs(vertical) > 0.001f)
        {
            climbing = true;
            rbody.gravityScale = 0;

            IgnorePlatformCollision(true);

            animator.SetBool("Climbing", true);
            animator.SetFloat("ClimbingSpeed", 0);
        }
        if (climbing)
        {
            float vSpeed = vertical * climbingSpeed;

            rbody.velocity = new Vector2(0, vSpeed);

            animator.SetFloat("ClimbingSpeed", vSpeed);
        }
        if (climbing && (!onLadder || onGround))
        {
            climbing = false;
            rbody.gravityScale = gravity;

            animator.SetBool("Climbing", false);
        }
    }

    private void Flipping()
    {
        if ((rbody.velocity.x > 0 && transform.localScale.x < 0) || (rbody.velocity.x < 0 && transform.localScale.x > 0))
        {
            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
        }
    }

    private void GroundCheck()
    {
        Debug.DrawRay(transform.position, Vector2.down * skin, Color.green);
        Collider2D coll = Physics2D.Raycast(transform.position, Vector2.down, skin, groundLayer).collider;
        onGround = coll && rbody.IsTouching(coll);
    }

    private void IgnorePlatformCollision(bool ignore)
    {
        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), onLadder.attachedPlatform, ignore);
    }

    public void Kill(float delay = 0.25f)
    {
        gameObject.SetActive(false);
        LevelManager.levelManager.Invoke("RestartLevel", delay);
    }
}
