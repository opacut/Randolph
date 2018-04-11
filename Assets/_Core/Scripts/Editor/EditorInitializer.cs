using System.Linq;

using Randolph.Interactable;

using UnityEditor;

using UnityEngine;

namespace Randolph.Core {
    /// <summary>Defines actions to be called after each compilation or starting the editor.</summary>
    [InitializeOnLoad]
    public static class EditorInitializer {

        static EditorInitializer() {
            CheckItemDatabase(); 
            CheckSpriteRenderers();
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

        #region Sprites

        ///<summary>Checks prefabs with sprite renderer</summary>
        static void CheckSpriteRenderers() { 
            var spriteRendererOfPrefabs = EditorMethods.FindComponentsOfPrefabs<SpriteRenderer>(); 
            spriteRendererOfPrefabs.ForEach(PixelPerfectSprite);
        }

        static void PixelPerfectSprite(SpriteRenderer spriteRederer) {
            // TODO: Set pixel perfect material
        }

        #endregion

    }
}
