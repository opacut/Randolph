using Assets._Interactable;
using Randolph.Interactable;
using UnityEngine;

public class GrassTuft : Interactable, ISlashable {
    [SerializeField] private Sprite cutGrass;

    [SerializeField] public Transform grass;

    private bool hasGrass = true;
    private Sprite original;

    [SerializeField] private GameObject spawnPoint;

    public void Slash() {
        if (hasGrass) {
            hasGrass = false;
            spriteRenderer.sprite = cutGrass;
            Instantiate(grass, spawnPoint.transform.position, Quaternion.identity);
        }
    }

    protected override void Awake() {
        base.Awake();
        original = spriteRenderer.sprite;
    }

    public override void Restart() {
        base.Restart();
        hasGrass = true;
        spriteRenderer.sprite = original;
    }

    public override void Interact() { Debug.Log("Tuft clicked"); }
}
