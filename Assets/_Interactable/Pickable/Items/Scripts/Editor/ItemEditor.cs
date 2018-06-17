using System.IO;
using Randolph.Core;
using UnityEditor;
using UnityEngine;

namespace Randolph.Interactable {
    [CustomEditor(typeof(Item))]
    public class ItemEditor : Editor {
        private SerializedProperty initialized;

        // TODO: Create via ItemDatabase

        private Item item;

        private void OnEnable() {
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

            EditorGUILayout.LabelField(new GUIContent("Properties"),
                                       new GUIStyle(GUI.skin.label) { fontStyle = FontStyle.Bold });
            EditorGUILayout.PropertyField(serializedObject.FindProperty("prefab"));

            serializedObject.ApplyModifiedProperties();
        }

        private void DisplayNotItemDatabaseWarning() {
            EditorGUILayout.HelpBox($"{target.name} isn't included in the item database",
                                    MessageType.Warning);
        }

        private bool ItemIsInitialized() { return initialized.boolValue; }

        private void DisplayInitializeButton() {
            var originalColor = GUI.backgroundColor;
            GUI.backgroundColor = Color.yellow;
            if (GUILayout.Button("Initialize item")) {
                Initialize();
            }

            GUI.backgroundColor = originalColor;
        }

        private void Initialize() {
            var itemName = target.name;

            //! Create folder
            var assetPath = AssetDatabase.GetAssetPath(Selection.activeObject);
            var parentFolder = Path.GetDirectoryName(assetPath)?.Replace(@"\\", "/").Replace(@"\", "/");
            AssetDatabase.CreateFolder(parentFolder, itemName);
            var newFolder = $"{parentFolder}/{itemName}";
            AssetDatabase.MoveAsset(assetPath, $"{newFolder}/{itemName}.asset");

            //! Create prefab
            var itemObject = new GameObject(itemName, typeof(SpriteRenderer), typeof(BoxCollider2D)) {
                hideFlags = HideFlags.DontSaveInBuild | HideFlags.DontUnloadUnusedAsset | HideFlags.HideInHierarchy
            };
            if (CreateItemScript(itemName, newFolder)) {
                // TODO: Add Component when compiled
                // itemObject.AddComponent(Type.GetType(itemName));
            }
            var prefabPath = $"{newFolder}/{itemName}.prefab";
            PrefabUtility.CreatePrefab(prefabPath, itemObject);
            //! Add itself to the database
            if (item && !ItemDatabase.itemDatabase.ContainsItem(item)) {
                ItemDatabase.itemDatabase.AddItem(item);
            }

            DestroyImmediate(itemObject);

            initialized.boolValue = true;
        }

        private static bool CreateItemScript(string name, string folder) {
            name = name.ToTitleCase();
            var scriptPath = $"{folder}/{name}.cs";

            if (File.Exists(scriptPath) == false) {
                GenerateFiles.GenerateMonobehaviour(name,
                                                    folder,
                                                    nameof(InventoryItem),
                                                    $"{nameof(Randolph)}.{nameof(Randolph.Interactable)}",
                                                    new[] { "System", "UnityEngine" },
                                                    new[] { "public override bool IsSingleUse { get; } = false" },
                                                    "public override bool IsApplicable(GameObject target)",
                                                    "public override void Apply(GameObject target)");

                return true;
            }
            return false;
        }
    }
}
