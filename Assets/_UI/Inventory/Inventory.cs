using System;
using System.Collections.Generic;
using System.Linq;
using Randolph.Characters;
using Randolph.Core;
using Randolph.Interactable;
using Randolph.Levels;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Randolph.UI {
    public class Inventory : MonoBehaviour {

        public const string InventoryKey = "Inventory";

        public static Inventory inventory;

        [SerializeField] InventoryIcon iconPrefab;
        [SerializeField] ItemDatabase itemDatabase;
        [SerializeField] float applicableDistance = 6;

        public float ApplicableDistance => applicableDistance;

        List<InventoryIcon> icons = new List<InventoryIcon>();

        public List<InventoryItem> Items {
            get { return new List<InventoryItem>(icons.Select(icon => icon.Item)); }
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

        void Awake() {
            Debug.Assert(iconPrefab, "The prefab for an inventory icon is missing!", gameObject);
            LevelManager.OnNewLevel += OnNewLevel;
        }

        void OnNewLevel(Scene scene) {
            inventory = FindObjectOfType<Inventory>();
        }


        public void Add(InventoryItem item) {
            InventoryIcon icon = Instantiate(iconPrefab, transform);
            icon.Init(this, item);
            icons.Add(icon);
        }

        public void Remove(InventoryItem item) {
            InventoryIcon icon = icons.Find(ico => ico.Item == item);
            if (icon == null) {
                return;
            }

            Destroy(icon.gameObject);
            icons.Remove(icon);
        }

        public bool Contains(InventoryItem item) {
            return icons.Any(ico => ico.Item == item);
        }

        public bool IsWithinApplicableDistance(Vector2 position) {
            return Vector2.Distance(position, Constants.Randolph.transform.position) <= ApplicableDistance;
        }

        public bool IsApplicableTo(InventoryItem item, GameObject target) {
            if (Contains(item) && ((target.GetComponent<InventoryItem>() && Contains(target.GetComponent<InventoryItem>())) || DistanceCheck(target))) return item.IsApplicable(target);
            return false;
        }

        public bool ApplyTo(InventoryItem item, GameObject target) {
            if (!IsApplicableTo(item, target)) {
                return false;
            }

            item.Apply(target);
            if (item.IsSingleUse) {
                Remove(item);
            }
            return true;
        }

        bool DistanceCheck(GameObject target) {
            if (target.tag == Constants.Tag.Player) {
                return true;
            }

            Debug.Assert(target.GetComponent<Collider2D>());
            ColliderDistance2D result = Constants.Randolph.GetComponent<Rigidbody2D>().Distance(target.GetComponent<Collider2D>());
            return result.isValid && Mathf.Abs(result.distance) < applicableDistance;
        }

        void OnDestroy() {
            LevelManager.OnNewLevel -= OnNewLevel;
        }

        /// <summary>Saves the inventory state to <see cref="PlayerPrefs"/>.</summary>       
        public void SaveStateToPrefs(List<InventoryItem> inventoryItems) {
            string itemString = GetItemsKey(inventoryItems);
            PlayerPrefs.SetString(InventoryKey, itemString);
        }

        /// <summary>Saves the inventory state to <see cref="PlayerPrefs"/>.</summary>       
        public void RestorStateFromPrefs() {
            var itemString = PlayerPrefs.HasKey(InventoryKey) ? PlayerPrefs.GetString(InventoryKey) : string.Empty;
            if (itemString == string.Empty) {
                Items = new List<InventoryItem>();
            } else {
                Items = GetItemsFromKey(itemString);
                DeactivateItemsInScene(Items);
            }
        }

        void DeactivateItemsInScene(List<InventoryItem> items) {
            foreach (InventoryItem item in items) {
                Type itemType = item.GetType();
                var sceneItem = (InventoryItem) FindObjectOfType(itemType);
                sceneItem.SetComponentsActive(false);
            }
        }

        /// <summary>Returns a string composed from all given items to save to <see cref="PlayerPrefs"/>.</summary>
        public string GetItemsKey(List<InventoryItem> inventoryItems) {
            return itemDatabase.GetItemsKey(inventoryItems);
        }

        /// <summary>Creates an item list from a given string key.</summary>
        public List<InventoryItem> GetItemsFromKey(string inventoryKey) {
            return itemDatabase.GetItemsFromKey(inventoryKey);
        }

    }
}
