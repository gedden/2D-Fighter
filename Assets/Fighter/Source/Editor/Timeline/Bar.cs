using Comboman;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Bar : IDrawablePanel
{
    public int Width = 64;
    private MoveFrame frame;
    private CharacterData _data;

    public Bar(MoveFrame frame, CharacterData _data)
    {
        this.frame = frame;
        this._data = _data;
    }

    /// <summary>
    /// Draw the actual bar
    /// </summary>
    public void Draw()
    {
        //GUI.DrawTexture(Rect, Texture2D.whiteTexture);
        GUILayout.Box(frame.FrameName, GUILayout.ExpandHeight(true), GUILayout.Width(Width));
    }
}
