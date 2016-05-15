using UnityEngine;
using UnityEditor;
using Comboman;
using System.Collections.Generic;

public class CombomanEditor : EditorWindow
{
    FrameSequenceEditor editor = null;
    CombomanControlPanel control = null ;
    FrameDataListPanel frames = null;
    CombomanContentWindow content = null;

    private List<CombomanTab> tabs = null;

    public static CombomanEditor Instance { get; private set; }


    // Add menu named "My Window" to the Window menu
    [MenuItem("Window/Comboman Editor")]
    static void Init()
    {
        
        var window = GetWindow<CombomanEditor>();
        window.Character = null;
        Instance = window;
        window.SetupTabs();
        
        window.position = new Rect(200, 200, 1000, 600);
    }

    private void SetupTabs()
    {
        tabs = new List<CombomanTab>();
        for(int x=0;x<6;x++ )
        {
            var tab = new CombomanTab();
            tabs.Add(tab);
        }
    }

    /// <summary>
    /// Load a character
    /// </summary>
    public void LoadCharacter()
    {
        var path = EditorUtility.OpenFilePanel(
                "Load Character XML",
                CharacterData.CHARACTER_DATA_PATH,
                "xml");
        if (path.Length == 0)
            return;

        Character = CharacterData.Read(path);
        OnCharacterLoaded();
    }

    /// <summary>
    /// Save a character's xml file!
    /// </summary>
    public void SaveCharacter()
    {
        Character.Write();
    }

    /// <summary>
    /// Create a new character from a sprite
    /// </summary>
    public void CreateCharacter()
    {
        var path = EditorUtility.OpenFilePanel(
                "Create Character",
                "Assets/Fighter/Artwork/Resources/Characters",
                "*.*");

        if (path.Length == 0)
            return;

        // Cull the path
        path = path.Substring(path.LastIndexOf("/")+1);

        // remove the file name
        path = path.Substring(0, path.LastIndexOf("."));

        Character = CharacterData.Create(path);
        OnCharacterLoaded();
    }

    /// <summary>
    /// Called once a character is laoded
    /// </summary>
    private void OnCharacterLoaded()
    {
        // Assign the left panel
        control.OnCharacterLoaded(Character);
        frames.OnCharacterLoaded(Character);
    }


    public CharacterData Character { get; set; }



    /// <summary>
    /// GUI Info
    /// </summary>
    void OnGUI()
    {
        Instance = this;

        // The toolbar
        GUILayout.BeginHorizontal(EditorStyles.toolbar);

        if (GUILayout.Button("Create", EditorStyles.toolbarButton))
            CreateCharacter();

        if (GUILayout.Button("Load", EditorStyles.toolbarButton))
            LoadCharacter();

        // For saving a character
        GUI.enabled = Character != null;
        if (Character != null && Character.Dirty)
            GUI.color = Color.green;
        if (GUILayout.Button("Save", EditorStyles.toolbarButton))
            SaveCharacter();
           GUI.color = Color.white;
        GUI.enabled = true;

        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();


        // Main Area
        var styleLeftView = new GUIStyle(GUI.skin.scrollView);

        var last = GUILayoutUtility.GetLastRect();


        GUILayout.BeginHorizontal(styleLeftView);
        {
            // Left Controls
            GUILayout.BeginVertical(GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true), GUILayout.MaxWidth(300));
            Control.Draw();
            GUILayout.EndVertical();

            // Main Content Area

            if (tabs == null || Character == null)
            {
                GUILayout.Box("No Character Selected", GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            }
            else
            {
                GUILayout.BeginVertical(GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
                {
                    // Tab bar
                    GUILayout.BeginHorizontal(EditorStyles.toolbar);
                    {
                        foreach (var tab in tabs)
                            tab.DrawTab();
                    }
                    GUILayout.EndHorizontal();

                    GUILayout.BeginHorizontal(GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
                    // Content 
                    foreach (var tab in tabs)
                        if (tab.Selected)
                            tab.Draw();
                    GUILayout.EndHorizontal();
                }
                GUILayout.EndVertical();
            }
        }
        GUILayout.EndHorizontal();


        GUILayout.BeginVertical();

        // Bottom Area
        Frames.Draw();

        GUILayout.EndVertical();




    }

    private CombomanControlPanel Control
    {
        get
        {
            if (control == null)
                control = new CombomanControlPanel();
            return control;
        }
    }

    private FrameDataListPanel Frames
    {
        get
        {
            if (frames == null)
                frames = new FrameDataListPanel();
            return frames;
        }
    }

    private CombomanTab _selected = null;
    public void DoSelect(CombomanTab tab)
    {
        if (_selected != null)
            _selected.Selected = false;
        tab.Selected = true;
        _selected = tab;
    }


}