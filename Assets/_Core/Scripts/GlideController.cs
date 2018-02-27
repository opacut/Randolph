using UnityEngine;

public class GlideController : MonoBehaviour, IRestartable
{
    public float speed;

    Vector3 destination;

    public Vector3 OriginalDestination;
    public Vector3 dest2;
    public Vector3 dest3;

    public bool ReachedFirst;

    void Start()
    {
        // Set the destination to be the object's position so it will not start off moving
        OriginalDestination = gameObject.transform.position;
        SetDestination(gameObject.transform.position);
    }

    void Update()
    {
        if(gameObject.transform.position == dest2)
        {
            ReachedFirst = true;
        }
        // If the object is not at the target destination
        if (destination != gameObject.transform.position)
        {
            // Move towards the destination each frame until the object reaches it
            IncrementPosition();
        }
    }

    public void Restart()
    {
        ReachedFirst = false;
        gameObject.transform.position = OriginalDestination;
        SetDestination(gameObject.transform.position);
        gameObject.SetActive(true);
    }

    void IncrementPosition()
    {
        // Calculate the next position
        float delta = speed * Time.deltaTime;
        Vector3 currentPosition = gameObject.transform.position;
        Vector3 nextPosition = Vector3.MoveTowards(currentPosition, destination, delta);

        // Move the object to the next position
        gameObject.transform.position = nextPosition;
    }

    // Set the destination to cause the object to smoothly glide to the specified location
    public void SetDestination(Vector3 value)
    {
        destination = value;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if ((!ReachedFirst) && (other.tag == "Player"))
        {
            SetDestination(dest2);
            //ReachedFirst = true;
        }
        else if (other.tag == "Player")
        {
            SetDestination(dest3);
        }
        else if (other.tag == "Enemy")
        {
            other.gameObject.GetComponent<Flytrap>().Invoke("Deactivate", 0);
            //Destroy(gameObject);
            gameObject.SetActive(false);
        }
    }
}