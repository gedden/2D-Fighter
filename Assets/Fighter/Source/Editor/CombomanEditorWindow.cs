using UnityEngine;
using UnityEditor;
using Comboman;

public class CombomanEditorWindow : EditorWindow
{
    Vector2 posLeft;
    GUIStyle styleLeftView;
    float splitterPos;
    Rect splitterRect;
    bool dragging;
    float splitterWidth = 3;

    FrameSequenceEditor editor = null;
    CombomanControlPanel control = null ;
    CombomanContentWindow content = null;

    // Add menu named "My Window" to the Window menu
    [MenuItem("Window/Comboman Editor Window")]
    static void Init()
    {
        var window = GetWindow<CombomanEditorWindow>();
        window.position = new Rect(200, 200, 1000, 600);
        window.splitterPos = 300;
        window.control = new CombomanControlPanel();
    }

    public float ContentWidth
    {
        get
        {
            return position.width - splitterRect.x;
        }
    }


    public CombomanControlPanel Control
    {
        get
        {
            return control;
        }
    }

    public static void ShowWindow()
    {
        Init();
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
        //control.CharacterData = data;
    }

    /// <summary>
    /// GUI Info
    /// </summary>
    void OnGUI()
    {

        GUILayout.BeginHorizontal(EditorStyles.toolbar);
        someOption = GUILayout.Toggle(someOption, "Toggle Me", EditorStyles.toolbarButton);

        if (GUILayout.Button("Load", EditorStyles.toolbarButton))
            LoadCharacter();

        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();


        GUILayout.BeginHorizontal();



        DrawLeftControls();
        DrawSplitter();

        // Right view
        DrawRightControls();
        GUILayout.EndHorizontal();

        // Splitter events
        if (Event.current != null)
        {
            switch (Event.current.type)
            {
                case EventType.MouseDown:
                    if (splitterRect.Contains(Event.current.mousePosition))
                    {
                        dragging = true;
                    }
                    break;
                case EventType.MouseDrag:
                    if (dragging)
                    {
                        splitterPos += Event.current.delta.x;
                        Repaint();
                    }
                    break;
                case EventType.MouseUp:
                    if (dragging)
                    {
                        dragging = false;
                    }
                    break;
            }
        }
    }

    private void DrawLeftControls()
    {
        if (styleLeftView == null)
            styleLeftView = new GUIStyle(GUI.skin.box);

        // Left view
        posLeft = GUILayout.BeginScrollView(posLeft,
            GUILayout.Width(splitterPos),
            GUILayout.MaxWidth(splitterPos),
            GUILayout.MinWidth(splitterPos));

        if (control == null)
            control = new CombomanControlPanel();
        control.Draw();
        /*
        GUILayout.Box("Left View",
                styleLeftView,
                GUILayout.ExpandWidth(true),
                GUILayout.ExpandHeight(true));
                */

        GUILayout.EndScrollView();
    }

    /// <summary>
    /// 
    /// </summary>
    private void DrawSplitter()
    {
        GUIStyle splitterStyle = new GUIStyle(GUI.skin.scrollView);
        // do whatever you want with this style, e.g.:
        splitterStyle.margin = new RectOffset(0, 0, 0, 0);
        // Splitter
        GUILayout.Box("", splitterStyle,
            GUILayout.Width(splitterWidth),
            GUILayout.MaxWidth(splitterWidth),
            GUILayout.MinWidth(splitterWidth),
            GUILayout.ExpandHeight(true));
        splitterRect = GUILayoutUtility.GetLastRect();
        EditorGUIUtility.AddCursorRect(splitterRect, MouseCursor.ResizeHorizontal);
    }

    private bool someOption = false;
    private void DrawRightControls()
    {
        if (content == null)
            content = new CombomanContentWindow(this);
        content.OnGUI();
    }
}