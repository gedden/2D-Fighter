using Comboman;
using System;
using UnityEditor;
using UnityEngine;

public class CombomanTab : CombomanPanel
{
    private static readonly Color UNSELECTED = new Color(0.9f, 0.9f, 0.9f);
    protected string TabName = "Tab Name";
    public CombomanTab()
    {
        Selected = false;
    }

    public void DrawTab()
    {
        GUI.color = Selected ? Color.white : UNSELECTED;

        if (GUILayout.Button(TabName, EditorStyles.toolbarButton))
            CombomanEditor.Instance.DoSelect(this);

        GUI.color = Color.white;
    }

    

    public bool Selected { get; set; }

    public override void Draw()
    {
        GUILayout.Box("Box", GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true));
    }

    public override void OnCharacterLoaded(CharacterData data)
    {
        throw new NotImplementedException();
    }

    public virtual void OnSelect()
    {

    }
}
