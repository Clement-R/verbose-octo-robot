using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class VisualDebug
{
    public static void DrawBox(Vector2 p_origin, Vector2 p_size, Color p_color, float p_duration=0f)
    {
        // Top
        Debug.DrawLine(new Vector3(p_origin.x - (p_size.x / 2f), p_origin.y + (p_size.y / 2f)),
                       new Vector3(p_origin.x + (p_size.x / 2f), p_origin.y + (p_size.y / 2f)),
                       p_color,
                       p_duration);
        // Bot
        Debug.DrawLine(new Vector3(p_origin.x - (p_size.x / 2f), p_origin.y - (p_size.y / 2f)),
                       new Vector3(p_origin.x + (p_size.x / 2f), p_origin.y - (p_size.y / 2f)),
                       p_color,
                       p_duration);
        // Left
        Debug.DrawLine(new Vector3(p_origin.x - (p_size.x / 2f), p_origin.y + (p_size.y / 2f)),
                       new Vector3(p_origin.x - (p_size.x / 2f), p_origin.y - (p_size.y / 2f)),
                       p_color,
                       p_duration);
        // Right
        Debug.DrawLine(new Vector3(p_origin.x + (p_size.x / 2f), p_origin.y + (p_size.y / 2f)),
                       new Vector3(p_origin.x + (p_size.x / 2f), p_origin.y - (p_size.y / 2f)),
                       p_color,
                       p_duration);
    }

    public static void DrawCross(Vector2 p_origin, float p_size, Color p_color, float p_duration = 0f)
    {
        Debug.DrawLine(new Vector3(p_origin.x - p_size, p_origin.y + p_size, 0f),
                       new Vector3(p_origin.x + p_size, p_origin.y - p_size, 0f),
                       p_color,
                       p_duration);

        Debug.DrawLine(new Vector3(p_origin.x + p_size, p_origin.y + p_size, 0f),
                       new Vector3(p_origin.x - p_size, p_origin.y - p_size, 0f),
                       p_color,
                       p_duration);
    }
}
