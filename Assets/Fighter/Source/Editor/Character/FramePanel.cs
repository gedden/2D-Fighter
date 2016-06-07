using Comboman;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

public class FramePanel
{
    private static Texture2D backgroundTexture, hitboxButtonTexture;
    public GUIBox HitBox, AttackBox;

    /// <summary>
    /// Class Constructor
    /// </summary>
    public FramePanel() : base()
    {
        if (backgroundTexture == null)
        {
            backgroundTexture = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/Fighter/Artwork/Editor/checker.png");
            backgroundTexture.wrapMode = TextureWrapMode.Repeat;
        }

        if(hitboxButtonTexture == null)
            hitboxButtonTexture = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/Fighter/Artwork/Editor/HitboxSelect.png");



        HitBox = new GUIBox("Hit", Color.yellow, hitboxButtonTexture);
        AttackBox = new GUIBox("Atk", Color.red, hitboxButtonTexture);
    }

    /// <summary>
    /// Clear it out
    /// </summary>
    public void Clear()
    {
        data = null;
        frame = null;
    }

    public FrameData data { get; private set; }
    public Frame frame { get; private set; }

    /// <summary>
    /// Called when a frame data is selected
    /// </summary>
    /// <param name="data"></param>
    public void SetFrameData(FrameData data)
    {
        if (this.data == data)
            return;

        this.data = data;
        var sprites = Character.LoadSprites();

        frame = Frame.CreateFrame(data, sprites);

        HitBox.Data = data.Hitbox;
        HitBox.Enabled = data.HasHitbox;

        AttackBox.Data = data.Attackbox;
        AttackBox.Enabled = data.HasAttackbox;

        CombomanEditor.Instance.RequestRepaint();

        this.data = data;
    }

    public void Save()
    {
        // Update the frame data
        Character.UpdateFrame(frame.Sprite, HitBox.Enabled, HitBox.Data, AttackBox.Enabled, AttackBox.Data);
    }

    public CharacterData Character
    {
        get
        {
            return CombomanEditor.Instance.Character;
        }
        set
        {
            CombomanEditor.Instance.Character = value;
        }
    }

    private float _scale = 1.0f;
    public float Scale
    {
        get
        {
            return _scale;
        }
        set
        {
            _scale = value;
            //CombomanEditor.Instance.ViewScale
            HitBox.GlobalScale = value;
            AttackBox.GlobalScale = value;
        }
    }

    public bool IsValid
    {
        get
        {
            if (data == null) return false;
            if (frame == null) return false;
            return true;
        }
    }

    public void Draw(GUILayoutOption layout)
    {
        if (!IsValid) return;

        // Draw the character
        GUILayout.BeginHorizontal(GUILayout.ExpandWidth(true), layout);
        {
            GUILayout.FlexibleSpace();
            var tx = frame.Sprite.textureRect;

            tx.width *= Scale;
            tx.height *= Scale;
            GUILayout.BeginVertical(GUILayout.ExpandHeight(true));
            {
                GUILayout.FlexibleSpace();
                GUILayout.Box("", GUILayout.Width(tx.width), GUILayout.Height(tx.height));

                var charRect = GUILayoutUtility.GetLastRect();
                DrawBackground(charRect);
                DrawCharacter(charRect);
                HitBox.Draw(charRect);
                AttackBox.Draw(charRect);
            }
            GUILayout.EndVertical();
            GUILayout.FlexibleSpace();
        }
        GUILayout.EndHorizontal();
    }

    public void Draw()
    {
        Draw(GUILayout.ExpandHeight(true));
    }

    /// <summary>
    /// Returns true if there are any pending changes
    /// </summary>
    /// <returns></returns>
    public bool HasPendingChanges()
    {
        if (HitBox.Enabled != frame.Data.HasHitbox)
            return true;

        if (HitBox.Data != frame.Data.Hitbox)
            return true;

        if (AttackBox.Enabled != frame.Data.HasAttackbox)
            return true;

        if (AttackBox.Data != frame.Data.Attackbox)
            return true;
        return false;
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

        var area = new Rect(rect.x, rect.y, tr.width * Scale, tr.height * Scale);
        area.x += rect.width / 2 - area.width / 2;
        area.y += rect.height / 2 - area.height / 2;

        // Render the character
        GUI.DrawTextureWithTexCoords(area, t, r);
    }
}
