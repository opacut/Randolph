using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using UnityEditor;

public class Inventory : MonoBehaviour
{
    [SerializeField] private float applicableDistance = 3;
    [SerializeField] private Rigidbody2D player;
    [SerializeField] private InventoryIcon iconPrefab;

    private List<InventoryIcon> icons = new List<InventoryIcon>();


    public List<InventoryItem> Items
    {
        get
        {
            return new List<InventoryItem>(icons.Select(icon => icon.Item));
        }
        set
        {
            foreach (InventoryIcon icon in icons)
            {
                Destroy(icon.gameObject);
            }

            icons.Clear();

            foreach (InventoryItem item in value)
            {
                Add(item);
            }
        }
    }


    public void Awake()
    {
        Debug.Assert(iconPrefab);
        Debug.Assert(player);
    }

    public void Add(InventoryItem item)
    {
        InventoryIcon icon = Instantiate(iconPrefab, transform);
        icon.Init(this, item);
        icons.Add(icon);
    }

    public void Remove(InventoryItem item)
    {
        InventoryIcon icon = icons.Find(ico => ico.Item == item);
        if (icon)
        {
            icons.Remove(icon);
            Destroy(icon.gameObject);
        }
    }

    public bool Contains(InventoryItem item)
    {
        return icons.Any(ico => ico.Item == item);
    }

    
    public bool IsApplicableTo(InventoryItem item, GameObject target)
    {
        if (Contains(item) && DistanceCheck(target))
            return item.IsApplicable(target);
        return false;
    }

    public bool ApplyTo(InventoryItem item, GameObject target)
    {
        if (!IsApplicableTo(item, target))
            return false;

        item.OnApply(target);
        Remove(item);
        return true;
    }
    
    private bool DistanceCheck(GameObject target)
    {
        if (target == player.gameObject)
            return true;

        Debug.Assert(target.GetComponent<Collider2D>());
        ColliderDistance2D result = player.Distance(target.GetComponent<Collider2D>());
        return result.isValid && Mathf.Abs(result.distance) < applicableDistance;
    }

    /*
    private void OnDrawGizmos()
    {
        Handles.color = Color.red;
        Handles.DrawWireDisc(player.transform.position, Vector3.back, applicableDistance);
    }*/
}
