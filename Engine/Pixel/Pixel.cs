using Godot;

public struct Pixel
{
    public Color Color;
    public PixelType Type;
    public PixelBehavior Behavior;

    public Vector2 Velocity = new Vector2(1, 1);

    public Pixel() { }
}

public enum PixelBehavior
{
    None, Solid, Powder, Liquid, Gas
}

public enum PixelType
{
    None,
    Stone,
    Sand,
    Water
}
