using Randolph.Core;
using UnityEditor;

namespace Randolph.Levels {
    [CustomEditor(typeof(Area))]
    public class AreaEditor : Editor {

        Area area;

        void OnEnable() {
            area = (Area) target;
            area.RefreshCameraData();
        }

        public override void OnInspectorGUI() {
            serializedObject.Update();
            EditorMethods.DisplayScriptField(area);
            NameCheck();
            if (NameCheck()) DisplayCorrespondingRoom();
            else EditorGUILayout.HelpBox("The name doesn't contain a corresponding Camera Room number.", MessageType.Error);
            serializedObject.ApplyModifiedProperties();
        }

        bool NameCheck() {
            var areaIndex = Methods.GetNumberFromString(area.name);
            return areaIndex >= 0;
        }

        void DisplayCorrespondingRoom() {
            if (area.MatchingCameraRoom == null) { 
                EditorGUILayout.HelpBox("No corresponding room found. Make sure the Area ID matches a corresponding Camera Room ID.", MessageType.Error);
                return;
            }
            EditorGUI.BeginDisabledGroup(true);
            string ID = area.MatchingCameraRoom.ID;
            // TODO: Duplicate check
            EditorGUILayout.TextField("Corresponding room", ID);
            EditorGUI.EndDisabledGroup();
        }


    }
}
