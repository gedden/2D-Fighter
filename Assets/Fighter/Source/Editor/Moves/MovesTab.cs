using Comboman;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
public class MovesTab : CombomanTab
{
    private FramePanel panel;
    private TimelinePanel Timeline;
    private Sequence _current = null;

    private bool _playing = false;

    /// <summary>
    /// Class Constructor
    /// </summary>
    public MovesTab() : base()
    {
        TabName = "Moves Tab";

        Timeline = new TimelinePanel(this);
        panel = new FramePanel();
    }

    /// <summary>
    /// Draw all the things
    /// </summary>
    public override void Draw()
    {
        UpdateAnimation();
        //var last = GUILayoutUtility.GetLastRect();

        GUILayout.BeginVertical(GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true));

        if (Move == null || !panel.IsValid)
        {
            GUILayout.Box("Animation Preview", GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true));
        }
        else
        {
            panel.Scale = CombomanEditor.Instance.ViewScale;

            var height = panel.frame.Sprite.textureRect.height * panel.Scale;
            height = Mathf.Pow(2.0f, Mathf.Ceil(Mathf.Log(height) / Mathf.Log(2.0f)));

            panel.Draw(GUILayout.Height(height));
        }

        // Draw the move
        DrawMoveTimeline();
        GUILayout.EndVertical();
        
    }

    /// <summary>
    /// Draw the timeline!
    /// </summary>
    private void DrawMoveTimeline()
    {
        if (Move == null)
        {
            GUILayout.Box("No Move Selected", GUILayout.Height(250), GUILayout.ExpandWidth(true));
            return;
        }
        Timeline.Draw();
    }

    /// <summary>
    /// On select frame
    /// </summary>
    public void OnSelectFrame(MoveFrame _frame)
    {
        // De-select whatever the currently selected bar is
        foreach (var bar in Timeline.Bars)
        {
            if (bar.Selected)
            {
                if (bar.MoveFrame != _frame)
                    bar.Selected = false;
            }
        }

        // Timeline.ReloadBars();
        panel.SetFrameData(_frame.GetFrame(Character));
    }

    public void OnRemoveFrame(MoveFrame _frame)
    {
        Move.Remove(_frame);
        Timeline.ReloadBars();
    }

    public MoveData Move { get; set; }

    public override void OnSelect()
    {
        Timeline.ReloadBars();
    }

    /// <summary>
    /// Do the play
    /// </summary>
    public void DoPlay()
    {
        _playing = true;
        _current = new Sequence(Sequence.Now, Character, Move);
    }

    /// <summary>
    /// Do stop
    /// </summary>
    public void DoStop()
    {
        _playing = false;
        _current = null;
    }

    public void UpdateAnimation()
    {
        if (_current == null)
            return;
        
        // Get the current frame
        var frameData = _current.GetFrame();

        if (frameData == null)
        {
            panel.Clear();
            return;
        }

        panel.SetFrameData(frameData);
        var t = (Sequence.Now+_current.Start) % Move.Duration;
        Timeline.MarkTime = t;
        CombomanEditor.Instance.RequestRepaint();

    }

    public bool Playing
    {
        get
        {
            return _playing;
        }
    }

}
