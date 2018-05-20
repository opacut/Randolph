using System.Collections.Generic;
using System.Linq;
using Com.LuisPedroFonseca.ProCamera2D;
using Randolph.Characters;
using Randolph.Core;
using Randolph.Environment;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Randolph.Levels {
    public class Area : MonoBehaviour {

        ProCamera2DRooms cameraRooms;
        Room matchingCameraRoom;

        public Room MatchingCameraRoom {
            get {
                if (matchingCameraRoom == null) RefreshCameraData();
                return matchingCameraRoom;
            }
            private set { matchingCameraRoom = value; }
        }

        public const int InvalidId = -1;

        public Rect Dimensions {
            get {
                Rect roomRect = MatchingCameraRoom.Dimensions;
                Vector2 bottomLeft = roomRect.center - roomRect.size;
                Vector2 topRight = roomRect.center;
                // topLeft.y = -topLeft.y;
                return Methods.MinMaxRect(bottomLeft, topRight);
            }
        }

        public int Id {
            get { return Methods.GetNumberFromString(gameObject.name); }
        }


        public static Area GetArea(int index) {
            Area[] areas = LevelManager.levelManager.Areas;
            if (index >= areas.Length || index < 0) return null;
            else return areas[index];
        }

        void Start() {
            LevelManager.OnNewLevel += OnNewLevel;
        }

        void OnNewLevel(Scene scene, PlayerController player) {
            RefreshCameraData();
            if (HasNestedAreas()) {
                Debug.LogError($"<b>{gameObject.name}</b> is nested in another area. This can cause problems.", gameObject);
            }
            CheckAreaBounds();
        }

        public void RefreshCameraData() {
            cameraRooms = FindObjectOfType<ProCamera2DRooms>();
            MatchingCameraRoom = cameraRooms.GetRoom($"{Methods.GetNumberFromString(gameObject.name)}");
            if (Application.isPlaying && matchingCameraRoom == null) Debug.LogError($"No corresponding camera room found for <b>{gameObject.name}</b>.");
        }

        bool HasNestedAreas() {
            Area[] areasUpwards = GetComponentsInParent<Area>(true); // includes this area
            return areasUpwards.Length > 1;
        }

        void CheckAreaBounds() {
            if (MatchingCameraRoom == null) return;
            var allChildren = new List<Transform>(GetComponentsInChildren<Transform>());
            allChildren.Remove(transform); // Without self
            var displayableChildren = allChildren.Where(t => Methods.IsDisplayableGameObject(t.gameObject));
            foreach (Transform child in displayableChildren) {
                if (!Contains(child.position)) {
                    Debug.LogWarning($"<b>{child.name}</b> is outside of its area bounds.", child.gameObject);
                }
            }
        }

        /// <summary>Checks whether a position is located within the area.</summary>
        public bool Contains(Vector2 position) {
            return Dimensions.Contains(position);
        }

        /// <summary>Checks whether an object is a child of the area.</summary>
        public bool Contains(GameObject obj) {
            return obj.transform.IsChildOf(transform);
        }

        public void SetAreaSpatialBlend(float spatialBlend) {
            foreach (AudioSource audioSource in GetComponentsInChildren<AudioSource>(true)) {
                audioSource.spatialBlend = spatialBlend;
            }
        }

        public List<SpikeTrap> GetAreaSpikes() {
            var spikes = new List<SpikeTrap>();
            GetComponentsInChildren(true, spikes);
            return spikes;
        }

        void OnDestroy() {
            LevelManager.OnNewLevel -= OnNewLevel;
        }

    }
}
