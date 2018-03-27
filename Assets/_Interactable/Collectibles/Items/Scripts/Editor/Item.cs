using System.IO;

using UnityEditor;

using UnityEngine;

namespace Randolph.Interactable {
	[CreateAssetMenu(fileName = "Item", menuName = "Randolph/Inventory/Item", order = 33)]
	public class Item : ScriptableObject {
		
		[SerializeField] GameObject prefab;
        
	    public void Initialize() {
	        // TODO: Create folder + empty prefab + Add itself to the database	        
	        string assetPath = AssetDatabase.GetAssetPath(Selection.activeObject);
            string parentFolder = Path.GetDirectoryName(assetPath);

            AssetDatabase.CreateFolder(parentFolder, name);
	        string newFolder = $"{parentFolder}/{name}";
            AssetDatabase.MoveAsset(assetPath, $"{newFolder}/{name}.asset");

            ItemExtensions.CreateItemScript(name, newFolder);

	        var itemPrefab = (GameObject) PrefabUtility.CreateEmptyPrefab($"{newFolder}/{name}.prefab");
	        itemPrefab.AddComponent<SpriteRenderer>();
	        itemPrefab.AddComponent<CapsuleCollider2D>();
	    }

	}
}