using Godot;

public partial class World : Node2D
{
    public Pixel this[int x, int y]
    {
        get
        {
            var chunk = ChunkAt(x, y);
            return chunk != null ? chunk[x, y] : Pixels.Air;
        }
        set
        {
            var chunk = ChunkAt(x, y);
            chunk[x, y] = value;
            chunk.Dirty = true;
        }
    }

    public void SwapPixels(int x1, int y1, int x2, int y2)
    {
        var swap = this[x1, y1];
        this[x1, y1] = this[x2, y2];
        this[x2, y2] = swap;

        WakeChunksOnEdge(x1, y1);
    }

    public void MovePixelTo(int x1, int y1, int x2, int y2)
    {
        this[x2, y2] = this[x1, y1];
        WakeChunksOnEdge(x1, y1);
    }

    public void WakeChunksOnEdge(int x, int y) {
        var chunk = ChunkAt(x, y);

        int cx = 0;
        int cy = 0;

        if (x == chunk.X) cx = -1;
        if (x == chunk.X + ChunkSize) cx = 1;

        if (y == chunk.Y) cy = -1;
        if (y == chunk.Y + ChunkSize) cy = 1;

        if (cx != 0) KeepChunkAlive(x + cx, y);
        if (cy != 0) KeepChunkAlive(x, y + cy);
        if (cx != 0 && cy != 0) KeepChunkAlive(x + cx, y + cy);
    }

    public override void _Ready()
    {
        RefreshChunks();
    }

    public override void _Process(double delta)
    {
        QueueRedraw();
    }

    public override void _PhysicsProcess(double delta)
    {
        ProcessSandSim();
    }

    public override void _Draw()
    {
        // if (!Engine.IsEditorHint()) return;

        var areaRect = GetChunkAreaRect();
        DrawRect(areaRect, Colors.WhiteSmoke, false);

        var mouse = GetGlobalMousePosition();
        var chunk = ChunkAt((int)mouse.X, (int)mouse.Y);
        if (chunk != null) DrawRect(new Rect2(chunk.X, chunk.Y, ChunkSize, ChunkSize), new Color(1, 1, 1, 0.1f));
    }
}
