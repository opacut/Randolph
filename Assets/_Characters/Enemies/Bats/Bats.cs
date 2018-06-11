using Randolph.Characters;
using Randolph.Core;
using Randolph.Levels;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(Glider))]
public class Bats : MonoBehaviour, IRestartable
{
    public bool Disturbed { get; private set; } = false;
    Queue<Vector2> destinationQueue = new Queue<Vector2>();
    public delegate void DestinationChange(Vector2 position, Vector2 nextDestination);
    public event DestinationChange OnDestinationChange;
    Vector2 currentDestination;
    [SerializeField] bool loop = false;
    public delegate void GlidingEnd(Vector2 position);
    public event GlidingEnd OnGlidingEnd;
    [SerializeField] List<Vector2> destinations = new List<Vector2>();
    [SerializeField] bool continuous = false;
    [SerializeField] float speed = 20;

    [SerializeField, ReadonlyField] Vector2 initialPosition;

    Animator animator;
    AudioSource audioSource;
    //Glider glider;

    // TODO: Harmful to: layer/tag | Destroyed by: layer/tag

    void Awake()
    {
        initialPosition = gameObject.transform.position;
        animator = GetComponent<Animator>();
        //glider = GetComponent<Glider>();

        //glider.OnPlayerDisturbed += OnPlayerDisturbed;

    }

    /*
    void OnPlayerDisturbed(PlayerController player)
    {
        return;
    }
    */

    public void Restart()
    {
        animator.SetBool("Trapped", false);
        gameObject.transform.position = initialPosition;

        transform.position = initialPosition;
        CreateDestinationQueue();
        Disturbed = false;
        gameObject.SetActive(true);
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == Constants.Tag.Player)
        {
            animator.SetBool("Trapped", true);
            other.GetComponent<PlayerController>().Kill();
        }
    }

    void Start()
    {
        initialPosition = transform.position;

        CreateDestinationQueue();
    }

    void Update()
    {
        // If the object is not at the target destination
        if ((Vector2)transform.position != currentDestination)
        {
            // Move towards the destination each frame until the object reaches it
            IncrementPosition();
        }
        else if (continuous && Disturbed)
        {
            SetNextDestination();
        }
    }

    void StartMoving()
    {
        Disturbed = true;
        SetNextDestination();
    }

    void SetNextDestination()
    {
        if (destinationQueue.Count > 0)
        {
            SetDestination(destinationQueue.Dequeue());
            OnDestinationChange?.Invoke(transform.position, currentDestination);
        }
        else if (loop)
        {
            CreateDestinationQueue();
            OnDestinationChange?.Invoke(transform.position, currentDestination);
        }
        else
        {
            // Invoke the event of ended gliding
            OnGlidingEnd?.Invoke(transform.position);
        }
    }

    void SetDestination(Vector2 value)
    {
        currentDestination = value;
    }

    void CreateDestinationQueue()
    {
        // Add all destinations to queue            
        destinationQueue = new Queue<Vector2>(destinations);

        // Set the destination to be the object's initial position so it will not start off moving            
        SetDestination(initialPosition);
    }

    void IncrementPosition()
    {
        // Calculate the next position
        float delta = speed * Time.deltaTime;

        // Move the object to the next position
        transform.position = Vector2.MoveTowards(transform.position, currentDestination, delta);
    }
}
