using UnityEngine;

public abstract class DrawableArea : IDrawablePanel
{
    private Rect _size;

    public float Height { get; set; }
    public float Width { get; set; }
    public Vector2 Position { get; set; }

    public Rect Rect
    {
        get
        {
            return new Rect(Position.x, Position.y, Width, Height);
        }
    }


    public void Draw()
    {
        GUILayout.BeginArea(Rect);
        {
            //DrawContent();
            GUILayout.Button("Click me");
            Debug.Log("RECT: " + Rect);
        }
        GUILayout.EndArea();
    }

    public abstract void DrawContent();
}
