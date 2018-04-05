using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Randolph.Tiles {
	public class EditorTile : ConnectedTile {
		public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData) {
			base.GetTileData(position, tilemap, ref tileData);
			tileData.colliderType = ColliderType.Grid;
			if (Application.isEditor && !Application.isPlaying) {
				tileData.sprite = sprite;
				return;
			}
			tileData.sprite = null;
		}

#if UNITY_EDITOR
		[MenuItem("Assets/Create/EditorTile")]
		public static void CreateEditorTile() {
			var path = EditorUtility.SaveFilePanelInProject("Save EditorTile", "editortile", "asset", "Save EditorTile");
			if (path == "") {
				return;
			}
			AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<EditorTile>(), path);
		}
#endif
	}
}