using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Bar : IDrawablePanel
{
    public int Width = 64;

    /// <summary>
    /// Draw the actual bar
    /// </summary>
    public void Draw()
    {
        //GUI.DrawTexture(Rect, Texture2D.whiteTexture);
        GUILayout.Box("No Move Selected", GUILayout.ExpandHeight(true), GUILayout.Width(Width));
    }
}
