using UnityEngine;

namespace Randolph.Core {    
    public static class Constants {

        public static readonly int pixelsPerUnit = 16;

        public static class Tilemap {

            public static readonly int tileCount = 47;

        }

        /// <summary>The optimal size for drawing dots with <see cref="Gizmos.DrawSphere"/>.</summary>
        public const float GizmoSphereRadius = 0.25f;

        /// <summary>Which layer(s) should behave as solid ground and platforms.</summary>
        public static readonly LayerMask GroundLayer = 1 << LayerMask.NameToLayer("Ground");
    }
}
