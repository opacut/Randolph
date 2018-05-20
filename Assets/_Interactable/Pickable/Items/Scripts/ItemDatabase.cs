using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Randolph.Core;

namespace Randolph.Interactable {
    //[CreateAssetMenu(fileName = "Item database", menuName = "Randolph/Inventory/Item database", order = 30)]
    public class ItemDatabase : ScriptableObject {

        //! Supports only 1 type / 1 item (e.g no 3 different Whiskeys)

        public static ItemDatabase itemDatabase;
        public const char StringSeparator = ',';

        [SerializeField] List<NumberedItem> numberedItemList = new List<NumberedItem>();

        [SerializeField] Sprite defaultBackground;
        [SerializeField] Sprite activeBackground;
        [SerializeField] Sprite passiveBackground;

        void OnEnable() {
            if (itemDatabase != null && itemDatabase != this) {
                Debug.LogWarning($"A duplicate ItemDatabase found. Try searching for <b>t:ItemDatabase</b> and merge them.");
            } else {
                itemDatabase = this;
            }
        }

        /// <summary>Removes all unnecessary item references from the database, such as duplicate or empty items.</summary>
        public void Prune() {
            numberedItemList = numberedItemList.RemoveInvalidItems();
        }

        public bool PruneCheck() {
            int newCount = numberedItemList.RemoveInvalidItems().Count;
            return (numberedItemList.Count > newCount);
        }

        /// <summary>Adds an item to the database and assigns an index to it.</summary>
        /// <param name="item">Item to add to the database.</param>
        public void AddItem(Item item) {
            numberedItemList.Add(new NumberedItem(numberedItemList.Count, item));
        }

        /// <summary>Returns a string composed from all given items to save to <see cref="PlayerPrefs"/>.</summary>
        /// <returns>Item IDs separated with a <see cref="StringSeparator"/> – or an empty string if the inventory is empty.</returns>
        public string GetItemsKey(List<InventoryItem> inventoryItems) {
            var builder = new StringBuilder();
            if (inventoryItems == null || inventoryItems.Count == 0) return string.Empty;
            foreach (InventoryItem item in inventoryItems) {
                if (!ContainsItem(item)) {
                    Debug.LogWarning($"Item <b>{item.GetType()}</b> isn't included in the ItemDatabase – and won't be saved.");
                } else {
                    int itemId = GetItemId(item);
                    if (itemId < 0) continue;
                    builder.Append($"{itemId}{StringSeparator}");
                }
            }
            return builder.ToString();
        }

        /// <summary>Creates an item list from a given string key.</summary>
        /// <returns>A list of items saved in the string key.</returns>
        public List<InventoryItem> GetItemsFromKey(string inventoryString) {
            var ids = Methods.StringToIntList(inventoryString, StringSeparator);
            var itemList = ids.Select(GetItemFromId).Where(item => item != null);
            return itemList.ToList();
        }

        /// <summary>Checks whether the item database contains the given item.</summary>
        /// <param name="item">Item to look for</param>
        /// <returns>True if the given item is a part of the item database.</returns>
        public bool ContainsItem(Item item) {
            return item != null && numberedItemList.Any(x => x.item == item);
        }

        /// <summary>Checks whether the item database contains the given inventory item.</summary>
        /// <param name="inventoryItem">Item to look for</param>
        /// <returns>True if the given item is a part of the item database.</returns>
        public bool ContainsItem(InventoryItem inventoryItem) {
            bool containsType = numberedItemList.Any(x => x.item.GetInventoryItem().GetType() == inventoryItem.GetType());
            return inventoryItem != null && containsType;
        }

        /// <summary>Gets the item's unique ID.</summary>        
        public int GetItemId(InventoryItem inventoryItem) {
            NumberedItem numberedItem = numberedItemList.FirstOrDefault(x => x.item.GetInventoryItem().GetType() == inventoryItem.GetType());
            return numberedItem?.id ?? -1;
        }

        /// <summary>Gets the item from its unique ID.</summary>        
        public InventoryItem GetItemFromId(int id) {
            return numberedItemList.FirstOrDefault(x => x.id == id)?.item.GetInventoryItem();
        }

    }
}
