using System;
using System.Collections.Generic;
using UnityEditor;
using System.Linq;
using Randolph.Core;
using UnityEngine;

namespace Randolph.UI {
    [CustomEditor(typeof(CursorDatabase))]
    public class CursorDatabaseEditor : Editor {

        SerializedProperty cursors;
        string[] cursorNames;
        bool changed;

        void OnEnable() {
            cursors = serializedObject.FindProperty(nameof(cursors));
            cursorNames = Enum.GetNames(typeof(Cursors));
            changed = !cursorNames.SequenceEqual(GetCursorNames());
        }

        public override void OnInspectorGUI() {
            serializedObject.Update();
            EditorMethods.DisplayScriptField(target as CursorDatabase);
            EditorGUI.BeginDisabledGroup(EditorApplication.isCompiling);

            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(cursors, true);
            string[] newNames = GetCursorNames();
            if (EditorGUI.EndChangeCheck()) {
                changed = !cursorNames.SequenceEqual(newNames);
            }
            EditorGUILayout.Space();            
            if (changed) {
                EditorGUILayout.HelpBox("The Cursors enum needs to be refreshed to match the database.", MessageType.Warning);
            } else {
                EditorGUILayout.HelpBox("The Cursors enum is up to date.", MessageType.None);
            }
            DisplayRefreshButton(newNames, !changed);

            EditorGUI.EndDisabledGroup();
            serializedObject.ApplyModifiedProperties();
        } 

        void DisplayRefreshButton(string[] names, bool disable = false) {
            var buttonContent = new GUIContent("Refresh Cursors", "Creates an enum file with cursor names.");            
            if (names.Length != names.Distinct().Count()) {
                disable = true;
                EditorGUILayout.HelpBox("Each cursor must have a distinct name.", MessageType.Error);
            }
            if (names.Length != names.Count(Methods.IsValidVariableName)) {
                disable = true;
                EditorGUILayout.HelpBox("Some cursor names aren't valid identifier names.", MessageType.Error);
            }
            EditorGUI.BeginDisabledGroup(disable);
            if (GUILayout.Button(buttonContent)) {
                GenerateFiles.GenerateEnum("Cursors", "Assets/_UI/Cursors/Scripts", names, "Randolph.UI");
                cursorNames = names;
            }
            EditorGUI.EndDisabledGroup();
        }

        string[] GetCursorNames() {
            var names = new List<string>();
            for (int i = 0; i < cursors.arraySize; i++) {
                SerializedProperty gameCursor = cursors.GetArrayElementAtIndex(i);
                string cursorName = gameCursor.FindPropertyRelative("name").stringValue;
                names.Add(cursorName);
            }
            return names.ToArray();
        }

    }
}
