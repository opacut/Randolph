using UnityEngine;

namespace Randolph.UI {
    [CreateAssetMenu(fileName = "Cursor", menuName = "Randolph/UI/Cursor", order = 22)]
    public class GameCursor : ScriptableObject {

        [SerializeField] Texture2D defaultCursor; // mandatory
        // [SerializeField] Texture2D hoverCursor;
        [SerializeField] Texture2D pressedCursor;

    }
}
