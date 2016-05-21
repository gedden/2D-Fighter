using Comboman;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

public class MovesTab : CombomanTab
{
    private TimelinePanel Timeline;

    /// <summary>
    /// Class Constructor
    /// </summary>
    public MovesTab() : base()
    {
        TabName = "Moves Tab";

        Timeline = new TimelinePanel(this);
    }

    /// <summary>
    /// Draw all the things
    /// </summary>
    public override void Draw()
    {
        //var last = GUILayoutUtility.GetLastRect();

        GUILayout.BeginVertical(GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true));
        GUILayout.Box("Animation Preview", GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true));

        // Draw the move
        DrawMove();
        GUILayout.EndVertical();
    }

    private void DrawMove()
    {
        if (Move == null)
        {
            GUILayout.Box("No Move Selected", GUILayout.Height(250), GUILayout.ExpandWidth(true));
            return;
        }
        //GUILayout.Box("Background", GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true));


        Timeline.Draw();



        /*
         * var last = GUILayoutUtility.GetLastRect();
        Timeline.Position = new Vector2(last.x, last.y);
        Timeline.Width = last.width;
        Timeline.Height = last.height;

        Timeline.Draw();
        */

    }

    public MoveData Move { get; set; }
}
