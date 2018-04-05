using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Randolph.Core {
	public class EditorTile : Tile {

#if UNITY_EDITOR
		[MenuItem("Assets/Create/EditorTile")]
		public static void CreateEditorTile() {
			var path = EditorUtility.SaveFilePanelInProject("Save Editortile", "editortile", "asset", "Save Editortile");
			if (path == "") {
				return;
			}
			AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<EditorTile>(), path);
		}
#endif
		public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData) {
			base.GetTileData(position, tilemap, ref tileData);
			tileData.colliderType = ColliderType.Grid;
			if (Application.isEditor && !Application.isPlaying) {
				tileData.sprite = sprite;
				return;
			}
			tileData.sprite = null;
		}
	}
}