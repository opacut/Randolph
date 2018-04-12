using UnityEngine;

namespace Randolph.Core {
    public static class Constants {

        public const int PixelsPerUnit = 16;

        public static class Audio {

            public static readonly Vector2 AudioListener = Camera.main.transform.position;

        }

        public static class Tilemap {

            public const int TileCount = 47;

        }

        public static class Tag {

            public const string Player = "Player";
            public const string Ladder = "Ladder";
            public const string Pickable = "Pickable";
            public const string Deadly = "Deadly";
            public const string Enemy = "Enemy";

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
