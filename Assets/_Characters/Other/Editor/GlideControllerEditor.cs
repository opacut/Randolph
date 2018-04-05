using UnityEditor;

using UnityEditorInternal;

using UnityEngine;

namespace Randolph.Characters {
    [CustomEditor(typeof(Glider))]
    public class GlideControllerEditor : Editor {

        ReorderableList destinationsList;
        Glider glider;

        SerializedProperty destinations;
        SerializedProperty speed;
        SerializedProperty loop;
        SerializedProperty continuous;

        void OnEnable() {
            glider = (Glider) target;
            FindProperties();
            InitializeReorderableList();
        }

        void FindProperties() {
            destinations = serializedObject.FindProperty(nameof(destinations));
            speed = serializedObject.FindProperty(nameof(speed));
            loop = serializedObject.FindProperty(nameof(loop));
            continuous = serializedObject.FindProperty(nameof(continuous));
        }

        protected virtual void OnSceneGUI() {
            serializedObject.Update();

            for (int i = 0; i < destinationsList.count; i++) {
                EditorGUI.BeginChangeCheck();
                Vector3 newTargetPosition = Handles.PositionHandle(destinationsList.serializedProperty.GetArrayElementAtIndex(i).vector2Value, Quaternion.identity);

                if (EditorGUI.EndChangeCheck()) {
                    Undo.RecordObject(glider, "Move Glide Destination");
                    destinationsList.serializedProperty.GetArrayElementAtIndex(i).vector2Value = newTargetPosition;
                }
            }

            serializedObject.ApplyModifiedProperties();
        }

        public override void OnInspectorGUI() {
            serializedObject.Update();

            DisplayScriptField();
            EditorGUILayout.PropertyField(speed);
            DisplayToggles();
            destinationsList.DoLayoutList();
            
            serializedObject.ApplyModifiedProperties();
        }


        void InitializeReorderableList() {
            destinationsList = new ReorderableList(serializedObject,
                    destinations,
                    true, true, true, true);

            destinationsList.drawHeaderCallback = (Rect rect) => { EditorGUI.LabelField(rect, "Glider destinations"); };

            destinationsList.drawElementCallback =
                    (Rect rect, int index, bool isActive, bool isFocused) => {
                        SerializedProperty element = destinationsList.serializedProperty.GetArrayElementAtIndex(index);
                        rect.y += 2;

                        EditorGUI.BeginDisabledGroup(true);
                        EditorGUI.PropertyField(
                                new Rect(rect.x + 30, rect.y, rect.width - (30), EditorGUIUtility.singleLineHeight),
                                element, GUIContent.none);
                        EditorGUI.EndDisabledGroup();
                    };
        }

        void DisplayScriptField() {
            EditorGUI.BeginDisabledGroup(true);
            MonoScript script = MonoScript.FromMonoBehaviour(target as Glider);
            script = EditorGUILayout.ObjectField(script, typeof(MonoScript), false) as MonoScript;
            EditorGUI.EndDisabledGroup();
        }

        void DisplayToggles() {
            loop.boolValue = EditorGUILayout.ToggleLeft(new GUIContent("Loop", "Return to the initial position from the last one?"), loop.boolValue);
            continuous.boolValue = EditorGUILayout.ToggleLeft(new GUIContent("Continuous flight", "Stop at each destination?"), continuous.boolValue);
        }

    }
}
