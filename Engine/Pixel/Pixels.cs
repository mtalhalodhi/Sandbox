using Godot;
using System;

public static class Pixels
{
    public static Pixel Air = new Pixel() {
        Color = Colors.Transparent,
        Material = PixelMaterial.None
    };

    public static Pixel Stone = new Pixel() {
        Color = Colors.SlateGray,
        Material = PixelMaterial.Solid
    };

    public static Pixel Sand = new Pixel() {
        Color = Colors.PaleGoldenrod,
        Material = PixelMaterial.Powder
    };

    public static Pixel Water = new Pixel() {
        Color = Colors.LightSkyBlue,
        Material = PixelMaterial.Liquid
    };
}
