using Godot;

public partial class Chunk : StaticBody2D
{
    private Pixel[] pixels;

    Sprite2D sprite;
    Image image;

    public int Size;
    public int X, Y;

    private bool _dirty = false;

    public bool Dirty
    {
        get => _dirty;
        set
        {
            QueueRedraw();
            _dirty = value;
        }
    }

    public bool CollidersNeedRefresh { get; set; }

    public Vector2 DirtyRectMin;
    public Vector2 DirtyRectMax;

    private Vector2 dirtyRectBufferMin;
    private Vector2 dirtyRectBufferMax;

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
            KeepPixelAlive(x, y);
        }
    }

    public Pixel this[Vector2 pos]
    {
        get => this[(int)pos.X, (int)pos.Y];
        set => this[(int)pos.X, (int)pos.Y] = value;
    }

    public void KeepPixelAlive(int x, int y)
    {
        dirtyRectBufferMin.X = Mathf.Clamp(Mathf.Min(x - 2, dirtyRectBufferMin.X), X, X + Size);
        dirtyRectBufferMin.Y = Mathf.Clamp(Mathf.Min(y - 2, dirtyRectBufferMin.Y), Y, Y + Size);

        dirtyRectBufferMax.X = Mathf.Clamp(Mathf.Max(x + 2, dirtyRectBufferMax.X), X, X + Size);
        dirtyRectBufferMax.Y = Mathf.Clamp(Mathf.Max(y + 2, dirtyRectBufferMax.Y), Y, Y + Size);

        _dirty = true;
    }

    public void UpdateDirtyRect()
    {
        DirtyRectMin = dirtyRectBufferMin;
        DirtyRectMax = dirtyRectBufferMax;

        dirtyRectBufferMin = new Vector2(X + Size, Y + Size);
        dirtyRectBufferMax = new Vector2(X - 1, Y - 1);
    }

    //https://forum.godotengine.org/t/how-can-i-automatically-create-a-collisionpolygon2d-from-an-image-using-gdnative-or-gdscript/22437/3
    public void GenerateColliders()
    {
        CollidersNeedRefresh = false;

        var children = GetChildren();
        foreach (var child in children)
        {
            if (child is CollisionPolygon2D) child.QueueFree();
        }

        var bitmap = new Bitmap();
        bitmap.CreateFromImageAlpha(image, 0.99f);
        var rect = new Rect2I(0, 0, Size, Size);
        var polygons = bitmap.OpaqueToPolygons(rect, 0.5f);
        for (int i = 0; i < polygons.Count; i++)
        {
            var collider = new CollisionPolygon2D
            {
                Polygon = polygons[i],
                BuildMode = CollisionPolygon2D.BuildModeEnum.Solids
            };
            AddChild(collider);
        }
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

        sprite = new Sprite2D
        {
            Texture = ImageTexture.CreateFromImage(image),
            Offset = new Vector2(Size / 2, Size / 2),
            ShowBehindParent = true
        };

        AddChild(sprite);
    }

    public override void _Draw()
    {
        ((ImageTexture)sprite.Texture).Update(image);

        if (!Settings.ShowDebugData) return;

        DrawRect(new Rect2(0, 0, Size, Size), new Color(1, 1, 1, 0.25f), false);

        if (!_dirty) return;

        var pos = new Vector2(X, Y);
        DrawRect(new Rect2(0, 0, Size, Size), new Color(1, .2f, .2f, 0.7f), false, .5f);

        var dsStart = DirtyRectMin - pos;
        var dsEnd = DirtyRectMax - pos;

        DrawRect(new Rect2(dsStart, dsEnd - dsStart), new Color(.8f, .6f, .9f, 1f), false, .5f);
    }
}
