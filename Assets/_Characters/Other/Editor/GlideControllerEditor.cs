using UnityEditor;

using UnityEditorInternal;

using UnityEngine;

namespace Randolph.Characters {
    [CustomEditor(typeof(Glider))]
    public class GlideControllerEditor : Editor {

        ReorderableList destinationsList;
        SerializedProperty straightLines;
        Glider glider;

        SerializedProperty destinations;
        SerializedProperty speed;
        SerializedProperty movesFromStart;
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
            movesFromStart = serializedObject.FindProperty(nameof(movesFromStart));
            loop = serializedObject.FindProperty(nameof(loop));
            continuous = serializedObject.FindProperty(nameof(continuous));
            straightLines = serializedObject.FindProperty(nameof(straightLines));
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
            EditorGUILayout.Space();
            StraightLinesToggle();
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

                        EditorGUI.BeginDisabledGroup(straightLines.boolValue == false);
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
            movesFromStart.boolValue = EditorGUILayout.ToggleLeft(new GUIContent("Moves from start", "Does the glider move without being touched first?"), movesFromStart.boolValue);
            loop.boolValue = EditorGUILayout.ToggleLeft(new GUIContent("Loop", "Return to the initial position from the last one?"), loop.boolValue);
            continuous.boolValue = EditorGUILayout.ToggleLeft(new GUIContent("Continuous flight", "Stop at each destination?"), continuous.boolValue);
        }

        void StraightLinesToggle() {
            bool currentLines = straightLines.boolValue;

            bool newLines = EditorGUILayout.ToggleLeft(new GUIContent("Straight lines", "Allow movement only along straight lines?"), straightLines.boolValue);
            if (!currentLines && newLines) {
                if (EditorUtility.DisplayDialog("Only Allow Straight Lines", "This is potentially a destructive action, as it will snap destinations.", "Confirm", "Cancel")) {
                    straightLines.boolValue = true;
                }
            } else {
                straightLines.boolValue = newLines;
            }

            if (!Application.isPlaying && straightLines.boolValue) {
                StraightenLines();
            }
        }

        void StraightenLines() {
            if (destinationsList != null && destinationsList.serializedProperty.arraySize > 0) {
                Vector2 previous = glider.transform.position;
                for (int index = 0; index < destinationsList.serializedProperty.arraySize; index++) {
                    var destination = destinationsList.serializedProperty.GetArrayElementAtIndex(index);
                    Vector2 current = destination.vector2Value;                                        
                    destination.vector2Value = AlignPoints(current, previous);
                    previous = destination.vector2Value;
                }

                if (loop.boolValue) glider.transform.position = AlignPoints(glider.transform.position, previous);
            }
        }

        Vector2 AlignPoints(Vector2 current, Vector2 previous) {
            Vector2 difference = previous - current;
            bool xIsMin = Mathf.Abs(difference.x) < Mathf.Abs(difference.y);
            if (xIsMin) current.x = previous.x;
            else current.y = previous.y;

            return current;
        }

    }
}
