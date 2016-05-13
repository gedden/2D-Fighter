using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

class FrameSequenceEditor
{
    public Rect windowRect = new Rect(20, 20, 120, 50);

    public FrameSequenceEditor()
    {
        
    }

    public void OnGUI()
    {
        windowRect = GUILayout.Window(0, windowRect, OnDrawWindow, "My Window");
    }

    public void OnDrawWindow(int id)
    {
        Debug.Log("ON DAW WINDOW" + id);
        GUILayout.Button("Hi");
    }
}
