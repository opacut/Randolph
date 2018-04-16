using System.Text.RegularExpressions;
using UnityEngine;


namespace Randolph.Core {
    public static class Methods {

        /// <summary>Checks whether an object is empty.</summary>
        /// <param name="obj">The object to be tested.</param>
        /// <param name="allowChildren">Empty objects with nested children are considered empty, too.</param>
        /// <returns>True if the object is empty.</returns>
        public static bool IsEmptyGameObject(GameObject obj, bool allowChildren = false) {
            Component[] allComponents = obj.GetComponents<Component>();
            return allComponents.Length == 1 && (allowChildren || obj.transform.childCount == 0);
        }

        /// <summary>Checks whether an object is supposed to be visible in the game, i.e. contains a <see cref="SpriteRenderer"/>.</summary>
        public static bool IsDisplayableGameObject(GameObject obj) {
            return obj.GetComponent<SpriteRenderer>() != null;
        }

        /// <summary>Gets the angle between a direction and a collision normal.</summary>
        /// <returns>Collision angle, 90f if there was no point of contact.</returns>
        public static float GetCollisionAngle(Collision2D collision, Vector2 direction) {
            if (collision.contacts.Length == 0) return 90f;
            Vector2 normal = collision.contacts[0].normal;
            return Vector2.Angle(direction, -normal);
        }

        public static void IgnoreLayerMaskCollision(int layer, LayerMask layers, bool ignore) {
            uint layerBits = (uint) layers.value;
            for (int i = 31; layerBits > 0; i--)
                if ((layerBits >> i) > 0) {
                    layerBits = ((layerBits << 32 - i) >> 32 - i);
                    Physics2D.IgnoreLayerCollision(layer, i, ignore);
                }
        }

        public static Vector3 GetMeanVector(Vector3[] positions) {
            Vector3 meanVector = Vector3.zero;
            if (positions.Length == 0) return meanVector;
            foreach (Vector3 position in positions) {
                meanVector.x += position.x;
                meanVector.y += position.y;
                meanVector.z += position.z;
            }
            return meanVector / positions.Length;
        }

        /// <summary>Returns a number in a string, or -1 if there's none.</summary>
        /// <param name="str">A string containing a negative or positive number.</param>
        /// <returns>The first number contained in a string or -1 if there's no number.</returns>
        public static int GetNumberFromString(string str) {
            string number = Regex.Match(str, @"-?\d+").Value;
            if (string.IsNullOrWhiteSpace(number)) return -1;
            else return int.Parse(number);
        }

        public static Rect MinMaxRect(Vector2 minXY, Vector2 maxXY) {
            return Rect.MinMaxRect(minXY.x, minXY.y, maxXY.x, maxXY.y);
        }

        public static void GizmosDrawCircle(Vector2 center, float radius, float step = 0.1f) {
            float theta = 0.0f;
            float x = radius * Mathf.Cos(theta);
            float y = radius * Mathf.Sin(theta);
            Vector2 position = center + new Vector2(x, y);
            Vector2 lastPosition = position;
            for (theta = step; theta < Mathf.PI * 2; theta += step) {
                x = radius * Mathf.Cos(theta);
                y = radius * Mathf.Sin(theta);
                Vector2 newPosition = center + new Vector2(x, y);
                Gizmos.DrawLine(position, newPosition);
                position = newPosition;
            }

            Gizmos.DrawLine(position, lastPosition);
        }

    }
}
