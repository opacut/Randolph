using UnityEditor;
using UnityEngine;
using Randolph.Core;

namespace Randolph.Environment {
    [CustomEditor(typeof(SpikeTrap))]
    [CanEditMultipleObjects]
    public class SpikeTrapEditor : Editor {

        SerializedProperty Up;
        SerializedProperty activeSprite;
        SerializedProperty inactiveSprite;

        void OnEnable() { 
            Up = serializedObject.FindProperty(nameof(Up));
            activeSprite = serializedObject.FindProperty(nameof(activeSprite));
            inactiveSprite = serializedObject.FindProperty(nameof(inactiveSprite));
        }

        public override void OnInspectorGUI() {
            serializedObject.Update();

            DisplayActiveBoolField();
            DisplaySpriteFields();

            serializedObject.ApplyModifiedProperties();
        }

        void DisplayActiveBoolField() {
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(Up, new GUIContent("Up"));
            if (EditorGUI.EndChangeCheck()) MatchSprite();
        }

        void DisplaySpriteFields() {
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(activeSprite, new GUIContent("Active Sprite"));
            EditorGUILayout.PropertyField(inactiveSprite, new GUIContent("Inactive Sprite"));
            if (EditorGUI.EndChangeCheck()) MatchSprite(excludePrefabs: false);
        }

        void MatchSprite(bool excludePrefabs = true) {
            if (!excludePrefabs || !EditorMethods.IsPrefab(target)) {
                foreach (Object spikeTrap in targets) {
                    var spriteRenderer = ((SpikeTrap) spikeTrap).GetComponent<SpriteRenderer>();

                    if (Up.boolValue) spriteRenderer.sprite = activeSprite.objectReferenceValue as Sprite;
                    else spriteRenderer.sprite = inactiveSprite.objectReferenceValue as Sprite;
                }
            }
        }

    }
}
