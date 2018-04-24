using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Randolph.Tiles {
	public class ConnectedTile : Tile {
		public string familyName;

		public override void RefreshTile(Vector3Int position, ITilemap tilemap) {
			base.RefreshTile(position, tilemap);
			for (int j = -1; j <= 1; ++j)
				for (int i = -1; i <= 1; ++i) {
					var neighborPos = new Vector3Int(position.x + i, position.y + j, position.z);
					if (CanConnect(tilemap, neighborPos))
						tilemap.RefreshTile(neighborPos);
				}
		}

		protected bool CanConnect(ITilemap tilemap, Vector3Int pos) {
			var autotile = tilemap.GetTile(pos) as ConnectedTile;
			if (autotile == null) {
				return false;
			}
			return autotile.familyName == familyName;
		}

#if UNITY_EDITOR
		[MenuItem("Assets/Create/ConnectedTile")]
		public static void CreateConnectedTile() {
			var path = EditorUtility.SaveFilePanelInProject("Save ConnectedTile", "connectedtile", "asset", "Save ConnectedTile");
			if (path == "") {
				return;
			}
			AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<ConnectedTile>(), path);
		}
#endif
	}
}