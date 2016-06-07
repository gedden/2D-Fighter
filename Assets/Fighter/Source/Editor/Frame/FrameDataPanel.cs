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

    /// <summary>
    /// Class constructor
    /// </summary>
    /// <param name="character"></param>
    /// <param name="data"></param>
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
    /// Check to see drag begin
    /// </summary>
    private void CheckDragBegin()
    {
        var rt = GUILayoutUtility.GetLastRect();
        var dm = DragManager.Instance;
        var val = dm.IsDraggingOver(rt);


        if (!dm.IsDragging && val)
        {
            dm.Frame = frame;
        }
    }

    /// <summary>
    /// Draw the actual frame panel
    /// </summary>
    public void Draw()
    {
        var s = new GUIStyle(GUI.skin.box);
        s.normal.background = Texture2D.blackTexture;


        /*
        if (GUILayout.Toggle(Selected, data.SpriteName, GUILayout.Width(FrameDataListPanel.Height), GUILayout.Height(FrameDataListPanel.Height)))
        {
            CheckDragBegin();
            CombomanEditor.Instance.DoSelect(this);
        }

        */

        if (Selected)
            GUI.color = new Color(0.2f, 0.2f, 0.2f);
        GUILayout.Box(data.SpriteName, GUILayout.Width(FrameDataListPanel.Height), GUILayout.Height(FrameDataListPanel.Height));
        GUI.color = Color.white;

        var rect = GUILayoutUtility.GetLastRect();

        if ( rect.Contains(Event.current.mousePosition) )
        {
            if( Event.current.type == EventType.MouseDown)
            {
                if( !Selected )
                {
                    Selected = true;
                    CombomanEditor.Instance.DoSelect(this);
                }
            }

            if (Selected)
                CheckDragBegin();
        }
        
        Texture t = frame.Sprite.texture;
        Rect tr = frame.Sprite.textureRect;
        Rect r = new Rect(tr.x / t.width, tr.y / t.height, tr.width / t.width, tr.height / t.height);

        var R = new Rect(rect.x, rect.y, FrameDataListPanel.Height, FrameDataListPanel.Height);
        GUIUtil.DrawFrame(R);

        var sx = tr.width / FrameDataListPanel.Height;
        var sy = tr.height / FrameDataListPanel.Height;

        scale = Mathf.Max(sx, sy);

        var area = new Rect(rect.x, rect.y, tr.width * scale, tr.height * scale);
        area.x += FrameDataListPanel.Height/2 - area.width / 2;
        area.y += FrameDataListPanel.Height / 2 - area.height / 2;

        GUI.DrawTextureWithTexCoords(area, t, r);
    }

    public void OnCharacterLoaded(CharacterData data)
    {
        throw new NotImplementedException();
    }
}
