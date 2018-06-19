using System.Collections.Generic;
using Randolph.Core;
using UnityEditor;
using UnityEngine;

namespace Randolph.Levels {
    [CustomEditor(typeof(Level))]
    public class LevelEditor : Editor {

        Level level;
        SerializedProperty levelMusic;

        SerializedProperty areaFold;

        void OnEnable() {
            level = target as Level;

            levelMusic = serializedObject.FindProperty(nameof(Level.levelMusic));
            areaFold = serializedObject.FindProperty(nameof(Level.areaFold));
        }

        public override void OnInspectorGUI() {
            serializedObject.Update();
            ShowInspector();
            serializedObject.ApplyModifiedProperties();
        }

        void ShowInspector() {
            EditorMethods.DisplayScriptField(level);

            EditorMethods.HeaderField("Level");
            EditorGUILayout.PropertyField(levelMusic, new GUIContent("Level Music", "Music to play and loop in this level"));

            areaFold.boolValue = EditorGUILayout.Foldout(areaFold.boolValue, "Areas");
            if (areaFold.boolValue) {
                EditorGUI.indentLevel++;
                EditorGUI.BeginDisabledGroup(true);
                var areas = new List<Area>();
                level.transform.GetComponentsInChildren(areas);
                areas.ForEach(area => EditorGUILayout.ObjectField(area.gameObject.name, area, typeof(Area), true));
                EditorGUI.EndDisabledGroup();
                EditorGUI.indentLevel--;
            }

        }

    }
}
