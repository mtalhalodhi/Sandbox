using Godot;
using System;

public static class ExtensionMethods
{

    public static Vector2 SnappedAtCorner(this Vector2 vec, int step)
    {
        float halfStep = step / 2;

        return new Vector2(
            Mathf.Snapped(vec.X - halfStep, step),
            Mathf.Snapped(vec.Y - halfStep, step)
        );
    }

}
