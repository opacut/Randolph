using Randolph.Interactable;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Randolph.UI;
using Randolph.Levels;

public class Slab : Clickable, IRestartable
{
    public override Cursors CursorType { get; protected set; } = Cursors.Inspect;

    /*
    public override void Restart()
    {
        base.Restart();
    }
    */
}
