using UnityEditor;
using UnityEngine;
using Comboman;
using System.Collections.Generic;
using System;

public class CombomanControlPanel : CombomanPanel
{
    private static int ID = 0;
    GUIStyle _style = null;
    

    /// <summary>
    /// Class Constructor
    /// </summary>
    public CombomanControlPanel()
    {
        ID++;

    }

    public override void Draw()
    {
        GUILayout.Space(4);

        var _char = CombomanEditor.Instance.Character;
        if (_char == null)
        {
            GUILayout.Box("No Character Loaded", GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true));
            return;
        }

        if( _style == null )
        {
            _style = new GUIStyle(GUI.skin.box);
            _style.normal.background = Texture2D.blackTexture;
        }

        EditorGUILayout.LabelField("Frames", ""+Character.Frames.Count);
        EditorGUILayout.LabelField("Sprites", "" + Character.LoadSprites().Length);

        GUILayout.TextField(_char.name);
        //if( GUILayout.Button("Add Frame Data") ) AddNewFrame();
        GUILayout.Button("Test 2 " + ID);

        //GUILayout.BeginArea(new Rect(0, 0, 200, 200), Texture2D.blackTexture);
        //GUILayout.Button("Test32 " + ID);
        //GUILayout.EndArea();

        var last = GUILayoutUtility.GetLastRect();
        var next = new Rect(6, last.yMax + EditorGUIUtility.singleLineHeight, last.width - 6, last.width - 6);
        GUIDrawRect(next, Color.black);
        //GUILayout.Space(next.height);
        GUILayout.Height(next.height);

        EditorGUILayout.LabelField("View Scale", "" + CombomanEditor.Instance.ViewScale);

        if (Event.current.type == EventType.scrollWheel)
            CombomanEditor.Instance.ViewScale -= Event.current.delta.y / 10.0f;
    }


    private static Texture2D _staticRectTexture;
    private static GUIStyle _staticRectStyle;

    // Note that this function is only meant to be called from OnGUI() functions.
    public static void GUIDrawRect(Rect position, Color color)
    {
        if (_staticRectTexture == null)
        {
            _staticRectTexture = new Texture2D(1, 1);
        }

        if (_staticRectStyle == null)
        {
            _staticRectStyle = new GUIStyle();
        }

        _staticRectTexture.SetPixel(0, 0, color);
        _staticRectTexture.Apply();

        _staticRectStyle.normal.background = _staticRectTexture;

        GUI.Box(position, GUIContent.none, _staticRectStyle);


    }

    public override void OnCharacterLoaded(CharacterData data)
    {
        //throw new NotImplementedException();
    }
}
