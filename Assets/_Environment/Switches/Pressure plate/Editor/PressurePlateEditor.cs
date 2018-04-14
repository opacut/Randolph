using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using Randolph.Levels;

namespace Randolph.Environment {
    [CustomEditor(typeof(PressurePlate))]
    public class PressurePlateEditor : Editor {

        PressurePlate pressurePlate;

        SerializedProperty Spikes;

        void OnEnable() {
            pressurePlate = (PressurePlate) target;
            Spikes = serializedObject.FindProperty(nameof(Spikes));
        }

        public override void OnInspectorGUI() {
            serializedObject.Update();

            DrawDefaultInspector();

            // This must be *after* DrawDefaultInspector, or the changes will be overwritten
            GetAreaSpikesButton();

            serializedObject.ApplyModifiedProperties();
        }

        void GetAreaSpikesButton() {
            if (GUILayout.Button("Get all spikes in the current area")) {
                var area = pressurePlate.GetComponentInParent<Area>();
                List<SpikeTrap> areaSpikes = area.GetAreaSpikes();
                Spikes.ClearArray();
                Spikes.arraySize = areaSpikes.Count;
                for (int i = 0; i < areaSpikes.Count; i++) {
                    Spikes.GetArrayElementAtIndex(i).objectReferenceValue = areaSpikes[i];
                }
            }
        }

    }
}
