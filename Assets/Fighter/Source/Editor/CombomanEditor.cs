using UnityEngine;
using UnityEditor;
using Comboman;

public class CombomanEditor : EditorWindow
{
    FrameSequenceEditor editor = null;
    CombomanControlPanel control = null ;
    CombomanContentWindow content = null;

    // Add menu named "My Window" to the Window menu
    [MenuItem("Window/Comboman Editor")]
    static void Init()
    {
        var window = GetWindow<CombomanEditor>();
        window.position = new Rect(200, 200, 1000, 600);
        window.control = new CombomanControlPanel();
    }

    /// <summary>
    /// Load a character
    /// </summary>
    public void LoadCharacter()
    {
        var path = EditorUtility.OpenFilePanel(
                "Load Character XML",
                "Assets/Fighter/Data",
                "xml");
        if (path.Length == 0)
            return;

        var data = CharacterData.Read(path);

        // Assign the left panel
        control.CharacterData = data;
    }



    /// <summary>
    /// GUI Info
    /// </summary>
    void OnGUI()
    {

        // The toolbar
        GUILayout.BeginHorizontal(EditorStyles.toolbar);

        if (GUILayout.Button("Load", EditorStyles.toolbarButton))
            LoadCharacter();

        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();


        // Main Area
        var styleLeftView = new GUIStyle(GUI.skin.scrollView);

        var last = GUILayoutUtility.GetLastRect();


        GUILayout.BeginHorizontal(styleLeftView);

            // Left Controls
            GUILayout.BeginVertical(GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true), GUILayout.MaxWidth(300));
            //GUILayout.Button("Just at test");
            control.Draw();
            //GUILayout.Box("Left Box " + last.height, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true), GUILayout.MaxWidth(300));
            GUILayout.EndVertical();

            // Main Content Area
            GUILayout.Box("Main Box", GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));

        GUILayout.EndHorizontal();


        GUILayout.BeginVertical();

        // Bottom Area
        GUILayout.Box("Lower Box", GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true), GUILayout.MaxHeight(200), GUILayout.MinHeight(200));

        GUILayout.EndVertical();




    }
    
}