using System.Linq;
using Randolph.Characters;
using Randolph.Levels;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Randolph.UI {
    [RequireComponent(typeof(Image))]
    public class Worldmap : MonoBehaviour {

        [System.Serializable]
        class MapState {

            // TODO: Serializable dictionary (1 scene : 1 map)
            [Scene] public string scene = string.Empty;
            public Sprite map = null;
            public Sprite highlight = null;

        }

        Image currentMap;
        [SerializeField] MapState[] mapStates; // TODO: Sync with build settings

        void Awake() {
            currentMap = GetComponent<Image>();
            Sprite map;
            if (TryGetMap(SceneManager.GetActiveScene().name, out map)) {
                currentMap.sprite = map;
            }
        }

        bool TryGetMap(string scene, out Sprite map) {
            map = mapStates.FirstOrDefault(mapState => mapState.scene == scene)?.map;
            return map != null;
        }

    }
}
