using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using PixelPerfectSprite = Com.LuisPedroFonseca.ProCamera2D.ProCamera2DPixelPerfectSprite;
using Randolph.Interactable;

namespace Randolph.Core {
    /// <summary>Defines actions to be called after each compilation or starting the editor.</summary>
    [InitializeOnLoad]
    public static class EditorInitializer {

        static EditorInitializer() {
            CheckItemDatabase();
            CheckSpriteRenderers();
            // CheckColliderRigidbodies();
        }


        #region Items

        /// <summary>Checks the <see cref="ItemDatabase"/> asset for possible problems, such as no database, duplicate databases or missing items.</summary>
        static void CheckItemDatabase() {
            string[] itemDatabases = AssetDatabase.FindAssets("t:ItemDatabase");
            if (itemDatabases.Length == 0) {
                Debug.LogWarning("There is no ItemDatabase asset. This can be the case of a reimport – otherwise, you should create one.");
            } else if (itemDatabases.Length > 1) {
                // Duplicates are handled inside the ItemDatabase class
            }
        }

        /// <summary>Creates an item database asset in the project's root folder.</summary>
        static void CreateItemDatabase() {
            var itemDatabaseAsset = ScriptableObject.CreateInstance<ItemDatabase>();
            AssetDatabase.CreateAsset(itemDatabaseAsset, "Assets/Item database.asset");
            AssetDatabase.SaveAssets();

            Selection.activeObject = itemDatabaseAsset;
            Debug.LogWarning("Created an ItemDatabase asset in the project's root.", itemDatabaseAsset);
        }

        #endregion

        #region Sprites

        ///<summary>Checks prefabs with sprite renderer</summary>
        static void CheckSpriteRenderers() {
            List<SpriteRenderer> spriteRendererOfPrefabs = EditorMethods.FindComponentsOfPrefabs<SpriteRenderer>();
            spriteRendererOfPrefabs.ForEach(PixelPerfectSprite);
        }

        static void PixelPerfectSprite(SpriteRenderer spriteRederer) {
            Material pixelSnapMaterial = EditorConstants.Sprites.PixelSnapMaterial;
            if (pixelSnapMaterial && spriteRederer.sharedMaterial != pixelSnapMaterial) {
                spriteRederer.sharedMaterial = pixelSnapMaterial;
            }

            // var ppSpiteComponent = spriteRederer.GetComponent<PixelPerfectSprite>();                    
            // if (ppSpiteComponent == null) {

            // TODO: Pixel Perfect Sprite component?

            // Move it up to the SpriteRenderer | …IndexOf(spriteRenderer)
            // UnityEditorInternal.ComponentUtility.MoveComponentUp(someComponent);
            // UnityEditorInternal.ComponentUtility.MoveComponentDown(someComponent);
            // }
        }

        #endregion

        #region Colliders and Rigidbodies

        static void CheckColliderRigidbodies() {
            List<Collider2D> collidersOfPrefabs = EditorMethods.FindComponentsOfPrefabs<Collider2D>();
            foreach (Collider2D collider in collidersOfPrefabs) { 
                if (collider.GetComponent<Rigidbody2D>() == null)
                {
                    Debug.LogWarning($"<b>{collider.gameObject.name}</b> prefab does not have a Rigidbody2D attached. Consider adding a rigidbody if it's a moving object (i.e. enemy, projectile) or a static object which flips between being harmful and harmless (i.e. spikes).", collider.gameObject);
                }
            }
        }

        #endregion
    }
}
