using System.IO;
using UnityEditor;
using UnityEngine;

namespace Randolph.Core {
	public class SpriteImporter : AssetPostprocessor {

		void OnPreprocessTexture() {
			Debug.Log("Preprocessing texture " + assetPath);

			TextureImporter textureImporter = (TextureImporter) assetImporter;
			textureImporter.filterMode = FilterMode.Point;

			textureImporter.spritePixelsPerUnit = Constants.pixelsPerUnit;
			textureImporter.textureCompression = TextureImporterCompression.Uncompressed;
			textureImporter.textureType = TextureImporterType.Sprite;

			if (assetPath.Contains("tile_"))
				PreprocessTileset();
		}

		void OnPostprocessTexture(Texture2D texture) {
			Debug.Log($"Postprocessing texture <color=orange>{assetPath}</color>", texture);

			if (assetPath.Contains("tile_"))
				PostprocessTileset(texture);
		}

		void OnPostprocessSprites(Texture2D texture, Sprite[] sprites) {
			Debug.Log("Postprocessing sprites " + assetPath);
			if (!assetPath.Contains("tile_"))
				return;
			Debug.Log("Postprocessing tilemap's sprites");
			Debug.Log("Sprites: " + sprites.Length);

		}

		#region Tileset
		static int tSize = 8;
		static int[, ] tileQuarters = new int[47, 4] { { 13, 14, 17, 18 }, { 2, 14, 17, 18 }, { 13, 3, 17, 18 }, { 2, 3, 17, 18 }, { 13, 14, 17, 7 }, { 2, 14, 17, 7 }, { 13, 3, 17, 7 }, { 2, 3, 17, 7 }, { 13, 14, 6, 18 }, { 2, 14, 6, 18 }, { 13, 3, 6, 18 }, { 2, 3, 6, 18 }, { 13, 14, 6, 7 }, { 2, 14, 6, 7 }, { 13, 3, 6, 7 }, { 2, 3, 6, 7 }, { 12, 14, 16, 18 }, { 12, 3, 16, 18 }, { 12, 14, 16, 7 }, { 12, 3, 16, 7 }, { 9, 10, 17, 18 }, { 9, 10, 17, 7 }, { 9, 10, 6, 18 }, { 9, 10, 6, 7 }, { 13, 15, 17, 19 }, { 13, 15, 6, 19 }, { 2, 15, 17, 19 }, { 2, 15, 6, 19 }, { 13, 14, 21, 22 }, { 2, 14, 21, 22 }, { 13, 3, 21, 22 }, { 2, 3, 21, 22 }, { 12, 15, 16, 19 }, { 9, 10, 21, 22 }, { 8, 9, 12, 18 }, { 8, 9, 12, 7 }, { 10, 11, 17, 15 }, { 10, 11, 6, 15 }, { 13, 19, 22, 23 }, { 2, 19, 22, 23 }, { 16, 14, 20, 21 }, { 16, 3, 20, 21 }, { 8, 11, 12, 15 }, { 8, 9, 20, 21 }, { 16, 19, 20, 23 }, { 10, 11, 22, 23 }, { 8, 11, 20, 23 } };
		static Rect[] tileRects = new Rect[24] { new Rect(0, 40, 8, 8), new Rect(8, 40, 8, 8), new Rect(16, 40, 8, 8), new Rect(24, 40, 8, 8), new Rect(0, 32, 8, 8), new Rect(8, 32, 8, 8), new Rect(16, 32, 8, 8), new Rect(24, 32, 8, 8), new Rect(0, 24, 8, 8), new Rect(8, 24, 8, 8), new Rect(16, 24, 8, 8), new Rect(24, 24, 8, 8), new Rect(0, 16, 8, 8), new Rect(8, 16, 8, 8), new Rect(16, 16, 8, 8), new Rect(24, 16, 8, 8), new Rect(0, 8, 8, 8), new Rect(8, 8, 8, 8), new Rect(16, 8, 8, 8), new Rect(24, 8, 8, 8), new Rect(0, 0, 8, 8), new Rect(8, 0, 8, 8), new Rect(16, 0, 8, 8), new Rect(24, 0, 8, 8) };

		void PreprocessTileset() {
			TextureImporter textureImporter = (TextureImporter) assetImporter;
			textureImporter.spriteImportMode = SpriteImportMode.None;
			textureImporter.spritesheet = null;
		}

		void PostprocessTileset(Texture2D texture) {
			Debug.Log("Creating tileset " + assetPath);
			TextureImporter textureImporter = (TextureImporter) assetImporter;

			textureImporter.spriteImportMode = SpriteImportMode.None;
			var autotilePath = Path.Combine(Path.GetDirectoryName(assetPath), Path.GetFileNameWithoutExtension(assetPath) + "_autotile.asset");
			var autotile = AssetDatabase.LoadAssetAtPath(autotilePath, typeof(Autotile)) as Autotile;
			if (autotile == null) {
				autotile = ScriptableObject.CreateInstance<Autotile>();
				AssetDatabase.CreateAsset(autotile, autotilePath);
			} else {
				EditorUtility.CopySerialized(ScriptableObject.CreateInstance<Autotile>(), autotile);
			}

			for (int i = 0; i < Constants.Tilemap.tileCount; ++i) {
				autotile.sprites[i] = SpriteFromSlices(i, texture, autotile);
			}
			autotile.sprite = autotile.sprites[Constants.Tilemap.tileCount - 1];
		}

		private Sprite SpriteFromSlices(int i, Texture2D texture, Autotile autotile) {
			var newTexture = new Texture2D(tSize * 2, tSize * 2);
			newTexture.name = "texture_" + i.ToString("00");
			newTexture.filterMode = FilterMode.Point;
			newTexture.SetPixels(0, tSize, tSize, tSize, texture.GetPixels(tileRects[tileQuarters[i, 0]]), 0);
			newTexture.SetPixels(tSize, tSize, tSize, tSize, texture.GetPixels(tileRects[tileQuarters[i, 1]]), 0);
			newTexture.SetPixels(0, 0, tSize, tSize, texture.GetPixels(tileRects[tileQuarters[i, 2]]), 0);
			newTexture.SetPixels(tSize, 0, tSize, tSize, texture.GetPixels(tileRects[tileQuarters[i, 3]]), 0);
			newTexture.Apply();
			UnityEditor.AssetDatabase.AddObjectToAsset(newTexture, autotile);

			var sprite = Sprite.Create(newTexture, new Rect(0.0f, 0.0f, tSize * 2, tSize * 2), new Vector2(0.5f, 0.5f), Constants.pixelsPerUnit);
			sprite.name = "sprite_" + i.ToString("00");
			UnityEditor.AssetDatabase.AddObjectToAsset(sprite, autotile);
			UnityEditor.AssetDatabase.SaveAssets();
			return sprite;
		}
		#endregion

	}
}