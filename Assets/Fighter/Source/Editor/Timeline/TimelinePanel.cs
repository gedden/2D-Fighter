using Comboman;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

public class TimelinePanel : IDrawablePanel, IDragTarget
{
    private static readonly float CONTROL_WIDTH = 150;
    public List<Bar> Bars;
    private MovesTab _tab;
    private Vector2 _lastDragPos;
    private Bar _selected;
    private int _selectPosition = -1;

    /// <summary>
    /// Class Constructor
    /// </summary>
    /// <param name="tab"></param>
    public TimelinePanel(MovesTab tab)
    {
        _selectPosition = -1;
        _tab = tab;
        Bars = new List<Bar>();
    }

    public MoveData Move
    {
        get
        {
            return _tab.Move;
        }
    }

    private void DrawMark()
    {

    }

    private void DrawLine()
    {
        GUI.color = Color.green;
        GUILayout.Box("", GUILayout.ExpandHeight(true), GUILayout.Width(5));
        GUI.color = Color.white;
    }
    private void DrawEmptyLine()
    {
        GUILayout.Box("", GUILayout.ExpandHeight(true), GUILayout.Width(5));
    }

    private void DrawControls()
    {
        GUILayout.BeginHorizontal(EditorStyles.toolbar);

        if (GUILayout.Button("■", EditorStyles.toolbarButton))
            _tab.DoStop();

        if (GUILayout.Toggle(_tab.Playing, "►", EditorStyles.toolbarButton))
            _tab.DoPlay();

        if (GUILayout.Button("▌▌", EditorStyles.toolbarButton))
            Debug.Log("Create");

        if (GUILayout.Button("∞", EditorStyles.toolbarButton))
            Debug.Log("Create");



        GUILayout.EndHorizontal();
    }

    /// <summary>
    /// Draw all the content
    /// </summary>
    public void Draw()
    {
        // get the drag manager
        var dm = DragManager.Instance;

        GUILayout.BeginHorizontal(new GUIStyle(GUI.skin.box), GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
        {
            // Draw the left Panel
            GUILayout.BeginVertical(GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true), GUILayout.MaxWidth(CONTROL_WIDTH));
            {
                GUILayout.Label("Move Details Here");
                DrawControls();
                //Move.Name = EditorGUILayout.TextField("Move Name: ", Move.Name, GUILayout.MaxWidth(CONTROL_WIDTH));
                EditorGUILayout.TextField("Move Name: ", Move.Name);
                Move.MoveType = (MoveType)EditorGUILayout.EnumPopup(Move.MoveType);
                Move.Name = GUILayout.TextField(Move.Name);

                if (GUILayout.Button("Delete Move"))
                {
                    CombomanEditor.Instance.DeleteMove(Move);
                    _tab.Move = null;
                }
            }
            GUILayout.EndVertical();

            // Get the scroll biew
            _lastDragPos = GUILayout.BeginScrollView(_lastDragPos, GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true));
            {
                DrawPipBar();

                GUILayout.BeginHorizontal(GUILayout.ExpandHeight(true));
                {
                    if (Bars.Count == 0)
                    {
                        GUILayout.Box("Drag here to create a keyframe", GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true));
                    }
                    else
                    {
                        for (int x = 0; x < Bars.Count; x++)
                        {
                            // get the bar
                            var bar = Bars[x];

                            if (dm.IsDragging && x == _selectPosition && bar.IsDraggingBefore)
                                DrawLine();

                            bar.Draw();

                            if (dm.IsDragging && bar.IsDraggingOver)
                                _selectPosition = x;

                            if (dm.IsDragging && x == _selectPosition && !bar.IsDraggingBefore)
                                DrawLine();
                        }

                        if (dm.IsDragging && _selectPosition == -1)
                            DrawLine();

                        try
                        {
                            GUILayout.FlexibleSpace();
                        }
                        catch
                        {

                        }
                    }
                }
                GUILayout.EndHorizontal();
            }
            GUILayout.EndScrollView();
        }
        GUILayout.EndHorizontal();

        // Get the rect
        var rect = GUILayoutUtility.GetLastRect();
        if (dm.IsDragging && dm.IsDraggingOver(rect))
        {
            if (dm.Frame != null || dm.Bar != null)
                dm.Target = this;
        }
    }

    public static Texture2D timelineTexture = null;
    private static GUIStyle timeStyle;
    private void DrawPipBar()
    {
        var width = 0f;
        for (int x = 0; x < Bars.Count; x++)
            width += Bars[x].Width;


        if (timelineTexture == null)
        {
            timelineTexture = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/Fighter/Artwork/Editor/timeline.png");
            timelineTexture.wrapMode = TextureWrapMode.Repeat;

            timeStyle = new GUIStyle(GUI.skin.label);
            timeStyle.padding = new RectOffset();
            timeStyle.margin = new RectOffset();
        }
        GUILayout.Box("", timeStyle, GUILayout.Width(width), GUILayout.Height(16));
        var dim = GUILayoutUtility.GetLastRect();
        GUI.DrawTextureWithTexCoords(dim, timelineTexture, new Rect(0, 0, dim.width / timelineTexture.width, dim.height / timelineTexture.height));

        // Get the mark position
        if (_tab.Playing)
        {
            var percent = MarkTime * Bar.PIXELS_PER_SECOND;
            var pos = new Rect(dim.x + percent, dim.y, 1, 16);
            GUI.color = Color.red;
            GUI.DrawTexture(pos, Texture2D.whiteTexture);
            GUI.color = Color.white;
        }
    }

    /// <summary>
    /// On before drag complete~
    /// </summary>
    public void OnBeforeDragComplete()
    {
        var c = CombomanEditor.Instance.Character;
        var dm = DragManager.Instance;

        if (dm.Frame != null)
        {
            var frame = dm.Frame;

            // Create a new move frame
            var moveFrame = new MoveFrame(frame.Name, 0.1f);

            // Add a new move frame 
            Move.AddFrame(moveFrame);
        }
        else if (dm.Bar != null)
        {
            var moveFrame = dm.Bar.MoveFrame;
            Move.MoveFrames.Remove(moveFrame);

            // Insert it
            Move.MoveFrames.Insert(_selectPosition, moveFrame);

            dm.Bar = null;
        }

        c.Dirty = true;
        ReloadBars();
    }

    /// <summary>
    /// Reload the bars
    /// </summary>
    public void ReloadBars()
    {
        _selectPosition = -1;
        Bars.Clear();

        foreach (var frame in Move.MoveFrames)
        {
            var bar = new Bar(frame, CombomanEditor.Instance.Character);

            // Add a bar to this list
            Bars.Add(bar);
        }
    }

    public float MarkTime;
}
