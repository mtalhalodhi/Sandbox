using Godot;
using System;
using System.Drawing;
using System.Linq;

public partial class WorldEditor : Node2D
{
    [Export] World world;

    public override void _Ready()
    {
    }

    public override void _Process(double delta)
    {
        var mouse = GetGlobalMousePosition();
        var mx = (int)mouse.X;
        var my = (int)mouse.Y;

        int halfSize = 4;

        if (Input.IsActionPressed("left_click"))
        {
            for (int x = mx - halfSize; x < mx + halfSize; x++)
            {
                for (int y = my - halfSize; y < my + halfSize; y++)
                {
                    world.SetPixel(x, y, Pixels.Sand);
                }
            }
        }

        if (Input.IsActionPressed("right_click"))
        {
            for (int x = mx - halfSize; x < mx + halfSize; x++)
            {
                for (int y = my - halfSize; y < my + halfSize; y++)
                {
                    world.SetPixel(x, y, Pixels.Stone);
                }
            }
        }
    }
}
