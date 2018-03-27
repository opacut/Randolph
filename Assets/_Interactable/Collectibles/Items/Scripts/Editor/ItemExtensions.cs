﻿using System.Collections.Generic;
using System.Globalization;
using System.Linq;
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

        public static void CreateItemScript(string name, string folder) {
            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
            string scriptName = textInfo.ToTitleCase(name).Replace("-","_").Replace(" ", "");
            string scriptPath = $"{folder}/{scriptName}.cs";

            Debug.Log("Creating Classfile: " + scriptPath);
        }

    }
}