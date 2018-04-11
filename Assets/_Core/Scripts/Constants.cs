using UnityEngine;

namespace Randolph.Core {
    public static class Constants {

        public static readonly int PixelsPerUnit = 16;

        public static class Tilemap {
            public static readonly int TileCount = 47;
        }

        public static class Tag {
            public static readonly string Player = "Player";
            public static readonly string Ladder = "Ladder";
            public static readonly string Pickable = "Pickable";
            public static readonly string Deadly = "Deadly";
        }

        public static class Layer {
            public static readonly int Ground = LayerMask.NameToLayer("Ground");
            public static readonly int Player = LayerMask.NameToLayer("Player");
            public static readonly int Dead = LayerMask.NameToLayer("Dead");
        }

        /// <summary>The optimal size for drawing dots with <see cref="Gizmos.DrawSphere"/>.</summary>
        public const float GizmoSphereRadius = 0.25f;

        public static readonly LayerMask GroundLayerMask = 1 << Layer.Ground;
    }
}