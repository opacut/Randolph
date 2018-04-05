using UnityEditor;
using UnityEngine;

using Randolph.Characters;


namespace Randolph.Core
{
    /// <summary>
    /// Displays small icons next to objects in the Hieararchy.
    /// </summary>
    [InitializeOnLoad]
    class HierarchyIcons
    {
        static readonly Texture2D PlayerIcon;

        static HierarchyIcons()
        {
            PlayerIcon = AssetDatabase.LoadAssetAtPath("Assets/Gizmos/Randolph/Player icon.png", typeof(Texture2D)) as Texture2D;
            EditorApplication.hierarchyWindowItemOnGUI += OnItemInHierarchy;
        }

        /// <summary>
        /// Displays icon next to marked objects on the right side of Hierarchy tab.
        /// </summary>
        /// <param name="instanceID">Instance ID of hierarchy object.</param>
        /// <param name="selectionRect">Area containing hierarchy object's label.</param>
        static void OnItemInHierarchy(int instanceID, Rect selectionRect)
        {
            GameObject item = EditorUtility.InstanceIDToObject(instanceID) as GameObject;
            if (item)
            {
                // Offset -> Align root gameobjects' icon with nested gameobjects' icon
                int offset = 10;
                Transform itemParent = item.transform.parent;
                while (itemParent != null)
                {
                    offset += 14;
                    itemParent = itemParent.transform.parent;
                }

                // Position the icon
                Rect rect = new Rect(selectionRect);
                rect.x = rect.width + offset;
                rect.width = 18;

                // Draw the icon if it's a player
                if (item.GetComponent<PlayerController>()) GUI.Label(rect, PlayerIcon);
            }
        }
    }
}
