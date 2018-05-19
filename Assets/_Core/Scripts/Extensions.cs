using System;
using System.Collections.Generic;
using System.Globalization;
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

        public static Texture2D ToGreyscale(this Texture2D texture) {
            Texture2D greyTexture = new Texture2D(texture.width, texture.height, TextureFormat.ARGB32, false);
            Color[] texColors = texture.GetPixels();
            for (int i = 0; i < texColors.Length; i++) {
                float greyValue = texColors[i].grayscale;
                texColors[i] = new Color(greyValue, greyValue, greyValue, texColors[i].a);
            }
            greyTexture.SetPixels(texColors);
            greyTexture.Apply();
            return greyTexture;
        }

        public static Vector3 Abs(this Vector3 vector) {
            return new Vector3(Mathf.Abs(vector.x), Mathf.Abs(vector.y), Mathf.Abs(vector.z));
        }

        public static Vector2 Abs(this Vector2 vector) {
            return ((Vector3) vector).Abs();
        }

        public static Vector3 Multiply(this Vector3 vector, Vector3 multiplier) {
            vector.Scale(multiplier);
            return vector;
        }

        public static Vector2 Multiply(this Vector2 vector, Vector2 multiplier) {
            return ((Vector3) vector).Multiply(multiplier);
        }

        /// <summary>Checks whether a number in within given bounds.</summary>
        public static bool IsRange(this float value, float lowerBound, float upperBound) {
            return value >= lowerBound && value <= upperBound;
        }

        /// <summary>Checks whether a number in more than zero and less than one.</summary>
        public static bool IsRange01(this float value) {
            return value.IsRange(0, 1);
        }

        /// <summary>Clamps a number within given bounds.</summary>
        public static float Clamp(this float value, float min, float max) {
            return Mathf.Clamp(value, min, max);
        }

        /// <summary>Clamps a number within zero to one range.</summary>
        public static float Clamp01(this float value) {
            return Mathf.Clamp01(value);
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

        public static string ToTitleCase(this string scriptName) {
            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
            return textInfo.ToTitleCase(scriptName).Replace("-", "_").Replace(" ", "");
        }

        /// <summary>Trims the end of the string.</summary>       
        public static string TrimEnd(this string source, string value) {
            if (!source.EndsWith(value)) return source;
            return source.Remove(source.LastIndexOf(value, StringComparison.Ordinal));
        }

        public static float RemapRange(this float value, float from1, float to1, float from2, float to2) {
            return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
        }

        public static float RemapRange(this float value, Tuple<float, float> from, Tuple<float, float> to) {
            return (value - @from.Item1) / (@from.Item2 - @from.Item1) * (to.Item2 - to.Item1) + to.Item1;
        }

    }
}
