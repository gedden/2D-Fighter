using Comboman;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

public class TimelinePanel : IDrawablePanel
{
    private static readonly float CONTROL_WIDTH = 150;
    public List<Bar> Bars;
    private MovesTab _tab;
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

        if (rect.Contains(Event.current.mousePosition) && Event.current.type == EventType.mouseDrag)
            Debug.LogWarning("OVER!" + Event.current.mousePosition + " t" + Event.current.type);
        else Debug.Log("RECT!" + rect + " vs " + Event.current.mousePosition  + " t " + Event.current.type);
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
}
