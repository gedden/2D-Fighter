﻿using UnityEngine;
using UnityEditor;
using Comboman;
using System.Collections.Generic;
using Owl;

[ExecuteInEditMode]
public class CombomanEditor : EditorWindow
{
    CombomanControlPanel control = null ;
    FrameDataListPanel frames = null;
    public static readonly float LEFT_CONTROL_WIDTH = 300;

    private List<CombomanTab> tabs = null;
    private FrameTab frameTab = null;
    private MovesTab moveTab = null;
    private float _scale = 1.0f;

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

    public float ViewScale
    {
        get
        {
            return _scale;
        }
        set
        {
            var last = _scale;

            if (value < 1)
                _scale = 1.0f;
            else
                _scale = value;

            if (last != _scale)
                RequestRepaint();
        }
    }

    /// <summary>
    /// Setup the tabs
    /// </summary>
    private void SetupTabs()
    {
        tabs = new List<CombomanTab>();

        frameTab = new FrameTab();
        tabs.Add(frameTab);

        moveTab = new MovesTab();
        tabs.Add(moveTab);


        for (int x=0;x<2;x++ )
        {
            var tab = new CombomanTab();
            tabs.Add(tab);
        }
    }

    public void Update()
    {
        if (_repaint)
        {
            Repaint();
            _repaint = false;
        }
    }

    private bool _repaint = false;
    public void RequestRepaint()
    {
        _repaint = true;
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
        AddMissingBasicMoves();
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


        if (GUILayout.Button("Add Missing", EditorStyles.toolbarButton))
        {
            frames.DoAddMissingFrames();
            AddMissingBasicMoves();
        }

            GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();


        // Main Area
        var styleLeftView = new GUIStyle(GUI.skin.scrollView);

        GUILayout.BeginHorizontal(styleLeftView);
        {
            // Left Controls
            GUILayout.BeginVertical(GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true), GUILayout.MaxWidth(LEFT_CONTROL_WIDTH));
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
        DragManager.Instance.Update();
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
        tab.OnSelect();
    }


    private FrameDataPanel _selectedFrame = null;
    public void DoSelect(FrameDataPanel frame)
    {
        var last = _selectedFrame;

        if (_selectedFrame != null)
            _selectedFrame.Selected = false;

        _selectedFrame = frame;
        _selectedFrame.Selected = true;

        if (frameTab == null)
            SetupTabs();
        

        frameTab.OnSelectFrame(frame.FrameData);

        // Make the tab the selection
        if( last == null )
            DoSelect(frameTab);
    }

    public void DoSelect(MoveData move)
    {
        moveTab.Move = move;
        DoSelect(moveTab);
    }

    public MovesTab MovesTab
    {
        get
        {
            return moveTab;
        }
    }


    public void AddMove()
    {
        // Create a new move
        MoveData move = new MoveData("New Move #" + Character.Moves.Count);
        Character.AddMove(move);
    }

    public void DeleteMove(MoveData move)
    {
        // Create a new move
        Character.DeleteMove(move);
    }

    public void AddMissingBasicMoves()
    {
        var moves = Character.Moves;

        foreach( var t in OwlUtil.GetValues<MoveType>() )
        {
            if (!t.IsBasicMove())
                continue;
            var found = false;

            foreach (var m in moves)
                if (m.MoveType == t)
                {
                    found = true;
                    break;
                }

            // Create a new move
            if( !found )
            {
                var move = new MoveData(t);
                Character.AddMove(move);
            }
        }
    }


}