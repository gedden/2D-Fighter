using UnityEditor;
using UnityEngine;
using Comboman;
using System.Collections.Generic;
using System;

public class CombomanControlPanel : CombomanPanel
{
    private static Texture2D _staticRectTexture;
    private static GUIStyle _staticRectStyle;
    private static int ID = 0;
    Vector2 scroll = Vector2.zero;
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
        if (_char == null || CombomanEditor.Instance.MovesTab == null)
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
        if (GUILayout.Button("Add Move"))
            CombomanEditor.Instance.AddMove();

        scroll = GUILayout.BeginScrollView(scroll, GUILayout.Height(300), GUILayout.ExpandWidth(true));
        {
            GUILayout.BeginVertical(GUILayout.ExpandWidth(true));
            {
                // get the currently selected move
                var move = CombomanEditor.Instance.MovesTab.Move;

                GUIStyle style = new GUIStyle(EditorStyles.toolbarButton);
                style.normal.background = Texture2D.whiteTexture;

                foreach (var m in Character.Moves)
                {
                    GUI.color = move == m ? Color.blue : Color.white;
                    if (GUILayout.Button(m.Name, style))
                        CombomanEditor.Instance.DoSelect(m);
                    GUI.color = Color.white;
                }
            }
            GUILayout.EndVertical();
        }
        GUILayout.EndScrollView();


        EditorGUILayout.LabelField("View Scale", "" + CombomanEditor.Instance.ViewScale);

        if (Event.current.type == EventType.scrollWheel)
            CombomanEditor.Instance.ViewScale -= Event.current.delta.y / 10.0f;
    }

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
