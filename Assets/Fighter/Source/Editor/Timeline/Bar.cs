using Comboman;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

public class Bar : IDrawablePanel
{
    public static readonly float PIXELS_PER_SECOND = 500f;
    public static readonly float WIDTH = 120f;
    
    private CharacterData _data;
    public bool IsDraggingOver;
    public bool IsDraggingBefore;
    private string currentDurationString;

    private static GUIStyle boxStyle;

    public Bar(MoveFrame frame, CharacterData _data)
    {
        this.MoveFrame = frame;
        currentDurationString = ""+MoveFrame.Duration;
        this._data = _data;
        IsDraggingOver = false;

        if (boxStyle == null)
        {
            boxStyle = new GUIStyle(GUI.skin.box);
            boxStyle.padding = new RectOffset();
            boxStyle.margin = new RectOffset();
        }
    }

    public float Width
    {
        get
        {
            return PIXELS_PER_SECOND * MoveFrame.Duration;
        }
    }


    public bool Selected { get; set; }

    /// <summary>
    /// Draw the actual bar
    /// </summary>
    public void Draw()
    {
        try
        {
            GUILayout.BeginVertical(GUILayout.ExpandHeight(true), GUILayout.Width(Width));
            {
                if (Selected)
                    GUI.color = new Color(0.2f, 0.2f, 0.2f);
                GUILayout.Box(MoveFrame.FrameName, boxStyle, GUILayout.Width(Width), GUILayout.Height(35));
                GUI.color = Color.white;

                currentDurationString = GUILayout.TextField(currentDurationString);

                try
                {
                    var nextValue = float.Parse(currentDurationString);
                    if ("" + nextValue == currentDurationString)
                        MoveFrame.Duration = nextValue;
                }
                catch
                {

                }

                if (GUILayout.Button("X", GUILayout.ExpandWidth(true)))
                    CombomanEditor.Instance.MovesTab.OnRemoveFrame(MoveFrame);
            }
            GUILayout.EndVertical();
        }
        catch
        {

        }
        /*
        if (Selected)
            GUI.color = new Color(0.2f, 0.2f, 0.2f);
        GUILayout.Box(MoveFrame.FrameName, GUILayout.ExpandWidth(true), GUILayout.Height(35));
        GUI.color = Color.white;

        DrawChar();
        */
        CheckSelected();
        CheckDragBegin();
    }

    private void DrawChar()
    {
        var sprites = _data.LoadSprites();
        var frame = Frame.CreateFrame(MoveFrame.GetFrame(_data), sprites);

        Texture t   = frame.Sprite.texture;
        Rect tr     = frame.Sprite.textureRect;
        Rect r      = new Rect(tr.x / t.width, tr.y / t.height, tr.width / t.width, tr.height / t.height);

        var rect = GUILayoutUtility.GetLastRect();
        var R = new Rect(rect.x, rect.y, WIDTH, WIDTH);
        GUIUtil.DrawFrame(R);

        var sx = tr.width / WIDTH;
        var sy = tr.height / WIDTH;

        var scale = Mathf.Max(sx, sy);

        var area = new Rect(rect.x, rect.y, tr.width * scale, tr.height * scale);
        area.x += WIDTH / 2 - area.width / 2;
        area.y += WIDTH / 2 - area.height / 2;

        GUI.DrawTextureWithTexCoords(area, t, r);
    }

    private void CheckSelected()
    {
        var rect = GUILayoutUtility.GetLastRect();

        if (rect.Contains(Event.current.mousePosition))
        {
            if (Event.current.type == EventType.MouseDown)
            {
                if (!Selected)
                {
                    Selected = true;
                    CombomanEditor.Instance.MovesTab.OnSelectFrame(MoveFrame);
                    CombomanEditor.Instance.RequestRepaint();
                }
            }
        }
    }

    /// <summary>
    /// Check to see if the dragging has begun
    /// </summary>
    private void CheckDragBegin()
    {
        var rt = GUILayoutUtility.GetLastRect();
        var dm = DragManager.Instance;
        IsDraggingOver = dm.IsDraggingOver(rt);

        // Get the bounds
        Bounds = GUILayoutUtility.GetLastRect();

        if (!dm.IsDragging && IsDraggingOver)
            dm.Bar = this;

        if (IsDraggingOver)
            IsDraggingBefore = (rt.x + rt.width / 2) >= Event.current.mousePosition.x;
    }
    public MoveFrame MoveFrame { get; private set; }
    public Rect Bounds { get; private set; }
}
