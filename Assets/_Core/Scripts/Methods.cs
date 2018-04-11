using System;

using UnityEngine;


namespace Randolph.Core {
    public static class Methods {

        public static bool IsEmptyGameObject(GameObject obj) {
            Component[] allComponents = obj.GetComponents<Component>();
            return allComponents.Length == 1 && obj.transform.childCount == 0;
        }

        public static void GizmosDrawCircle(Vector2 center, float radius, float step = 0.1f) {
            float theta = 0.0f;
            float x = radius * Mathf.Cos(theta);
            float y = radius * Mathf.Sin(theta);
            Vector2 position = center + new Vector2(x, y);
            Vector2 newPosition = position;
            Vector2 lastPosition = position;
            for (theta = step; theta < Mathf.PI * 2; theta += step) {
                x = radius * Mathf.Cos(theta);
                y = radius * Mathf.Sin(theta);
                newPosition = center + new Vector2(x, y);
                Gizmos.DrawLine(position, newPosition);
                position = newPosition;
            }

            Gizmos.DrawLine(position, lastPosition);
        }

        /// <summary>Gets the angle between a direction and a collision normal.</summary>
        /// <returns>Collision angle, 90f if there was no point of contact.</returns>
        public static float GetCollisionAngle(Collision2D collision, Vector2 direction) {
            if (collision.contacts.Length == 0) return 90f;
            Vector2 normal = collision.contacts[0].normal;
            return Vector2.Angle(direction, -normal);
        }

    }
}
