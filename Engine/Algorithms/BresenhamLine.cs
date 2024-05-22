using Godot;
using System.Collections.Generic;

//https://en.wikipedia.org/wiki/Bresenham%27s_line_algorithm
public static class BresenhamLine
{
    public static List<Vector2> Compute(Vector2 a, Vector2 b)
    {
        return Compute((int)a.X, (int)a.Y, (int)b.X, (int)b.Y);
    }

    public static List<Vector2> Compute(int x1, int y1, int x2, int y2)
    {
        var points = new List<Vector2>();

        var dx = Mathf.Abs(x2 - x1);
        var sx = x1 < x2 ? 1 : -1;
        var dy = -Mathf.Abs(y2 - y1);
        var sy = y1 < y2 ? 1 : -1;

        var error = dx + dy;

        while (true)
        {
            points.Add(new Vector2(x1, y1));

            if (x1 == x2 && y1 == y2) break;

            var e2 = 2 * error;
            if (e2 >= dy)
            {
                if (x1 == x2) break;
                error += dy;
                x1 = x1 + sx;
            }
            if (e2 <= dx)
            {
                if (y1 == y2) break;
                error += dx;
                y1 = y1 + sy;
            }
        }

        return points;
    }
}
