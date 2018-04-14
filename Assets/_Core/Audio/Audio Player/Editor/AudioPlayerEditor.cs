using Randolph.Levels;
using UnityEngine;
using UnityEditor;

namespace Randolph.Core {
    [CustomEditor(typeof(AudioPlayer))]
    public class AudioPlayerEditor : Editor {

        AudioPlayer audioPlayer;

        SerializedProperty musicSource;
        SerializedProperty soundSource;
        SerializedProperty pitchRange;
        SerializedProperty audioPlayerMode;

        SerializedProperty audioListener;

        SerializedProperty currentArea;

        const int DecimalDigits = 2;
        static readonly Vector2 PitchLimits = new Vector2(0.75f, 1.25f);

        void OnEnable() {
            audioPlayer = (AudioPlayer) target;

            musicSource = serializedObject.FindProperty(nameof(musicSource));
            soundSource = serializedObject.FindProperty(nameof(soundSource));
            pitchRange = serializedObject.FindProperty(nameof(pitchRange));
            audioPlayerMode = serializedObject.FindProperty(nameof(audioPlayerMode));

            audioListener = serializedObject.FindProperty(nameof(audioListener));

            currentArea = serializedObject.FindProperty(nameof(currentArea));
        }

        public override void OnInspectorGUI() {
            serializedObject.Update();

            EditorMethods.DisplayScriptField(audioPlayer);
            DisplayAudioSourceFields();
            DisplayAudioListener();
            DisplayAudioPlayerModeSwitch();
            DisplayPitchRangeField();

            serializedObject.ApplyModifiedProperties();
        }

        void DisplayAudioSourceFields() {
            EditorGUILayout.PropertyField(musicSource, new GUIContent("Music Audio Source"));
            EditorGUILayout.PropertyField(soundSource, new GUIContent("Sound Audio Source"));
        }

        void DisplayAudioPlayerModeSwitch() {
            string helpMessage;
            var messageType = MessageType.None;
            switch (audioPlayerMode.enumValueIndex) {
                case (int) AudioPlayer.AudioPlayerMode.Local:
                    helpMessage = "Sounds are only heard within a certain distance from their source. Music is always global.";
                    break;
                case (int) AudioPlayer.AudioPlayerMode.Rooms:
                    helpMessage = "Sound volume does not fade with distance, but only those from the current room are played. Music is always global.";
                    break;
                case (int) AudioPlayer.AudioPlayerMode.Global:
                    helpMessage = "No sound or music volume fades with distance from the source. Therefore, ALL sounds in the scene play at a full volume.";
                    break;
                default:
                    helpMessage = "Unknown audio player state!";
                    messageType = MessageType.Error;
                    break;
            }
            EditorGUILayout.HelpBox(helpMessage, messageType);
            EditorGUI.BeginDisabledGroup(Application.isPlaying);
            audioPlayerMode.enumValueIndex = GUILayout.Toolbar(audioPlayerMode.enumValueIndex, audioPlayerMode.enumDisplayNames);
            EditorGUI.EndDisabledGroup();
            if (audioPlayerMode.enumValueIndex == (int) AudioPlayer.AudioPlayerMode.Rooms) {
                EditorGUI.BeginDisabledGroup(true);
                EditorGUILayout.ObjectField(new GUIContent("Current Area", "The current area allowed to play sounds."),
                        (currentArea.objectReferenceValue as Area)?.transform, typeof(Transform), allowSceneObjects: true);
                //EditorGUILayout.PropertyField(currentArea, new GUIContent("Current Area", "The current area allowed to play sounds."));
                EditorGUI.EndDisabledGroup();
            }
        }

        void DisplayAudioListener() {
            EditorGUI.BeginDisabledGroup(true);
            if (audioListener.objectReferenceValue == null) audioListener.objectReferenceValue = FindObjectOfType<AudioListener>();
            Transform listener = (audioListener.objectReferenceValue as AudioListener)?.transform;
            EditorGUILayout.ObjectField(new GUIContent("Audio Listener", "The current scene's audio listener"),
                    listener, typeof(Transform), allowSceneObjects: true);
            EditorGUI.EndDisabledGroup();
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

    }
}
