using UnityEngine;

namespace Randolph.Core {
    public static class Extensions {

        public static Color[] GetPixels(this Texture2D texture, Rect rect) {
            return texture.GetPixels((int) rect.x, (int) rect.y, (int) rect.width, (int) rect.height);
        }

    }
}
