using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Randolph.Core {
    public static class Extensions {

        public static Color[] GetPixels(this Texture2D texture, Rect rect) {
            return texture.GetPixels((int) rect.x, (int) rect.y, (int) rect.width, (int) rect.height);
        }

        public static Transform GetNearest(this Transform currentTransform, Transform[] otherTransforms) {
            Transform nearest = null;
            float currentDistance = Mathf.Infinity;
            foreach (var other in otherTransforms) {
                float newDistance = Vector2.Distance(currentTransform.position, other.position);
                if (newDistance < currentDistance) {
                    currentDistance = newDistance;
                    nearest = other;
                }
            }

            return nearest;
        }

        public static void AlignToGround(this Transform transform, float rayLength = 30f) {
            var collider2D = transform.GetComponent<Collider2D>();
            //var spriteRenderer = transform.GetComponent<SpriteRenderer>();
            Vector2 minPointBelow;
            float offsetY = 0f;
            if (collider2D) {
                Bounds bounds = collider2D.bounds;
                bounds.Expand(Constants.RaycastBoundsShrinkage);
                minPointBelow = new Vector2(transform.position.x, bounds.min.y);
                offsetY = bounds.center.y - minPointBelow.y;
            } else return;
            // else if (spriteRenderer) {
            //     spriteRenderer.size
            //   }
            RaycastHit2D hit = Physics2D.Raycast(minPointBelow, Vector2.down, rayLength, Constants.GroundLayerMask);
            Vector2 groundPoint = hit.point;
            transform.position = new Vector2(transform.position.x, groundPoint.y + offsetY);
        }

        public static Vector3 Abs(this Vector3 vector) {
            return new Vector3(Mathf.Abs(vector.x), Mathf.Abs(vector.y), Mathf.Abs(vector.z));
        }

        public static Vector2 Abs(this Vector2 vector) {
            return ((Vector3) vector).Abs();
        }

        public static float RoundToDigits(this float value, int decimalDigits) {
            var unit = Mathf.Pow(10, decimalDigits);
            return Mathf.Round(value * unit) / unit;
        }

        public static T GetNextItem<T>(this IList<T> list, T current) {
            return list.SkipWhile(item => !item.Equals(current)).Skip(1).FirstOrDefault();
        }

        public static T GetPreviousItem<T>(this IList<T> list, T current) {
            return list.TakeWhile(item => !item.Equals(current)).LastOrDefault();
        }

        public static string GetPath(this Transform current) {
            if (current.parent == null) return "/" + current.name;
            return current.parent.GetPath() + "/" + current.name;
        }

        public static string GetPath(this Component component) {
            return component.transform.GetPath() + "/" + component.GetType().ToString();
        }

        public static float RemapRange(this float value, float from1, float to1, float from2, float to2) {
            return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
        }

        public static float RemapRange(this float value, Tuple<float, float> from, Tuple<float, float> to) {
            return (value - @from.Item1) / (@from.Item2 - @from.Item1) * (to.Item2 - to.Item1) + to.Item1;
        }

    }
}
