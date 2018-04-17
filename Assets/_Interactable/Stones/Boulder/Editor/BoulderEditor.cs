using Randolph.Core;
using UnityEditor;
using UnityEngine;

namespace Randolph.Interactable {
    [CustomEditor(typeof(Boulder))]
    public class BoulderEditor : Editor {

        Boulder boulder;
        CircleCollider2D circleCollider;

        SerializedProperty maxFallAngle;
        SerializedProperty maxPushAngle;

        void OnEnable() {
            boulder = (Boulder) target;
            circleCollider = boulder.GetComponent<CircleCollider2D>();
            maxFallAngle = serializedObject.FindProperty(nameof(maxFallAngle));
            maxPushAngle = serializedObject.FindProperty(nameof(maxPushAngle));
        }

        void OnSceneGUI() {
            DrawFallAngle();
            DrawPushAngle();
        }

        void DrawFallAngle() {
            var orangeColor = new Color32(0xF7, 0x95, 0x45, 0xFF / 5);
            DrawCircularArch(orangeColor, 180, maxFallAngle.intValue);
        }

        void DrawPushAngle() {
            var greenColor = new Color32(0x00, 0xAF, 0x4F, 0xFF / 5);
            DrawCircularArch(greenColor, 90, maxPushAngle.intValue);
            DrawCircularArch(greenColor, -90, maxPushAngle.intValue);
        }

        // TODO: ArcHandle
        void DrawCircularArch(Color color, int initialAngle, int angleWidth) {
            Handles.color = color;
            Vector3 startingPoint = Methods.GetCirclePoint(circleCollider.offset, initialAngle + angleWidth / 2, circleCollider.radius);
            Handles.DrawSolidArc(
                    center: boulder.transform.position,
                    normal: boulder.transform.forward,
                    from: startingPoint,
                    angle: angleWidth,
                    radius: circleCollider.radius * 1f
            );
        }

    }
}
