using System;
using System.Collections;
using UnityEngine;
using UnityEditor;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using Object = UnityEngine.Object;

namespace Randolph.Core {
    public class EditorMethods {

        /// <summary>Finds all prefabs containing a specific component in a given folder and its subfolders.</summary>
        /// <typeparam name="T">The desired component.</typeparam>
        /// <param name="relativeFolderPaths">Paths relative to the project folder to start a recursive search from.</param>
        public static List<GameObject> FindPrefabsWithComponent<T>(string[] relativeFolderPaths = null) where T : Component {
            return FindPrefabs(relativeFolderPaths).Where(prefab => prefab.GetComponent<T>() != null).ToList();
        }

        /// <summary>Finds all components of prefabs in a given folder and its subfolders.</summary>
        /// <typeparam name="T">The desired component.</typeparam>
        /// <param name="relativeFolderPaths">Paths relative to the project folder to start a recursive search from.</param>    
        public static List<T> FindComponentsOfPrefabs<T>(string[] relativeFolderPaths = null) where T : Component {
            return FindPrefabs(relativeFolderPaths).Select(prefab => prefab.GetComponent<T>()).Where(component => component != null).ToList();
        }

        /// <summary>
        /// Returns all prefabs in a folder and its subfolders. Optionally, folders to search in can be provided.
        /// </summary>
        /// <param name="relativeFolderPaths">Paths relative to the project folder to start the search in.</param>
        public static List<GameObject> FindPrefabs(string[] relativeFolderPaths = null) {
            string[] prefabsGUIDs = (relativeFolderPaths == null)
                    ? AssetDatabase.FindAssets("t:Prefab")
                    : AssetDatabase.FindAssets("t:Prefab", relativeFolderPaths);

            return prefabsGUIDs.Select(guid => AssetDatabase.LoadAssetAtPath<GameObject>(AssetDatabase.GUIDToAssetPath(guid))).ToList();
        }

        /// <summary>Displays a default readonly script field inside the editor Inspector.</summary>
        public static void DisplayScriptField(MonoBehaviour monoBehaviour) {
            EditorGUI.BeginDisabledGroup(true);
            MonoScript script = MonoScript.FromMonoBehaviour(monoBehaviour);
            script = EditorGUILayout.ObjectField("Script", script, typeof(MonoScript), false) as MonoScript;
            EditorGUI.EndDisabledGroup();
        }

        /// <summary>Displays a default readonly script field inside the editor Inspector.</summary>
        public static void DisplayScriptField(ScriptableObject scriptableObject) {
            EditorGUI.BeginDisabledGroup(true);
            MonoScript script = MonoScript.FromScriptableObject(scriptableObject);
            script = EditorGUILayout.ObjectField("Script", script, typeof(MonoScript), false) as MonoScript;
            EditorGUI.EndDisabledGroup();
        }

        /// <summary>Checks whether a target object is a prefab (e.g. in an Inspector).</summary>
        public static bool IsPrefab(Object target) {
            // return PrefabUtility.GetPrefabParent(target) == null && PrefabUtility.GetPrefabObject(target) != null;
            return PrefabUtility.GetPrefabType(target) == PrefabType.Prefab;
        }
        
        /// <summary>Returns path to the folder containing an asset.</summary>        
        public static string GetFolderPath(Object assetObject) {
            string filePath = AssetDatabase.GetAssetPath(assetObject);
            string folderPath = Path.GetDirectoryName(filePath)?.Replace("\\", "/");
            return folderPath;
        }

        /// <summary>Make a bold label field with space above.</summary>
        /// <param name="label">The header text.</param>
        public static void HeaderField(string label) {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField(label, new GUIStyle(GUI.skin.label) { fontStyle = FontStyle.Bold });
        }

    }
}
