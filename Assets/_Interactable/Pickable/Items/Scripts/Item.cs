using UnityEngine;

namespace Randolph.Interactable {

    [CreateAssetMenu(fileName = "Item", menuName = "Randolph/Inventory/Item", order = 33)]    
    public class Item : ScriptableObject {

        [SerializeField] GameObject prefab;
        [SerializeField] bool initialized = false;

        public bool IsInitialized() {
            return initialized;           
        }

        public InventoryItem GetInventoryItem() {
            return prefab.GetComponent<InventoryItem>();
        }

    }
}
