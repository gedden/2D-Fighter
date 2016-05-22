using Comboman;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

public class TimelinePanel : IDrawablePanel, IDragTarget
{
    private static readonly float CONTROL_WIDTH = 150;
    public List<Bar> Bars;
    private MovesTab _tab;
    private Vector2 _lastDragPos;

    /// <summary>
    /// Class Constructor
    /// </summary>
    /// <param name="tab"></param>
    public TimelinePanel(MovesTab tab)
    {
        _tab = tab;
        Bars = new List<Bar>();
    }

    public MoveData Move
    {
        get
        {
            return _tab.Move;
        }
    }

    /// <summary>
    /// Draw all the content
    /// </summary>
    public void Draw()
    {
        GUILayout.BeginVertical(GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true), GUILayout.MaxWidth(CONTROL_WIDTH));
        Move.Name = EditorGUILayout.TextField("Move Name: ",  Move.Name);

        GUILayout.EndVertical();

        GUILayout.BeginScrollView(Vector2.zero, GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true));
        {
            GUILayout.BeginHorizontal(GUILayout.ExpandHeight(true));
            {
                if (Bars.Count == 0)
                {
                    GUILayout.Box("Drag here to create a keyframe", GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true));
                }
                else
                {
                    foreach (var b in Bars)
                        b.Draw();
                    GUILayout.FlexibleSpace();
                }
            }
            GUILayout.EndHorizontal();
        }
        GUILayout.EndScrollView();


        var rect = GUILayoutUtility.GetLastRect();

        var dm = DragManager.Instance;
        if (dm.IsDragging && dm.IsDraggingOver(rect) && dm.Frame != null)
        {
            dm.Target = this;
        }
    }

    /// <summary>
    /// Add a bar to this list
    /// </summary>
    /// <param name="bar"></param>
    public void Add(Bar bar)
    {
        // Add a bar to this list
        Bars.Add(bar);
    }

    public void OnBeforeDragComplete()
    {
        var c = CombomanEditor.Instance.Character;
        var dm = DragManager.Instance;
        var frame = dm.Frame;

        // Create a new move frame
        var moveFrame = new MoveFrame(frame.Name, 0.5f);

        // Add a new move frame 
        Move.AddFrame(moveFrame);

        
        c.Dirty = true;
        ReloadBars();
    }


    public void ReloadBars()
    {
        Bars.Clear();

        foreach( var frame in Move.MoveFrames )
        {
            var bar = new Bar(frame, CombomanEditor.Instance.Character);
            Add(bar);
        }
    }
}
