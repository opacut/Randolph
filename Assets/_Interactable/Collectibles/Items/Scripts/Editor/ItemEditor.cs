using System;
using System.IO;

using UnityEditor;

using UnityEngine;

namespace Randolph.Interactable {
    [CustomEditor(typeof(Item))]
    public class ItemEditor : Editor {

        SerializedProperty initialized;

        void OnEnable() {
            initialized = serializedObject.FindProperty(nameof(initialized));
        }

        public override void OnInspectorGUI() {
            serializedObject.Update();

            if (!ItemIsInitialized()) {
                DisplayNotItemDatabaseWarning();
                DisplayInitializeButton();
            }

            DisplayScriptField();

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

        void DisplayScriptField() {
            EditorGUI.BeginDisabledGroup(true);
            MonoScript script = MonoScript.FromScriptableObject(target as Item);
            script = EditorGUILayout.ObjectField(script, typeof(MonoScript), false) as MonoScript;
            EditorGUI.EndDisabledGroup();
        }

        void Initialize() {            
            initialized.boolValue = true;
            
            //! Create folder
            string assetPath = AssetDatabase.GetAssetPath(Selection.activeObject);
            string parentFolder = Path.GetDirectoryName(assetPath)?.Replace(@"\\", "/").Replace(@"\", "/");
            AssetDatabase.CreateFolder(parentFolder, name);
            string newFolder = $"{parentFolder}/{name}";
            AssetDatabase.MoveAsset(assetPath, $"{newFolder}/{name}.asset");            

            //! Create prefab
            var itemObject = new GameObject(name);
            itemObject.AddComponent<SpriteRenderer>();
            itemObject.AddComponent<CapsuleCollider2D>();
            if (ItemExtensions.CreateItemScript(name, newFolder)) {
                itemObject.AddComponent(Type.GetType(name));
            }
            string prefabPath = $"{newFolder}/{name}.prefab";
            PrefabUtility.CreatePrefab(prefabPath, itemObject);

            //! Add itself to the database
            var item = target as Item;
            if (item && !ItemDatabase.itemDatabase.ContainsItem(item)) {
                ItemDatabase.itemDatabase.AddItem(item);
            }

        }

    }
}
