using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;
using Comboman;

class CombomanContentWindow
{
    public Rect windowRect = new Rect(20, 20, 120, 50);
    List<FrameDataEditor> frameWindows;
    Vector2 posRight;
    GUIStyle styleRightView = null;
    private readonly CombomanEditorWindow parent;

    public CombomanContentWindow(CombomanEditorWindow parent)
    {
        this.parent = parent;
        frameWindows = new List<FrameDataEditor>();
    }

    private CharacterData Data
    {
        get
        {
            return parent.Control.CharacterData;
        }
    }


    public void OnGUI()
    {
        if (styleRightView == null) styleRightView = new GUIStyle(GUI.skin.box);


        

        if (Data == null)
        {
            posRight = GUILayout.BeginScrollView(posRight, GUILayout.ExpandWidth(true));
            GUILayout.Box("No Character Loaded", styleRightView, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            GUILayout.EndScrollView();
        }
        else
        {
            UpdateFrameWindows();
            DrawFrameWindows();
        }



        
    }

    Vector2 p = Vector2.zero;
    private void DrawFrameWindows()
    {

        p = GUILayout.BeginScrollView(p, GUILayout.ExpandWidth(true), GUILayout.Height(350));
        GUILayout.BeginHorizontal("box");
        foreach (var win in frameWindows)
            win.OnGUI();
        GUILayout.EndHorizontal();
        GUILayout.EndScrollView();
    }

    private void UpdateFrameWindows()
    {
        if (frameWindows.Count == Data.Frames.Count) return;
        RefreshFrames();
    }

    private void AddNewFrame()
    {
        // Add a new frames
        var frame = new FrameData();
        Data.Frames.Add(frame);

        // Refresh the frames
        RefreshFrames();
    }

    private void RefreshFrames()
    {
        // Clear out the existing frames
        frameWindows.Clear();

        foreach (var frame in Data.Frames)
        {
            var window = new FrameDataEditor(Data, frame);
            frameWindows.Add(window);
        }
    }
}
