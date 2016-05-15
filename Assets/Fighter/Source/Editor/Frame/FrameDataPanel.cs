using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;
using Comboman;

class FrameDataPanel : ICombomanPanel
{
    GUIStyle styleRightView = null;
    private FrameData data;
    private Frame frame;

    public FrameDataPanel(CharacterData character, FrameData data)
    {
        styleRightView = new GUIStyle(GUI.skin.box);

        var sprites = character.LoadSprites();
        frame = Frame.CreateFrame(data, sprites);
        this.data = data;
    }

    public void Draw()
    {
        //pos = GUILayout.BeginScrollView(pos, GUILayout.ExpandHeight(true), GUILayout.Width(width), GUILayout.Height(300));
        //GUILayout.Box("No Character Loaded", styleRightView, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
        GUILayout.Box(data.SpriteName, styleRightView, GUILayout.Width(FrameDataListPanel.Height), GUILayout.Height(FrameDataListPanel.Height));


        var rect = GUILayoutUtility.GetLastRect();
        Texture t = frame.Sprite.texture;
        Rect tr = frame.Sprite.textureRect;
        Rect r = new Rect(tr.x / t.width, tr.y / t.height, tr.width / t.width, tr.height / t.height);

        GUI.DrawTextureWithTexCoords(new Rect(rect.x, rect.y, tr.width*2, tr.height*2), t, r);

    }

    public void OnCharacterLoaded(CharacterData data)
    {
        throw new NotImplementedException();
    }
}
