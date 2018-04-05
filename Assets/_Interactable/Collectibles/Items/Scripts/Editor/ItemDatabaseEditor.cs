using UnityEngine;

using UnityEditor;

using UnityEditorInternal;
using Randolph.Core;

namespace Randolph.Interactable {
    [CustomEditor(typeof(ItemDatabase))]
    public class ItemDatabaseEditor : Editor {

        ReorderableList numberedItemList;

        SerializedProperty defaultBackground;
        SerializedProperty activeBackground;
        SerializedProperty passiveBackground;
        bool spriteFold = true;

        void OnEnable() {
            InitializeReorderableList();
            InitializeSpriteFields();
        }

        void InitializeSpriteFields() {
            defaultBackground = serializedObject.FindProperty(nameof(defaultBackground));
            activeBackground = serializedObject.FindProperty(nameof(activeBackground));
            passiveBackground = serializedObject.FindProperty(nameof(passiveBackground));
        }

        void InitializeReorderableList() {
            numberedItemList = new ReorderableList(serializedObject,
                    serializedObject.FindProperty(nameof(numberedItemList)),
                    true, true, true, true);

            numberedItemList.drawHeaderCallback = (Rect rect) => { EditorGUI.LabelField(rect, "Item database"); };

            numberedItemList.drawElementCallback =
                    (Rect rect, int index, bool isActive, bool isFocused) => {
                        SerializedProperty element = numberedItemList.serializedProperty.GetArrayElementAtIndex(index);
                        rect.y += 2;

                        EditorGUI.BeginDisabledGroup(true);
                        var itemIdProperty = element.FindPropertyRelative("id");
                        itemIdProperty.intValue = index; // Item index = item position in the reorderable list
                        EditorGUI.PropertyField(
                                new Rect(rect.x, rect.y, 20, EditorGUIUtility.singleLineHeight),
                                itemIdProperty, GUIContent.none);
                        EditorGUI.EndDisabledGroup();

                        EditorGUI.PropertyField(
                                new Rect(rect.x + 30, rect.y, rect.width - (30), EditorGUIUtility.singleLineHeight),
                                element.FindPropertyRelative("item"), GUIContent.none);
                    };
        }

        public override void OnInspectorGUI() {
            serializedObject.Update();

            DisplayScriptField();
            DisplayPruneButton();
            DisplaySpriteFields();
            numberedItemList.DoLayoutList();

            serializedObject.ApplyModifiedProperties();
        }

        void DisplaySpriteFields() {
            spriteFold = EditorGUILayout.Foldout(spriteFold, "Background sprites");
            if (spriteFold) {
                EditorGUILayout.PropertyField(defaultBackground, new GUIContent("Default background"));
                EditorGUILayout.PropertyField(activeBackground, new GUIContent("Active background"));
                EditorGUILayout.PropertyField(passiveBackground, new GUIContent("Passive background"));
            }
        }

        void DisplayPruneButton() {
            var database = (ItemDatabase) serializedObject.targetObject;
            bool invalidItems = database.PruneCheck();
            Color originalColor = GUI.backgroundColor;


            EditorGUILayout.BeginHorizontal();
            GUI.backgroundColor = (invalidItems) ? Color.yellow : originalColor;

            // GUILayout.FlexibleSpace();
            if (GUILayout.Button(
                    new GUIContent("Prune", "Removes all unnecessary item references from the database, such as duplicate or empty items."))
            ) {
                database.Prune();
            }

            GUI.backgroundColor = originalColor;
            EditorGUILayout.EndHorizontal();
        }

        void DisplayScriptField() {
            EditorGUI.BeginDisabledGroup(true);
            MonoScript script = MonoScript.FromScriptableObject(target as ItemDatabase);
            script = EditorGUILayout.ObjectField(script, typeof(MonoScript), false) as MonoScript;
            EditorGUI.EndDisabledGroup();
        }

    }
}
