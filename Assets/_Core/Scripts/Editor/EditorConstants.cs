using UnityEngine;

using UnityEditor;

public class EditorConstants {

    [InitializeOnLoad]
    public static class Sprites {

        public static readonly string PixelSnapMaterialName = "Pixel sprite";

        static Material _pixelSnapMaterial = null;

        public static Material PixelSnapMaterial {
            get {
                if (_pixelSnapMaterial != null) return _pixelSnapMaterial;
                else {
                    _pixelSnapMaterial = GetPixelSnapMaterial();
                    return _pixelSnapMaterial;
                }
            }
        }

        static Sprites() {
            GetPixelSnapMaterial();
        }

        private static Material GetPixelSnapMaterial() {
            string[] guids = AssetDatabase.FindAssets($"t:Material {PixelSnapMaterialName}");
            if (guids == null || guids.Length == 0) {
                Debug.LogWarning($"There is no material called <color=red>{PixelSnapMaterialName}</color>.");
                return null;
            }

            return AssetDatabase.LoadAssetAtPath<Material>(AssetDatabase.GUIDToAssetPath(guids[0]));
        }

    }

}
