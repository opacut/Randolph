using System.Collections.Generic;
using System.Linq;

using UnityEngine;

using UnityEditor;

using UnityEngine.WSA;

using Object = UnityEngine.Object;

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

}
