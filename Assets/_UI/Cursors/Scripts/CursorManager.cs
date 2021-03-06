﻿using Randolph.Core;
using Randolph.Interactable;
using UnityEngine;
using UnityEngine.Assertions;

namespace Randolph.UI {
public class CursorManager : MonoBehaviour {
    // TODO include mouse events on inventory icons
    public static readonly Vector2 CursorHotspot = Vector2.zero;
    public const CursorMode Mode = CursorMode.Auto;

    public static CursorManager cursorManager;
    public static GameCursor currentCursor;

    [SerializeField] CursorDatabase cursorDatabase;
    Clickable continuousTarget = null;
    bool clickHold = false;

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

    void Update() {
        OnMouseOverClickable();
    }

    public void SetCursorDefault() {
        continuousTarget = null;
        clickHold = false;
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
    }

        public bool WithinDistance(Vector2 position) => Inventory.inventory?.IsWithinApplicableDistance(position) ?? false;

        void OnMouseEnterClickable(Clickable target) {
        // Update even when player moves
        continuousTarget = target;
        clickHold = false;
    }

    void OnMouseOverClickable() {
        // Inside Update
        if (PauseMenu.IsPaused) SetCursorDefault();
        else if (continuousTarget != null && !clickHold) {
            if (WithinDistance(continuousTarget.transform.position)) SetCursorOver(continuousTarget.CursorType);
            else SetCursorGrey(continuousTarget.CursorType);
        }
    }

    void OnMouseExitClickable(Clickable target) {
        continuousTarget = null;
        SetCursorDefault();
    }

    void OnMouseDownClickable(Clickable target, Constants.MouseButton button) {
        clickHold = true;
        if (WithinDistance(target.transform.position)) {
            SetCursorPressed((button == Constants.MouseButton.Right) ? Cursors.Inspect : target.CursorType);
        } else {
            SetCursorGrey((button == Constants.MouseButton.Right) ? Cursors.Inspect : target.CursorType);            
        }
    }

    void OnMouseUpClickable(Clickable target, Constants.MouseButton button) {
        clickHold = false;
        if (WithinDistance(target.transform.position)) SetCursorOver(target.CursorType);
        else SetCursorGrey(target.CursorType);
    }
    
}
}
