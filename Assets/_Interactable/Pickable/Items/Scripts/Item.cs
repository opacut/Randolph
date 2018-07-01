using UnityEngine;

namespace Randolph.Interactable {
    [CreateAssetMenu(fileName = "Item", menuName = "Randolph/Inventory/Item", order = 33)]
    public class Item : ScriptableObject {
        [SerializeField]
        private bool initialized;

        [SerializeField]
        private GameObject prefab;

        public bool IsInitialized => initialized;
        public InventoryItem InventoryItem => prefab.GetComponent<InventoryItem>();
    }
}
