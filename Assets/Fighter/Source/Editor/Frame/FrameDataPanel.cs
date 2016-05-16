using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;
using Comboman;

public class FrameDataPanel : ICombomanPanel
{
    
    GUIStyle styleRightView = null;
    private FrameData data;
    private Frame frame;
    private float scale = 1.0f;

    public FrameDataPanel(CharacterData character, FrameData data)
    {
        styleRightView = new GUIStyle(GUI.skin.box);

        var sprites = character.LoadSprites();
        frame = Frame.CreateFrame(data, sprites);
        this.data = data;
        Selected = false;
    }

    public bool Selected { get; set; }

    public FrameData FrameData { get { return data; } }

    /// <summary>
    /// Draw the actual frame panel
    /// </summary>
    public void Draw()
    {
        
        if( GUILayout.Toggle(Selected, data.SpriteName, GUILayout.Width(FrameDataListPanel.Height), GUILayout.Height(FrameDataListPanel.Height)) )
            CombomanEditor.Instance.DoSelect(this);
        //GUILayout.Box(data.SpriteName, styleRightView, GUILayout.Width(FrameDataListPanel.Height), GUILayout.Height(FrameDataListPanel.Height));

        var rect = GUILayoutUtility.GetLastRect();
        Texture t = frame.Sprite.texture;
        Rect tr = frame.Sprite.textureRect;
        Rect r = new Rect(tr.x / t.width, tr.y / t.height, tr.width / t.width, tr.height / t.height);

        var sx = tr.width / FrameDataListPanel.Height;
        var sy = tr.height / FrameDataListPanel.Height;

        scale = Mathf.Max(sx, sy);

        var area = new Rect(rect.x, rect.y, tr.width * scale, tr.height * scale);
        area.x += FrameDataListPanel.Height/2 - area.width / 2;
        area.y += FrameDataListPanel.Height / 2 - area.height / 2;

        GUI.DrawTexture(area, Texture2D.whiteTexture);
        GUI.DrawTextureWithTexCoords(area, t, r);
        

    }

    public void OnCharacterLoaded(CharacterData data)
    {
        throw new NotImplementedException();
    }
}
