using UnityEngine;

public static class GUIUtil
{
    public static void DrawFrame(Rect rect)
    {
        GUI.color = Color.black;
        var top = new Rect(rect.x, rect.y, rect.width, 1);
        GUI.DrawTexture(top, Texture2D.whiteTexture);

        var bottom = new Rect(rect.x + rect.height, rect.y + rect.height, rect.width, 1);
        GUI.DrawTexture(bottom, Texture2D.whiteTexture);

        var left = new Rect(rect.x, rect.y, 1, rect.height);
        GUI.DrawTexture(left, Texture2D.whiteTexture);

        var right = new Rect(rect.x + rect.width, rect.y, 1, rect.height);
        GUI.DrawTexture(right, Texture2D.whiteTexture);

        GUI.color = Color.white;
    }
}
