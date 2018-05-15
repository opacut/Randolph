using UnityEngine;

namespace Randolph.UI {
    public class CursorManager : MonoBehaviour {

        public static CursorManager cursorManager;

        [SerializeField] GameCursor mainCursor;

        void Start() {
            //! Pass Level Manager between levels; destroy excess ones
            DontDestroyOnLoad(this);

            if (FindObjectsOfType(GetType()).Length > 1) {
                Destroy(gameObject);
            } else cursorManager = this;
        }

    }
}
