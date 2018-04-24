using System.Collections.Generic;

using Randolph.Environment;
using Randolph.Levels;

using UnityEditor;

using UnityEngine;

[CustomEditor(typeof(Switch))]
public class SwitchEditor : Editor {

    Switch @switch;

    SerializedProperty Spikes;

    void OnEnable() {
        @switch = (Switch) target;
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
            var area = @switch.GetComponentInParent<Area>();
            List<SpikeTrap> areaSpikes = area.GetAreaSpikes();
            Spikes.ClearArray();
            Spikes.arraySize = areaSpikes.Count;
            for (int i = 0; i < areaSpikes.Count; i++) {
                Spikes.GetArrayElementAtIndex(i).objectReferenceValue = areaSpikes[i];
            }
        }
    }

}
