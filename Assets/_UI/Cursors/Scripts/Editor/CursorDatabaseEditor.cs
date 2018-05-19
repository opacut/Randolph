using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using System.Linq;
using Randolph.Core;
using UnityEngine;

namespace Randolph.UI {
    [CustomEditor(typeof(CursorDatabase))]
    public class CursorDatabaseEditor : Editor {

        CursorDatabase cursorDatabase;

        SerializedProperty cursors;
        string[] cursorNames;
        bool changed;

        void OnEnable() {
            cursorDatabase = target as CursorDatabase;
            cursors = serializedObject.FindProperty(nameof(cursors));
            cursorNames = Enum.GetNames(typeof(Cursors));
            changed = !cursorNames.SequenceEqual(GetCursorNames());
        }

        public override void OnInspectorGUI() {
            serializedObject.Update();
            EditorMethods.DisplayScriptField(target as CursorDatabase);
            EditorGUI.BeginDisabledGroup(EditorApplication.isCompiling);

            if (GUILayout.Button("Create Greyscale Textures")) {
                MakeGreyscaleTextures();
            }

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
                string folderPath = EditorMethods.GetFolderPath(MonoScript.FromScriptableObject(cursorDatabase));
                GenerateFiles.GenerateEnum(nameof(Cursors), folderPath, names, $"{nameof(Randolph)}.{nameof(UI)}");
                cursorNames = names;
            }
            EditorGUI.EndDisabledGroup();
        }

        string[] GetCursorNames() {
            var names = new List<string>();
            for (int i = 0; i < cursors.arraySize; i++) {
                SerializedProperty gameCursor = cursors.GetArrayElementAtIndex(i);
                string cursorName = gameCursor.FindPropertyRelative(nameof(GameCursor.name)).stringValue;
                names.Add(cursorName);
            }
            return names.ToArray();
        }

        void MakeGreyscaleTextures() {
            for (int i = 0; i < cursors.arraySize; i++) {
                SerializedProperty gameCursor = cursors.GetArrayElementAtIndex(i);
                SerializedProperty disabledTexture = gameCursor.FindPropertyRelative(nameof(GameCursor.disabledCursor));
                var overTexture = gameCursor.FindPropertyRelative(nameof(GameCursor.overCursor)).objectReferenceValue as Texture2D;

                string folderPath = EditorMethods.GetFolderPath(overTexture);
                string textureName = $"{overTexture?.name} GREY";
                Texture2D newTexture = overTexture.ToGreyscale();
                newTexture.name = textureName;
                byte[] bytes = newTexture.EncodeToPNG();
                DestroyImmediate(newTexture);

                string path = $"{Application.dataPath.TrimEnd("/Assets")}/{folderPath}/{textureName}.png";
                File.WriteAllBytes(path, bytes);

                AssetDatabase.Refresh();

                var texture = AssetDatabase.LoadAssetAtPath<Texture2D>($"{folderPath}/{textureName}.png");
                disabledTexture.objectReferenceValue = texture;
            }
        }

    }
}
