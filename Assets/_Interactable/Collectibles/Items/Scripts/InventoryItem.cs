using UnityEngine;


public abstract class InventoryItem : Pickable
{
    public Sprite icon;
    public Sprite iconOK;
    public Sprite iconNOK;
    Inventory inventory;

    private void Awake()
    {
        inventory = FindObjectOfType<Inventory>();
    }

    public override void OnPick()
    {
        inventory.Add(this);
        gameObject.SetActive(false);
    }

    public abstract bool IsApplicable(GameObject target);

    public abstract void OnApply(GameObject target);
}
