using Godot;
using System;

public static class Pixels
{
    public static Pixel Air = new Pixel()
    {
        Color = Colors.Transparent
    };

    public static Pixel Stone = new Pixel()
    {
        Color = Colors.SlateGray,
        Type = PixelType.Stone,
        Behavior = PixelBehavior.Solid
    };

    public static Pixel Sand = new Pixel()
    {
        Color = Colors.PaleGoldenrod,
        Type = PixelType.Sand,
        Behavior = PixelBehavior.Powder
    };

    public static Pixel Water = new Pixel()
    {
        Color = Colors.LightSkyBlue,
        Type = PixelType.Water,
        Behavior = PixelBehavior.Liquid
    };
}
