using UnityEngine;

using UnityEditor;


namespace Randolph.Core {
    [InitializeOnLoad]
    public static class PlaymodeInitializer {
        

        static PlaymodeInitializer() {
            // After opening Unity Editor or after each recompilation
            EditorApplication.playModeStateChanged += OnPlaymodeChanged;
        }

        static void OnPlaymodeChanged(PlayModeStateChange state) {
            // Debug.Log($"<b>{state}</b>");
        }

    }
}
