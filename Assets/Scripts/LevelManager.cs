using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private PlayerController player;
    [SerializeField] private List<Checkpoint> checkpoints = new List<Checkpoint>();

    private Checkpoint reached;

    private void Awake()
    {
        Debug.Assert(checkpoints.Any());
        reached = checkpoints.First();
    }

    private void Update()
    {
        if (Input.GetKey("escape"))
        {
            Application.Quit();
        }
    }

    private void Start()
    {
        player.transform.position = reached.transform.position;
    }

    public void CheckpointReached(Checkpoint checkpoint)
    {
        reached = (IsCheckpointVisited(checkpoint)) ? reached : checkpoint;
    }

    private bool IsCheckpointVisited(Checkpoint checkpoint)
    {
        return reached && checkpoints.IndexOf(checkpoint) < checkpoints.IndexOf(reached);
    }

    public void RestartLevel()
    {
        reached.RestoreState();
        player.gameObject.SetActive(true);
        player.transform.position = reached.transform.position;
    }
}
