using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using JetBrains.Annotations;
using Randolph.Core;
using UnityEditor;
using UnityEngine;

namespace Randolph.Interactable {
    public static class ItemExtensions {

        /// <summary>Removes all unnecessary item references from the database, such as duplicate or empty items.</summary>
        /// <param name="numberedItemList">Item database's list of items.</param>
        public static List<NumberedItem> RemoveInvalidItems(this List<NumberedItem> numberedItemList) {
            int originalCount = numberedItemList.Count;

            numberedItemList = numberedItemList.RemoveNullItems();
            numberedItemList = numberedItemList.RemoveDuplicateItems();

            int removedCount = originalCount - numberedItemList.Count;
            if (removedCount > 0) {
                numberedItemList.RecalculateItemIndexes();
            }

            return numberedItemList;
        }

        static List<NumberedItem> RemoveNullItems(this List<NumberedItem> numberedItemList) {
            return numberedItemList.Where(x => x.item != null).ToList();
        }

        static List<NumberedItem> RemoveDuplicateItems(this List<NumberedItem> numberedItemList) {
            return numberedItemList
                  .GroupBy(numberedItem => numberedItem.item)
                  .Select(group => group.First())
                  .ToList();
        }

        /// <summary>Makes sure there are no gaps in item numbering due to removing items.</summary>
        /// <param name="numberedItemList">Item database's list of items.</param>
        static void RecalculateItemIndexes(this List<NumberedItem> numberedItemList) {
            for (int i = 0; i < numberedItemList.Count; i++) {
                numberedItemList[i].id = i;
            }
        }

        public static bool CreateItemScript(string name, string folder) {
            name = name.ToTitleCase();
            string scriptPath = $"{folder}/{name}.cs";

            if (File.Exists(scriptPath) == false) {
                GenerateFiles.GenerateMonobehaviour(name,
                        folder,
                        nameof(InventoryItem),
                        $"{nameof(Randolph)}.{nameof(Randolph.Interactable)}",
                        new[] { "System", "UnityEngine" },
                        new [] { "public override bool IsSingleUse { get; } = false" },
                        "public override bool IsApplicable(GameObject target)",
                        "public override void OnApply(GameObject target)"
                );

                return true;
            } else return false;
        }

    }
}
