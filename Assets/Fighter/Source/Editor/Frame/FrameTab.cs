using Comboman;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

class FrameTab : CombomanTab
{
    private FramePanel panel;
    private static Texture2D saveButtonTexture;

    private bool editAttackbox;

    /// <summary>
    /// Class Constructor
    /// </summary>
    public FrameTab() : base()
    {
        TabName = "Frame Tab";

        panel = new FramePanel();

        if (saveButtonTexture == null)
            saveButtonTexture = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/Fighter/Artwork/Editor/save-icon.png");
    }


    /// <summary>
    /// Called when a frame data is selected
    /// </summary>
    /// <param name="data"></param>
    public void OnSelectFrame(FrameData data)
    {
        panel.SetFrameData(data);
        CombomanEditor.Instance.RequestRepaint();
    }

    public override void Draw()
    {
        if (panel.data == null) return;
        if (panel.frame == null) return;
        GUILayout.BeginHorizontal(GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
        {

            // Draw the tools
            //GUILayout.Box("asdf", GUILayout.Width(35), GUILayout.ExpandHeight(true));
            GUILayout.BeginVertical(GUILayout.Width(35), GUILayout.ExpandHeight(true));
            {
                //DrawTools(GUILayoutUtility.GetLastRect());
                DrawTools();
            }
            GUILayout.EndVertical();

            // Draw the character
            panel.Scale = CombomanEditor.Instance.ViewScale;
            panel.Draw();
        }
        GUILayout.EndHorizontal();
    }

    /// <summary>
    /// Draw the tools
    /// </summary>
    private void DrawTools()
    {
        panel.HitBox.Selected = panel.HitBox.DrawToolButton();
        panel.AttackBox.Selected = panel.AttackBox.DrawToolButton();

        GUI.color = panel.HasPendingChanges() ? Color.green : Color.white;
        if (GUILayout.Button(saveButtonTexture))
            panel.Save();
        GUI.color = Color.white;
    }
    
    /// <summary>
    /// On Character loaded
    /// </summary>
    /// <param name="data"></param>
    public override void OnCharacterLoaded(CharacterData data)
    {
        panel.Clear();
    }
}
