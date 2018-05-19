using System;
using Randolph.Core;
using Randolph.Interactable;
using UnityEngine;
using UnityEngine.Assertions;

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
            Pickable.OnMouseEnterPickable += OnMouseEnterPickable;
            Pickable.OnMouseExitPickable += OnMouseExitPickable;
            Pickable.OnMouseDownPickable += OnMouseDownPickable;
            Pickable.OnMouseUpPickable += OnMouseUpPickable;
        }

        void OnMouseEnterPickable() {
            SetCursorOver(Cursors.Pick);
        }

        void OnMouseExitPickable() {
            SetCursorDefault();
        }

        void OnMouseDownPickable(Constants.MouseButton button) {
            SetCursorPressed(Cursors.Pick);
            /*
            switch (button) {
                case Constants.MouseButton.Left:
                    Debug.Log("Left click on pickable");                    
                    break;
                case Constants.MouseButton.Right:
                    Debug.Log("Right click on pickable");
                    break;
                case Constants.MouseButton.Middle:
                    Debug.Log("Middle click on pickable");
                    break;
            }
            */
        }

        void OnMouseUpPickable(Constants.MouseButton button) {
            SetCursorOver(Cursors.Pick);
        }

    }
}
