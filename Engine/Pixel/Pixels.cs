using Godot;

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
        Behavior = PixelBehavior.Powder,
        Velocity = new Vector2(2, 2)
    };

    public static Pixel Water = new Pixel()
    {
        Color = new Color(Colors.LightSkyBlue, 0.8f),
        Type = PixelType.Water,
        Behavior = PixelBehavior.Liquid,
        Velocity = new Vector2(6, 2)
    };
}
