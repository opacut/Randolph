/*
using UnityEngine;
using UnityEditor;
using Core.Managers;

[CustomEditor(typeof(GetLevelManager), true)]
public class ScenePickerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        var picker = target as GetLevelManager;
        var oldScene = AssetDatabase.LoadAssetAtPath<SceneAsset>(picker.sceneToLoad);

        serializedObject.Update();

        EditorGUI.BeginChangeCheck();
        var newScene = EditorGUILayout.ObjectField("Scene", oldScene, typeof(SceneAsset), false) as SceneAsset;

        if (EditorGUI.EndChangeCheck())
        {
            var newPath = AssetDatabase.GetAssetPath(newScene);
            var scenePathProperty = serializedObject.FindProperty("sceneToLoad");
            scenePathProperty.stringValue = newPath;
        }
        serializedObject.ApplyModifiedProperties();
    }
}
*/