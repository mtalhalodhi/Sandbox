using Godot;
using System;

[Tool]
public partial class Chunk : Sprite2D
{
    private Pixel[] pixels;

    Image image;

    public int Size;
    public int X, Y;

    private int index(int x, int y)
    {
        return x - X + (y - Y) * Size;
    }

    private int localIndex(int x, int y)
    {
        return x + y * Size;
    }

    public Pixel this[int x, int y]
    {
        get => pixels[index(x, y)];
        set
        {
            pixels[index(x, y)] = value;
            image.SetPixel(x - X, y - Y, value.Color);
        }
    }

    public Pixel this[Vector2 pos]
    {
        get => this[(int)pos.X, (int)pos.Y];
        set => this[(int)pos.X, (int)pos.Y] = value;
    }

    public override void _Ready()
    {
        X = (int)GlobalPosition.X;
        Y = (int)GlobalPosition.Y;

        pixels = new Pixel[Size * Size];

        image = Image.Create(Size, Size, false, Image.Format.Rgba8);
        for (int x = 0; x < Size; x++)
        {
            for (int y = 0; y < Size; y++)
            {
                image.SetPixel(x, y, pixels[localIndex(x, y)].Color);
            }
        }

        Texture = ImageTexture.CreateFromImage(image);
        TextureFilter = TextureFilterEnum.Nearest;
        Offset = new Vector2(Size / 2, Size / 2);
    }

    public void MarkForUpdate()
    {
        QueueRedraw();
    }

    public override void _Draw()
    {
        if (Engine.IsEditorHint() || true)
        {
            DrawRect(new Rect2(0, 0, Size, Size), new Color(1, 1, 1, 0.05f), false);
        }

        if (!Engine.IsEditorHint())
        {
            ((ImageTexture)Texture).Update(image);
        }
    }
}
