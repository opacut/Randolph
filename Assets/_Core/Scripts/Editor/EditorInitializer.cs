using System;
using System.Collections;
using System.Reflection;

using Randolph.Interactable;

using UnityEditor;

using UnityEngine;

namespace Randolph.Core {
    [InitializeOnLoad]
    public static class EditorInitializer {

        // After each compilation or starting the editor
        static EditorInitializer() {
            CheckItemDatabase();
        }


        #region Items

        /// <summary>Checks the <see cref="ItemDatabase"/> asset for possible problems, such as no database, duplicate databases or missing items.</summary>
        static void CheckItemDatabase() {
            string[] itemDatabases = AssetDatabase.FindAssets("t:ItemDatabase");
            if (itemDatabases.Length == 0) {
                var itemDatabaseAsset = ScriptableObject.CreateInstance<ItemDatabase>();
                AssetDatabase.CreateAsset(itemDatabaseAsset, "Assets/Item database.asset");
                AssetDatabase.SaveAssets();

                Selection.activeObject = itemDatabaseAsset;
                Debug.LogWarning("There is no ItemDatabase asset. Creating one in the project's root.", itemDatabaseAsset);
            } else if (itemDatabases.Length > 1) {
                // Duplicates are handled inside the ItemDatabase class
            }
        }

        #endregion

    }
}
