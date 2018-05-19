using System;
using UnityEngine;
using System.Collections;
using Randolph;
using Randolph.Interactable;
using Randolph.UI;

public class ClickableType : Clickable {

    public override Cursors CursorType { get; protected set; } = Cursors.Interact;

}
