using System.IO;
using Randolph.Core;
using UnityEditor;
using UnityEngine;

namespace Randolph.Interactable {
    [CustomEditor(typeof(Item))]
    public class ItemEditor : Editor {

        // TODO: To ItemDatabase

        Item item;

        SerializedProperty initialized;

        void OnEnable() {
            item = (Item) target;
            initialized = serializedObject.FindProperty(nameof(initialized));
        }

        public override void OnInspectorGUI() {
            serializedObject.Update();

            if (!ItemIsInitialized()) {
                DisplayNotItemDatabaseWarning();
                DisplayInitializeButton();
            }

            EditorMethods.DisplayScriptField(item);

            if (GUILayout.Button("Item database")) {
                Selection.activeObject = ItemDatabase.itemDatabase;
            }

            EditorGUILayout.LabelField(
                    new GUIContent("Properties"),
                    new GUIStyle(GUI.skin.label) {fontStyle = FontStyle.Bold}
            );
            EditorGUILayout.PropertyField(serializedObject.FindProperty("prefab"));

            serializedObject.ApplyModifiedProperties();
        }

        void DisplayNotItemDatabaseWarning() {
            EditorGUILayout.HelpBox(
                    $"{target.name} isn't included in the item database",
                    MessageType.Warning
            );
        }

        bool ItemIsInitialized() {
            return initialized.boolValue;
        }

        void DisplayInitializeButton() {
            Color originalColor = GUI.backgroundColor;
            GUI.backgroundColor = Color.yellow;
            if (GUILayout.Button("Initialize item")) {
                Initialize();
            }

            GUI.backgroundColor = originalColor;
        }

        void Initialize() {
            string itemName = target.name;

            //! Create folder
            string assetPath = AssetDatabase.GetAssetPath(Selection.activeObject);
            string parentFolder = Path.GetDirectoryName(assetPath)?.Replace(@"\\", "/").Replace(@"\", "/");
            AssetDatabase.CreateFolder(parentFolder, itemName);
            string newFolder = $"{parentFolder}/{itemName}";
            AssetDatabase.MoveAsset(assetPath, $"{newFolder}/{itemName}.asset");

            //! Create prefab
            var itemObject = new GameObject(itemName, typeof(SpriteRenderer), typeof(BoxCollider2D)) {
                    hideFlags = HideFlags.DontSaveInBuild | HideFlags.DontUnloadUnusedAsset | HideFlags.HideInHierarchy
            };
            if (ItemExtensions.CreateItemScript(itemName, newFolder)) {
                // TODO: Add when compiled
                // itemObject.AddComponent(Type.GetType(itemName));
            }
            string prefabPath = $"{newFolder}/{itemName}.prefab";
            PrefabUtility.CreatePrefab(prefabPath, itemObject);           
            //! Add itself to the database
            if (item && !ItemDatabase.itemDatabase.ContainsItem(item)) {
                ItemDatabase.itemDatabase.AddItem(item);
            }

            DestroyImmediate(itemObject);

            initialized.boolValue = true;
        }

    }
}
