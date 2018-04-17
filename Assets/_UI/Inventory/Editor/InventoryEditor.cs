using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Randolph.Characters;
using UnityEditor;
using UnityEngine;
using Randolph.Core;

namespace Randolph.UI {
    [CustomEditor(typeof(Inventory))]
    class InventoryEditor : Editor {

        // Inventory inventory;
        SerializedProperty applicableDistance;

        void OnEnable() {
            // inventory = (Inventory) target;
            applicableDistance = serializedObject.FindProperty(nameof(applicableDistance));
        }

        void OnSceneGUI() {
            serializedObject.Update();

            DrawApplicableDistanceHandle();

            serializedObject.ApplyModifiedProperties();
        }

        void DrawApplicableDistanceHandle() {
            var player = FindObjectOfType<PlayerController>();
            if (player) {
                Handles.color = new Color32(0xE5, 0xDD, 0x35, 0xFF / 5);
                Handles.DrawSolidDisc(player.transform.position, Vector3.forward, applicableDistance.floatValue);

                Handles.color = Color.yellow;
                Vector3 handlePosition = player.transform.position + new Vector3(applicableDistance.floatValue, 0, 0);
                applicableDistance.floatValue =
                        Handles.ScaleValueHandle(
                                value: applicableDistance.floatValue,
                                position: handlePosition,
                                rotation: Quaternion.Euler(-90, -90, 0),
                                size: HandleUtility.GetHandleSize(handlePosition),
                                capFunction: Handles.CylinderHandleCap,
                                snap: 1
                        ).Clamp(0, float.MaxValue);
            }
        }

    }
}
