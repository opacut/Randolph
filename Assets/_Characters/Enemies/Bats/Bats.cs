using System.Collections.Generic;
using Randolph.Characters;
using Randolph.Core;
using Randolph.Interactable;
using Randolph.UI;
using UnityEngine;

// TODO Use glider
//[RequireComponent(typeof(Glider))]
public class Bats : Clickable {
    public delegate void DestinationChange(Vector2 position, Vector2 nextDestination);

    public delegate void GlidingEnd(Vector2 position);

    private Animator animator;
    private AudioSource audioSource;
    [SerializeField] private bool continuous;
    private Vector2 currentDestination;

    private Queue<Vector2> destinationQueue = new Queue<Vector2>();
    [SerializeField] private List<Vector2> destinations = new List<Vector2>();

    [SerializeField] private bool loop;
    [SerializeField] private float speed = 20;
    [SerializeField] private AudioClip swooshSound;
    public override Cursors CursorType { get; protected set; } = Cursors.Inspect;

    public bool Disturbed { get; private set; }

    public event DestinationChange OnDestinationChange;

    public event GlidingEnd OnGlidingEnd;
    //Glider glider;

    // TODO: Harmful to: layer/tag | Destroyed by: layer/tag

    protected override void Awake() {
        base.Awake();

        animator = GetComponent<Animator>();
        //glider = GetComponent<Glider>();

        //glider.OnPlayerDisturbed += OnPlayerDisturbed;
        audioSource = AudioPlayer.audioPlayer.AddAudioSource(gameObject);
    }

    protected override void Start() {
        base.Start();
        outline.color = 2;
        CreateDestinationQueue();
    }

    public void OnTriggerEnter2D(Collider2D other) {
        if (other.tag != Constants.Tag.Player) {
            return;
        }

        animator.SetBool("Trapped", true);
        other.GetComponent<PlayerController>().Kill();
    }

    protected override void Update() {
        base.Update();
        
        // If the object is not at the target destination
        if ((Vector2) transform.position != currentDestination) {
            // Move towards the destination each frame until the object reaches it
            IncrementPosition();
        } else if (continuous && Disturbed) {
            SetNextDestination();
        }
    }

    [ContextMenu("Fly Away")]
    public void StartMoving() {
        animator.SetBool("Flying", true);
        Disturbed = true;
        AudioPlayer.audioPlayer.PlayLocalSound(audioSource, swooshSound);
        SetNextDestination();
    }

    private void SetNextDestination() {
        if (destinationQueue.Count > 0) {
            SetDestination(destinationQueue.Dequeue());
            OnDestinationChange?.Invoke(transform.position, currentDestination);
        } else if (loop) {
            CreateDestinationQueue();
            OnDestinationChange?.Invoke(transform.position, currentDestination);
        } else {
            // Invoke the event of ended gliding
            OnGlidingEnd?.Invoke(transform.position);
        }
    }

    private void SetDestination(Vector2 value) {
        currentDestination = value;
    }

    private void CreateDestinationQueue() {
        // Add all destinations to queue            
        destinationQueue = new Queue<Vector2>(destinations);

        // Set the destination to be the object's initial position so it will not start off moving            
        SetDestination(savedPosition);
    }

    private void IncrementPosition() {
        // Calculate the next position
        var delta = speed * Time.deltaTime;

        // Move the object to the next position
        transform.position = Vector2.MoveTowards(transform.position, currentDestination, delta);
    }

    #region IRestartable
    public override void Restart() {
        base.Restart();
        animator.SetBool("Trapped", false);
        animator.SetBool("Flying", false);

        CreateDestinationQueue();
        Disturbed = false;
        gameObject.SetActive(true);
    }
    #endregion

    private void OnDrawGizmosSelected() {
        for (var i = 0; i < destinations.Count - 1; ++i) {
            Gizmos.DrawLine(destinations[i], destinations[i + 1]);
        }
        if (loop) {
            Gizmos.DrawLine(destinations[destinations.Count - 1], destinations[0]);
        }
    }
}
