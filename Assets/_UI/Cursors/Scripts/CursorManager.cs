using UnityEngine;

namespace Randolph.UI {
    public class CursorManager : MonoBehaviour {

        public static CursorManager cursorManager;
        public static GameCursor currentCursor;

        [SerializeField] CursorDatabase cursorDatabase;

        void Awake() {
            //! Pass Cursor Manager between levels; destroy excess ones 
            DontDestroyOnLoad(this);

            if (FindObjectsOfType(GetType()).Length > 1) {
                Destroy(gameObject);
            } else cursorManager = this;

            
        }

    }
}
