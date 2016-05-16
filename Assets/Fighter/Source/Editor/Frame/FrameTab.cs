using Comboman;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

class FrameTab : CombomanTab
{
    private FrameData data;
    private Frame frame;
    private Texture2D backgroundTexture;
    private Texture2D hitboxButtonTexture;
    
    private bool editAttackbox;

    private GUIBox HitBox;

    /// <summary>
    /// Class Constructor
    /// </summary>
    public FrameTab() : base()
    {
        TabName = "Frame Tab";
        
        backgroundTexture = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/Fighter/Artwork/Editor/checker.png");
        backgroundTexture.wrapMode = TextureWrapMode.Repeat;

        hitboxButtonTexture = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/Fighter/Artwork/Editor/HitboxSelect.png");
        HitBox = new GUIBox(Color.yellow);
    }

    /// <summary>
    /// Called when a frame data is selected
    /// </summary>
    /// <param name="data"></param>
    public void OnSelectFrame(FrameData data)
    {
        var sprites = Character.LoadSprites();

        frame = Frame.CreateFrame(data, sprites);
        this.data = data;
    }

    public override void Draw()
    {
        if (data == null) return;
        if (frame == null) return;
        GUILayout.Box("", GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
        var tabRect = new Rect(GUILayoutUtility.GetLastRect());

        
        // Draw the background
        DrawBackground(tabRect);

        // Draw the character
        DrawCharacter(tabRect);

        HitBox.Draw(tabRect);

        // Draw the tools
        DrawTools(tabRect);
    }

    private void DrawTools(Rect dim)
    {
        var size = new Rect(CombomanEditor.LEFT_CONTROL_WIDTH + dim.x + 15, dim.y + 45, 35, 100);
        GUILayout.BeginArea(size);
        //GUILayout.Box("asdf", GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
        HitBox.Selected = DrawToolButton(HitBox.Selected, hitboxButtonTexture);
        editAttackbox = DrawToolButton(editAttackbox, hitboxButtonTexture);
        //GUILayout.Button("test");
        //GUILayout.Button("test");
        GUILayout.EndArea();
    }

    private bool DrawToolButton(bool value, Texture2D texture)
    {
        GUI.color = value ? Color.gray : Color.white;

        var next = GUILayout.Button(texture);

        GUI.color = Color.white;
        return next?!value:value;
    }

    /// <summary>
    /// Draw the checkerd background texture
    /// </summary>
    /// <param name="dim"></param>
    private void DrawBackground(Rect dim)
    {
        GUI.DrawTextureWithTexCoords(dim, backgroundTexture, new Rect(0, 0, dim.width / backgroundTexture.width, dim.height / backgroundTexture.height));
    }

    /// <summary>
    /// Draw the actual character
    /// </summary>
    /// <param name="rect"></param>
    private void DrawCharacter(Rect rect)
    {
        Texture t = frame.Sprite.texture;
        Rect tr = frame.Sprite.textureRect;
        Rect r = new Rect(tr.x / t.width, tr.y / t.height, tr.width / t.width, tr.height / t.height);


        var sx = rect.width / tr.width;
        var sy = rect.height / tr.height;

        var scale = 1.0f;// Mathf.Min(sx, sy);

        var area = new Rect(rect.x, rect.y, tr.width * scale, tr.height * scale);
        area.x += rect.width / 2 - area.width / 2;
        area.y += rect.height / 2 - area.height / 2;

        // Render the character
        GUI.DrawTextureWithTexCoords(area, t, r);
    }

    public override void OnCharacterLoaded(CharacterData data)
    {
        //this.character = data;
        frame = null;
        data = null;
    }
}
