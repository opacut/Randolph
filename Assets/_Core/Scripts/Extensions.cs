using System.Collections.Generic;
using System.Linq;

using UnityEngine;

namespace Randolph.Core {
    public static class Extensions {

        public static Color[] GetPixels(this Texture2D texture, Rect rect) {
            return texture.GetPixels((int) rect.x, (int) rect.y, (int) rect.width, (int) rect.height);
        }

        public static Vector3 Abs(this Vector3 vector) {
            return new Vector3(Mathf.Abs(vector.x), Mathf.Abs(vector.y), Mathf.Abs(vector.z));
        }

        public static Vector2 Abs(this Vector2 vector) {
            return ((Vector3) vector).Abs();
        }

        public static T GetNextItem<T>(this IList<T> list, T current) {
            return list.SkipWhile(item => !item.Equals(current)).Skip(1).FirstOrDefault();
        }

        public static T GetPreviousItem<T>(this IList<T> list, T current) {
            return list.TakeWhile(item => !item.Equals(current)).LastOrDefault();
        }

    }
}
