using UnityEditor;
using UnityEngine;
using Comboman;
using System.Collections.Generic;

public class CombomanControlPanel
{
    private static int ID = 0;
    GUIStyle _style = null;
    CharacterData _char;



    

    /// <summary>
    /// Class Constructor
    /// </summary>
    public CombomanControlPanel()
    {
        ID++;

    }

    public void Draw()
    {
        GUILayout.Space(4);
        if (_char == null)
        {
            GUILayout.Box("No Character Loaded", GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true));
            return;
        }

        if( _style == null )
        {
            _style = new GUIStyle(GUI.skin.box);
            _style.normal.background = Texture2D.blackTexture;
        }

        GUILayout.TextField(_char.name);
        //if( GUILayout.Button("Add Frame Data") ) AddNewFrame();
        GUILayout.Button("Test 2 " + ID);

        //GUILayout.BeginArea(new Rect(0, 0, 200, 200), Texture2D.blackTexture);
        //GUILayout.Button("Test32 " + ID);
        //GUILayout.EndArea();

        var last = GUILayoutUtility.GetLastRect();
        var next = new Rect(6, last.yMax + EditorGUIUtility.singleLineHeight, last.width - 6, last.width - 6);
        GUIDrawRect(next, Color.black);
        //GUILayout.Space(next.height);
        GUILayout.Height(next.height);

        // Make this auto show up after that last draw.... Maybe some kind of GUILayoutUtlity.SetLastRect?
        // GUILayout.Button("Test " + ID);
    }

    /// <summary>
    /// Character Data accessor
    /// </summary>
    public CharacterData CharacterData
    {
        get
        {
            return _char;
        }
        set
        {
            _char = value;

            // Assign the left window
        }
    }


    private static Texture2D _staticRectTexture;
    private static GUIStyle _staticRectStyle;

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
}
