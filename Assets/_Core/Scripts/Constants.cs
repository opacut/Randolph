using Com.LuisPedroFonseca.ProCamera2D;
using Randolph.Characters;
using UnityEngine;

namespace Randolph.Core {
    public static class Constants {

        public const int PixelsPerUnit = 16;

        public static class Camera {
            public static ProCamera2DRooms rooms {
                get {
                    return UnityEngine.Camera.main.gameObject.GetComponent<ProCamera2DRooms>();
                }
            }
            public static ProCamera2DTransitionsFX transition {
                get {
                    return UnityEngine.Camera.main.gameObject.GetComponent<ProCamera2DTransitionsFX>();
                }
            }
        }

        public const float ApplicableDistance = 6f;

        public enum MouseButton {
            Left = 0,
            Right = 1,
            Middle = 2
        }

        public static class Audio {

            public const float FullVolume = 1.0f;
            public const float BackgroundVolume = 0.5f;
            public const float SilentVolume = 0.0f;

            /// <summary>Returns the current position of the audio listener.</summary>
            public static Vector2 AudioListener {
                get { return UnityEngine.Camera.main.transform.position; }
            }

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

        const float RaycastSkinWidth = 0.015f; // overlapping tolerance
        public const float RaycastBoundsShrinkage = RaycastSkinWidth * -2;

        /// <summary>Finds a <see cref="GameObject"/> with the <see cref="Tag.Player"/> tag and gets its <see cref="PlayerController"/>.</summary>
        public static PlayerController Randolph => GameObject.FindWithTag(Tag.Player)?.GetComponent<PlayerController>();
    }
}