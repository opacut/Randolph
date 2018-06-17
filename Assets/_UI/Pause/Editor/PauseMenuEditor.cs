using Randolph.Core;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PauseMenu))]
public class PauseMenuEditor : Editor {

    PauseMenu pauseMenu;

    readonly string[] toolbarItems = {"Pause", "Settings", "Map"};
    SerializedProperty toolbarIdx;

    //! Pause
    SerializedProperty pauseMenuControls;
    SerializedProperty pauseUI;
    SerializedProperty menuScene;

    //! Settings
    SerializedProperty settingsUI;

    //! Map
    SerializedProperty mapUI;

    void OnEnable() {
        pauseMenu = (PauseMenu) target;
        toolbarIdx = serializedObject.FindProperty(nameof(PauseMenu.toolbarIdx));

        pauseMenuControls = serializedObject.FindProperty(nameof(PauseMenu.pauseMenuControls));
        pauseUI = serializedObject.FindProperty(nameof(PauseMenu.pauseUI));
        menuScene = serializedObject.FindProperty(nameof(PauseMenu.menuScene));

        settingsUI = serializedObject.FindProperty(nameof(PauseMenu.settingsUI));

        mapUI = serializedObject.FindProperty(nameof(PauseMenu.mapUI));
    }

    public override void OnInspectorGUI() {
        serializedObject.Update();
        ShowInspector();
        serializedObject.ApplyModifiedProperties();
    }

    void ShowInspector() {
        EditorMethods.DisplayScriptField(pauseMenu);
        EditorGUILayout.Space();
        int newToolbarIdx = GUILayout.Toolbar(toolbarIdx.intValue, toolbarItems);

        switch (newToolbarIdx) {
            case 0:
                ShowPauseInspector();
                break;
            case 1:
                ShowSettingsInspector();
                break;
            case 2:
                ShowMapInspector();
                break;
        }

        if (newToolbarIdx != toolbarIdx.intValue) {
            RefreshMenu(newToolbarIdx, toolbarIdx.intValue);
            toolbarIdx.intValue = newToolbarIdx;
        }
    }

    void ShowPauseInspector() {
        EditorGUILayout.PropertyField(pauseMenuControls);
        EditorGUILayout.PropertyField(pauseUI);
        EditorGUILayout.Space();
        EditorGUILayout.PropertyField(menuScene);
    }

    void ShowSettingsInspector() {
        EditorGUILayout.PropertyField(settingsUI);
    }

    void ShowMapInspector() {
        EditorGUILayout.PropertyField(mapUI);
    }

    /// <summary>Closes current menu and opens a new one.</summary>
    /// <param name="newToolbarIdx">The new toolbar index (menu from <see cref="toolbarItems"/> to open).</param>
    /// <param name="oldToolbarIdx">The old toolbar index from the previous Inspector repaint.</param>
    void RefreshMenu(int newToolbarIdx, int oldToolbarIdx) {
        if (pauseUI.objectReferenceValue == null ||
            settingsUI.objectReferenceValue == null ||
            mapUI.objectReferenceValue == null) {
            return;
        }

        if (oldToolbarIdx == 1) pauseMenu.CloseSettings();
        else if (oldToolbarIdx == 2) pauseMenu.CloseMap();

        if (newToolbarIdx == 1) pauseMenu.OpenSettings();
        else if (newToolbarIdx == 2) pauseMenu.OpenMap();
    }

}
