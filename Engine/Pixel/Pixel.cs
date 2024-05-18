using Godot;
using System;

public struct Pixel
{
    public Color Color;
    public PixelType Type;
    public PixelBehavior Behavior;

    public int VelocityX;
    public int VelocityY;
}

public enum PixelBehavior
{
    None, Solid, Powder, Liquid, Gas
}

public enum PixelType
{
    None,
    Air,
    Stone,
    Sand,
    Water
}
