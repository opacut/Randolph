using System.Collections.Generic;
using System.Linq;
using Randolph.Characters;
using Randolph.Interactable;
using Randolph.Levels;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Randolph.UI {
    public class Inventory : MonoBehaviour {

        // public static Inventory inventory;

        [SerializeField] InventoryIcon iconPrefab;
        [SerializeField] float applicableDistance = 3;

        Rigidbody2D player;

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

        void Awake() {
            // TODO: Pass along with a canvas
            
            /*           
            //! Pass Inventory between levels; destroy excess ones
            DontDestroyOnLoad(this);

            if (FindObjectsOfType(GetType()).Length > 1) {
                Destroy(gameObject);
            } else {
                inventory = this;
            }
            */
            Debug.Assert(iconPrefab, "The prefab for an inventory icon is missing!", gameObject);

            LevelManager.OnNewLevel += OnNewLevel;
        }

        void OnNewLevel(Scene scene, PlayerController playerController) {
            player = playerController.GetComponent<Rigidbody2D>();
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

        public bool IsWithinApplicableDistance(Vector2 position) {
            return Vector2.Distance(position, player.transform.position) <= applicableDistance;
        }

        public bool IsApplicableTo(InventoryItem item, GameObject target) {
            if (Contains(item) && DistanceCheck(target)) return item.IsApplicable(target);
            return false;
        }

        public bool ApplyTo(InventoryItem item, GameObject target) {
            if (!IsApplicableTo(item, target)) return false;

            item.OnApply(target);
            if (item.IsSingleUse) {
                Remove(item);
            }
            return true;
        }

        bool DistanceCheck(GameObject target) {
            if (target == player.gameObject) return true;

            Debug.Assert(target.GetComponent<Collider2D>());
            ColliderDistance2D result = player.Distance(target.GetComponent<Collider2D>());
            return result.isValid && Mathf.Abs(result.distance) < applicableDistance;
        }

    }
}
