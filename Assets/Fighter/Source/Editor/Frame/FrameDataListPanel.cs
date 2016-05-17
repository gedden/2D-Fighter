using UnityEditor;
using UnityEngine;
using Comboman;
using System.Collections.Generic;

public class FrameDataListPanel : ICombomanPanel
{
    GUIStyle _style = null;
    GUIStyle _mainbox = null;

    Vector2 scrollPos = Vector2.zero;

    private List<FrameDataPanel> panels;

    /// <summary>
    /// Class Constructor
    /// </summary>
    public FrameDataListPanel()
    {
        if (_style == null)
        {
            _style = new GUIStyle(GUI.skin.box);
            _style.normal.background = Texture2D.blackTexture;
        }

        panels = new List<FrameDataPanel>();
    }

    public void DoAddMissingFrames()
    {
        Character.DoAddMissingFrames();
        OnCharacterLoaded(Character);
    }

    /// <summary>
    /// height property
    /// </summary>
    public static float Height
    {
        get
        {
            return 120f;
        }
    }
    public static float LocalHeight
    {
        get
        {
            return Height + 10;
        }
    }


    public void Draw()
    {
        if( CombomanEditor.Instance.Character == null )
        {
            GUILayout.Box("No Character Loaded", GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true), GUILayout.MaxHeight(LocalHeight), GUILayout.MinHeight(LocalHeight));
            return;
        }

        GUILayout.BeginHorizontal(new GUIStyle(GUI.skin.box), GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true), GUILayout.MaxHeight(LocalHeight), GUILayout.MinHeight(LocalHeight));
        {
            // Draw the list controls
            GUILayout.BeginVertical(GUILayout.MaxWidth(75), GUILayout.MinWidth(75), GUILayout.ExpandHeight(true), GUILayout.MaxHeight(LocalHeight), GUILayout.MinHeight(LocalHeight));
            {
                if (GUILayout.Button("Add Missing"))
                    DoAddMissingFrames();
            }
            GUILayout.EndVertical();

            scrollPos = GUILayout.BeginScrollView(scrollPos, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true), GUILayout.MaxHeight(LocalHeight), GUILayout.MinHeight(LocalHeight));

            GUILayout.BeginHorizontal();
            {
                foreach (var panel in panels)
                    panel.Draw();
            }
            GUILayout.EndHorizontal();

            GUILayout.EndScrollView();
        }
        GUILayout.EndHorizontal();
        
    }

    /// <summary>
    /// Clear all the panels
    /// </summary>
    public void Clear()
    {
        panels.Clear();
        scrollPos = Vector2.zero;
    }

    public CharacterData Character
    {
        get
        {
            return CombomanEditor.Instance.Character;
        }
    }


    public void OnCharacterLoaded(CharacterData data)
    {
        Clear();

        foreach (var frame in data.Frames)
        {
            var panel = new FrameDataPanel(data, frame);
            panels.Add(panel);
        }
    }
}
