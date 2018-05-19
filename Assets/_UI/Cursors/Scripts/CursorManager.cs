using Randolph.Interactable;
using UnityEngine;
using UnityEngine.Assertions;
using static Randolph.Core.Constants;

namespace Randolph.UI {
    public class CursorManager : MonoBehaviour {

        public static readonly Vector2 CursorHotspot = Vector2.zero;
        public const CursorMode Mode = CursorMode.Auto;

        public static CursorManager cursorManager;
        public static GameCursor currentCursor;

        [SerializeField] CursorDatabase cursorDatabase;

        void Awake() {
            //! Pass Cursor Manager between levels; destroy excess ones 
            DontDestroyOnLoad(this);

            if (FindObjectsOfType(GetType()).Length > 1) {
                Destroy(gameObject);
            } else cursorManager = this;

            Assert.IsNotNull(cursorDatabase, "The Cursor Database is not assigned → the cursor can't change.");
            RegisterEvents();
        }

        public void SetCursorDefault() {
            GameCursor cursor = cursorDatabase.GetDefault();
            Cursor.SetCursor(cursor.overCursor, CursorHotspot, Mode);
        }

        public void SetCursorOver(Cursors cursorType) {
            GameCursor cursor = cursorDatabase.GetCursor(cursorType);
            Cursor.SetCursor(cursor.overCursor, CursorHotspot, Mode);
        }

        public void SetCursorPressed(Cursors cursorType) {
            GameCursor cursor = cursorDatabase.GetCursor(cursorType);
            Cursor.SetCursor(cursor.pressedCursor, CursorHotspot, Mode);
        }

        void RegisterEvents() {
            Clickable.OnMouseEnterClickable += OnMouseEnterClickable;
            Clickable.OnMouseExitClickable += OnMouseExitClickable;
            Clickable.OnMouseDownClickable += OnMouseDownClickable;
            Clickable.OnMouseUpClickable += OnMouseUpClickable;
        }

        void OnMouseEnterClickable(Cursors cursorType) {
            SetCursorOver(cursorType);
        }        

        void OnMouseExitClickable(Cursors cursorType) {
            SetCursorDefault();
        }

        void OnMouseDownClickable(Cursors cursorType, MouseButton button) {
            SetCursorPressed(cursorType);
        }

        void OnMouseUpClickable(Cursors cursorType, MouseButton button) {
            SetCursorOver(cursorType);
        }

    }
}
