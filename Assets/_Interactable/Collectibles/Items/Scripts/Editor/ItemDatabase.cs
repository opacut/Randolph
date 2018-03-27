using UnityEngine;

using System.Collections.Generic;
using System.Linq;

namespace Randolph.Interactable {
    //[CreateAssetMenu(fileName = "Item database", menuName = "Randolph/Inventory/Item database", order = 30)]
    public class ItemDatabase : ScriptableObject {

        public static ItemDatabase itemDatabase;

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
            int originalCount = numberedItemList.Count;
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

        /// <summary>Checks whether the item database contains the given item.</summary>
        /// <param name="item">Item to look for</param>
        /// <returns>True if the given item is a part of the item database.</returns>
        public bool ContainsItem(Item item) {
            return item != null && numberedItemList.Any(x => x.item == item);
        }

    }
}
