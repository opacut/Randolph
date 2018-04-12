using UnityEngine;
using UnityEditor;

namespace Randolph.Core {
    [CustomEditor(typeof(AudioPlayer))]
    public class AudioPlayerEditor : Editor {

        AudioPlayer audioPlayer;

        SerializedProperty musicSource;
        SerializedProperty soundSource;
        SerializedProperty pitchRange;
        SerializedProperty global;

        SerializedProperty audioListener;

        const int DecimalDigits = 2;
        static readonly Vector2 PitchLimits = new Vector2(0.75f, 1.25f);

        void OnEnable() {
            audioPlayer = (AudioPlayer) target;

            musicSource = serializedObject.FindProperty(nameof(musicSource));
            soundSource = serializedObject.FindProperty(nameof(soundSource));
            pitchRange = serializedObject.FindProperty(nameof(pitchRange));
            global = serializedObject.FindProperty(nameof(global));

            audioListener = serializedObject.FindProperty(nameof(audioListener));
        }

        public override void OnInspectorGUI() {
            serializedObject.Update();

            DisplayScriptField();
            DisplayAudioSourceFields();
            DisplayPitchRangeField();

            serializedObject.ApplyModifiedProperties();
        }

        void DisplayAudioSourceFields() {
            EditorGUILayout.PropertyField(musicSource, new GUIContent("Music Audio Source"));
            EditorGUILayout.PropertyField(soundSource, new GUIContent("Sound Audio Source"));

            EditorGUI.BeginDisabledGroup(true);
            if (audioListener.objectReferenceValue == null) audioListener.objectReferenceValue = FindObjectOfType<AudioListener>();
            Transform listener = (audioListener.objectReferenceValue as AudioListener)?.transform;
            EditorGUILayout.ObjectField(new GUIContent("Audio Listener", "The current scene's audio listener"),
                    listener, typeof(Transform), allowSceneObjects: true);
            EditorGUI.EndDisabledGroup();

            EditorGUILayout.HelpBox($"Sound volume {(global.boolValue ? "does not fade" : "fades")} with distance. Music is always global.", MessageType.None);

            int selected = (global.boolValue) ? 0 : 1;
            selected = GUILayout.Toolbar(selected, new[] {new GUIContent("Global"), new GUIContent("Local")});
            global.boolValue = (selected == 0);
        }

        void DisplayPitchRangeField() {
            Vector2 oldValue = pitchRange.vector2Value;
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(new GUIContent("Pitch Range", "The range for the random pitch change when a sound is played."), GUILayout.MaxWidth(EditorGUIUtility.labelWidth));
            GUILayout.FlexibleSpace();
            EditorGUILayout.DelayedFloatField(oldValue.x.RoundToDigits(DecimalDigits), GUILayout.ExpandWidth(false), GUILayout.MaxWidth(50));
            EditorGUILayout.MinMaxSlider(ref oldValue.x, ref oldValue.y, PitchLimits.x, PitchLimits.y, GUILayout.ExpandWidth(true));
            EditorGUILayout.DelayedFloatField(oldValue.y.RoundToDigits(DecimalDigits), GUILayout.ExpandWidth(false), GUILayout.MaxWidth(50));
            GUILayout.Space(20);
            EditorGUILayout.EndHorizontal();
            pitchRange.vector2Value = oldValue;
        }

        void DisplayScriptField() {
            EditorGUI.BeginDisabledGroup(true);
            MonoScript script = MonoScript.FromMonoBehaviour(audioPlayer);
            script = EditorGUILayout.ObjectField("Script", script, typeof(MonoScript), false) as MonoScript;
            EditorGUI.EndDisabledGroup();
        }

    }
}
