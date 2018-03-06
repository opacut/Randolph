using Randolph.UI;
using UnityEditor;

[CustomEditor(typeof(LoadLevelOnClick), true)]
public class ScenePickerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        var picker = target as LoadLevelOnClick;
        if (picker != null) {
            var oldScene = AssetDatabase.LoadAssetAtPath<SceneAsset>(picker.sceneToLoad);

            serializedObject.Update();

            EditorGUI.BeginChangeCheck();
            var newScene = EditorGUILayout.ObjectField("Scene", oldScene, typeof(SceneAsset), false) as SceneAsset;

            if (EditorGUI.EndChangeCheck())
            {
                string newPath = AssetDatabase.GetAssetPath(newScene);
                var scenePathProperty = serializedObject.FindProperty("sceneToLoad");
                scenePathProperty.stringValue = newPath;
            }
        }

        serializedObject.ApplyModifiedProperties();
    }
}
