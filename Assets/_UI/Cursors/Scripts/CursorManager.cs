using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;
using static Randolph.Core.Constants;
using Randolph.UI;
using Randolph.Characters;
using Randolph.Interactable;
using Randolph.Levels;

//namespace Randolph.UI {
public class CursorManager : MonoBehaviour {

    public static readonly Vector2 CursorHotspot = Vector2.zero;
    public const CursorMode Mode = CursorMode.Auto;

    public static CursorManager cursorManager;
    public static GameCursor currentCursor;

    [SerializeField] CursorDatabase cursorDatabase;
    PlayerController player;

    void Awake() {
        //! Pass Cursor Manager between levels; destroy excess ones 
        if (FindObjectsOfType(GetType()).Length > 1) {
            Destroy(gameObject);
        } else {
            cursorManager = this;
            DontDestroyOnLoad(this);
        }

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

    public void SetCursorGrey(Cursors cursorType) {
        GameCursor cursor = cursorDatabase.GetCursor(cursorType);
        Cursor.SetCursor(cursor.disabledCursor, CursorHotspot, Mode);
    }

    void RegisterEvents() {
        Clickable.OnMouseEnterClickable += OnMouseEnterClickable;
        Clickable.OnMouseExitClickable += OnMouseExitClickable;
        Clickable.OnMouseDownClickable += OnMouseDownClickable;
        Clickable.OnMouseUpClickable += OnMouseUpClickable;

        LevelManager.OnNewLevel += OnNewLevel;
    }

    void OnNewLevel(Scene scene, PlayerController player) {
        this.player = player;
    }

    void OnMouseEnterClickable(Cursors cursorType, Vector2 position) {
        SetCursorOver(cursorType);
    }

    void OnMouseExitClickable(Cursors cursorType, Vector2 position) {
        SetCursorDefault();
    }

    void OnMouseDownClickable(Cursors cursorType, MouseButton button, Vector2 position) {
        SetCursorPressed(cursorType);
    }

    void OnMouseUpClickable(Cursors cursorType, MouseButton button, Vector2 position) {
        SetCursorOver(cursorType);
    }

    void OnDestroy() {
        LevelManager.OnNewLevel -= OnNewLevel;
    }

}
//}
