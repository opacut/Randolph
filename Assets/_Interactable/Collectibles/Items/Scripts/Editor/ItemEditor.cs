using UnityEditor;

using UnityEngine;

namespace Randolph.Interactable {
    [CustomEditor(typeof(Item))]
    public class ItemEditor : Editor {

        void OnEnable() { }

        public override void OnInspectorGUI() {
            serializedObject.Update();

            var item = target as Item;
            
            if (!ItemIsInitialized(item)) {
                DisplayNotItemDatabaseWarning();
                DisplayInitializeButton(item);
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

        bool ItemIsInitialized(Item item) {
            if (!item) return false;
            return ItemDatabase.itemDatabase.ContainsItem(item);
        }

        void DisplayInitializeButton(Item item) {
            Color originalColor = GUI.backgroundColor;
            GUI.backgroundColor = Color.yellow;
            if (GUILayout.Button("Initialize item")) {
                item.Initialize();
            }

            GUI.backgroundColor = originalColor;
        }

        void DisplayScriptField() {
            EditorGUI.BeginDisabledGroup(true);
            MonoScript script = MonoScript.FromScriptableObject(target as Item);
            script = EditorGUILayout.ObjectField(script, typeof(MonoScript), false) as MonoScript;
            EditorGUI.EndDisabledGroup();
        }

    }
}
