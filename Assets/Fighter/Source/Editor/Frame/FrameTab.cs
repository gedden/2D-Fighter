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
    private Texture2D hitboxButtonTexture, saveButtonTexture;
    
    private bool editAttackbox;

    private GUIBox HitBox, AttackBox;

    /// <summary>
    /// Class Constructor
    /// </summary>
    public FrameTab() : base()
    {
        TabName = "Frame Tab";
        
        backgroundTexture = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/Fighter/Artwork/Editor/checker.png");
        backgroundTexture.wrapMode = TextureWrapMode.Repeat;

        hitboxButtonTexture = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/Fighter/Artwork/Editor/HitboxSelect.png");
        saveButtonTexture = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/Fighter/Artwork/Editor/save-icon.png");
        HitBox = new GUIBox("Hit", Color.yellow, hitboxButtonTexture);
        AttackBox = new GUIBox("Atk", Color.red, hitboxButtonTexture);
    }

    /// <summary>
    /// Called when a frame data is selected
    /// </summary>
    /// <param name="data"></param>
    public void OnSelectFrame(FrameData data)
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

    public override void Draw()
    {
        if (data == null) return;
        if (frame == null) return;
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
            //GUILayout.Box("big", GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            GUILayout.BeginHorizontal(GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            {
                GUILayout.FlexibleSpace();
                var tx = frame.Sprite.textureRect;
                tx.width *= CombomanEditor.Instance.ViewScale;
                tx.height *= CombomanEditor.Instance.ViewScale;
                GUILayout.BeginVertical(GUILayout.ExpandHeight(true));
                {
                    GUILayout.FlexibleSpace();
                    GUILayout.Box("asdf", GUILayout.Width(tx.width), GUILayout.Height(tx.height));

                    var charRect = GUILayoutUtility.GetLastRect();
                    DrawBackground(charRect);
                    DrawCharacter(charRect);
                    HitBox.Draw(charRect);
                    AttackBox.Draw(charRect);

                    GUILayout.FlexibleSpace();
                }
                GUILayout.EndVertical();
                GUILayout.FlexibleSpace();
            }
            GUILayout.EndHorizontal();

        }
        GUILayout.EndHorizontal();
    }

    /// <summary>
    /// Returns true if there are any pending changes
    /// </summary>
    /// <returns></returns>
    private bool HasPendingChanges()
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

    private void Save()
    {
        // Update the frame data
        Character.UpdateFrame(frame.Sprite, HitBox.Enabled, HitBox.Data, AttackBox.Enabled, AttackBox.Data);
    }

    /// <summary>
    /// Draw the tools
    /// </summary>
    private void DrawTools()
    {
        HitBox.Selected = HitBox.DrawToolButton();
        AttackBox.Selected = AttackBox.DrawToolButton();

        GUI.color = HasPendingChanges() ? Color.green : Color.white;
        if (GUILayout.Button(saveButtonTexture))
            Save();
        GUI.color = Color.white;
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

        var scale = CombomanEditor.Instance.ViewScale;

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
