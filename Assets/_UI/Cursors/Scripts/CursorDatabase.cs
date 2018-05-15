using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Randolph.UI {
    [CreateAssetMenu(fileName = "Cursor Database", menuName = "Randolph/UI/Cursor", order = 66)]
    public class CursorDatabase : ScriptableObject {

        [SerializeField] List<GameCursor> cursors;

        /// <summary>Returns the first cursor in the Cursor Database.</summary>
        public GameCursor GetDefault() {
            return GetCursor(0);
        }

        public GameCursor GetCursor(Cursors cursorType) {
            return GetCursor(cursorType.ToString());
        }

        public GameCursor GetCursor(int index) {
            return cursors.ElementAt(index);
        }

        public GameCursor GetCursor(string cursorName) {
            return cursors.FirstOrDefault(cursor => cursor.name == cursorName);
        }

    }
}
