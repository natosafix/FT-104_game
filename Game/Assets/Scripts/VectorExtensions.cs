using System;
using UnityEngine;

public static class Vector2DExtension
{
    public static float GetAngle(this Vector2 vector)
    {
        return Mathf.Atan2(vector.y, vector.x) * Mathf.Rad2Deg;
    }

    public static float AngleBetween(this Vector2 vector1, Vector2 vector2)
    {
        var sin = vector1.x * vector2.y - vector2.x * vector1.y;  
        var cos = vector1.x * vector2.x + vector1.y * vector2.y;

        return Mathf.Atan2(sin, cos) * Mathf.Rad2Deg;
    }

    public static Vector2 Rotate(this Vector2 vector, float angle)
    {
        angle *= Mathf.Deg2Rad;
        return new Vector2(
            vector.x * Mathf.Cos(angle) - vector.y * Mathf.Sin(angle), 
            vector.x * Mathf.Sin(angle) + vector.y * Mathf.Cos(angle));
    }
}