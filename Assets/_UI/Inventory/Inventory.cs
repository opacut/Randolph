using System.Collections.Generic;
using System.Linq;
using Randolph.Core;
using Randolph.Interactable;
using UnityEngine;

namespace Randolph.UI {
    public class Inventory : MonoBehaviour {

        [SerializeField]
        float applicableDistance = 3;
        [SerializeField]
        Rigidbody2D player;
        [SerializeField]
        InventoryIcon iconPrefab;

        List<InventoryIcon> icons = new List<InventoryIcon>();

        public List<InventoryItem> Items {
            get { return new List<InventoryItem>(icons.Select(icon => icon.item)); }
            set {
                foreach (InventoryIcon icon in icons) {
                    Destroy(icon.gameObject);
                }

                icons.Clear();

                foreach (InventoryItem item in value) {
                    Add(item);
                }
            }
        }

        public void Awake() {
            Debug.Assert(iconPrefab, "The prefab for an inventory icon is missing!", gameObject);
            Debug.Assert(player);
        }

        public void Add(InventoryItem item) {
            InventoryIcon icon = Instantiate(iconPrefab, transform);
            icon.Init(this, item);
            icons.Add(icon);
        }

        public void Remove(InventoryItem item) {
            InventoryIcon icon = icons.Find(ico => ico.item == item);
            if (icon == null) {
                return;
            }

            Destroy(icon.gameObject);
            icons.Remove(icon);
        }

        public bool Contains(InventoryItem item) {
            return icons.Any(ico => ico.item == item);
        }

        public bool IsApplicableTo(InventoryItem item, GameObject target) {
            if (Contains(item) && DistanceCheck(target)) return item.IsApplicable(target);
            return false;
        }

        public bool ApplyTo(InventoryItem item, GameObject target) {
            if (!IsApplicableTo(item, target)) return false;

            item.OnApply(target);
            Remove(item);
            return true;
        }

        private bool DistanceCheck(GameObject target) {
            if (target == player.gameObject) return true;

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
}