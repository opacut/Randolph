using UnityEngine;

public class Crows : MonoBehaviour
{
    public bool SecondPosition;
    public bool FinalPosition;
    private Animator animator;
    private Transform tr;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if ((!SecondPosition) && (other.tag == "Player"))
        {
            animator.SetBool("FirstTouch", true);
            //gameObject.transform.position = new Transform;
            SecondPosition = true;
        }
        else if ((!FinalPosition) && (other.tag == "Player"))
        {
            animator.SetBool("SecondTouch", true);
            FinalPosition = true;
        }
        else if (other.tag == "Enemy")
        {
            Destroy(gameObject);
        }
    }
    
}
