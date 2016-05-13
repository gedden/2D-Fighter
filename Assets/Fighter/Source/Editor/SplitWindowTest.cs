using UnityEngine;
using UnityEditor;

public class GUISplitter : EditorWindow
{
    Vector2 posLeft;
    Vector2 posRight;
    GUIStyle styleLeftView;
    GUIStyle styleRightView;
    float splitterPos;
    Rect splitterRect;
    Vector2 dragStartPos;
    bool dragging;
    float splitterWidth = 3;

    // Add menu named "My Window" to the Window menu
    [MenuItem("Window/GUISplitter")]
    static void Init()
    {
        GUISplitter window = (GUISplitter)EditorWindow.GetWindow(
            typeof(GUISplitter));
        window.position = new Rect(200, 200, 1000, 600);
        window.splitterPos = 300;
    }
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(GUISplitter));
    }

    void OnGUI()
    {
        if (styleLeftView == null)
            styleLeftView = new GUIStyle(GUI.skin.box);
        if (styleRightView == null)
            styleRightView = new GUIStyle(GUI.skin.box);

        GUILayout.BeginHorizontal();

        // Left view
        posLeft = GUILayout.BeginScrollView(posLeft,
            GUILayout.Width(splitterPos),
            GUILayout.MaxWidth(splitterPos),
            GUILayout.MinWidth(splitterPos));
        GUILayout.Box("Left View",
                styleLeftView,
                GUILayout.ExpandWidth(true),
                GUILayout.ExpandHeight(true));
        GUILayout.EndScrollView();


        GUIStyle myStyle = new GUIStyle(GUI.skin.scrollView);
        // do whatever you want with this style, e.g.:
        myStyle.margin = new RectOffset(0, 0, 0, 0);
        // Splitter
        GUILayout.Box("", myStyle,
            GUILayout.Width(splitterWidth),
            GUILayout.MaxWidth(splitterWidth),
            GUILayout.MinWidth(splitterWidth),
            GUILayout.ExpandHeight(true));
        splitterRect = GUILayoutUtility.GetLastRect();
        EditorGUIUtility.AddCursorRect(splitterRect, MouseCursor.ResizeHorizontal);

        // Right view
        posRight = GUILayout.BeginScrollView(posRight,
            GUILayout.ExpandWidth(true));
        GUILayout.Box("Right View",
        styleRightView,
        GUILayout.ExpandWidth(true),
        GUILayout.ExpandHeight(true));
        GUILayout.EndScrollView();

        GUILayout.EndHorizontal();

        // Splitter events
        if (Event.current != null)
        {
            switch (Event.current.type)
            {
                case EventType.MouseDown:
                    if (splitterRect.Contains(Event.current.mousePosition))
                    {
                        Debug.Log("Start dragging");
                        dragging = true;
                    }
                    break;
                case EventType.MouseDrag:
                    if (dragging)
                    {
                        Debug.Log("moving splitter");
                        splitterPos += Event.current.delta.x;
                        Repaint();
                    }
                    break;
                case EventType.MouseUp:
                    if (dragging)
                    {
                        Debug.Log("Done dragging");
                        dragging = false;
                    }
                    break;
            }
        }
    }
}