using Godot;
using System.Collections.Generic;

public partial class World : Node2D
{
    #region Properties

    [Export] private Camera2D cam;
    [Export] public int ChunkSize = 64;
    [Export] public Vector2 ChunkAreaSize = new Vector2(256, 160);

    public List<Chunk> Chunks = new List<Chunk>();
    public Dictionary<Vector2, Chunk> ChunkLookup = new Dictionary<Vector2, Chunk>();

    SandSim sandSim = new SandSim();
    PhysicsSim physicsSim = new PhysicsSim();

    #endregion

    #region Node Overrides

    public override void _Ready()
    {
        RefreshChunks();
        sandSim.World = this;
        physicsSim.World = this;
    }

    public override void _Process(double delta)
    {
        sandSim.Process(delta);
        physicsSim.Process(delta);

        QueueRedraw();
    }

    public override void _Draw()
    {
        if (!Settings.ShowDebugData) return;

        var areaRect = GetChunkAreaRect();
        DrawRect(areaRect, Colors.WhiteSmoke, false);

        var mouse = GetGlobalMousePosition();
        var chunk = ChunkAt((int)mouse.X, (int)mouse.Y);
        if (chunk != null) DrawRect(new Rect2(chunk.X, chunk.Y, ChunkSize, ChunkSize), new Color(1, 1, 1, 0.1f));
    }

    #endregion

    #region Pixel Methods

    public Pixel this[int x, int y]
    {
        get
        {
            var chunk = ChunkAt(x, y);
            return chunk != null ? chunk[x, y] : Pixels.Stone;
        }
        set
        {
            var chunk = ChunkAt(x, y);
            chunk[x, y] = value;
            chunk.Dirty = true;
            chunk.CollidersNeedRefresh = true;
        }
    }

    public Pixel this[Vector2 pos]
    {
        get => this[(int)pos.X, (int)pos.Y];
        set => this[(int)pos.X, (int)pos.Y] = value;
    }

    public bool IsEmpty(int x, int y)
    {
        return this[x, y].Type == PixelType.None;
    }

    public bool IsEmpty(Vector2 pos) => IsEmpty((int)pos.X, (int)pos.Y);

    public void SwapPixels(int x1, int y1, int x2, int y2)
    {
        var a = this[x1, y1];
        var b = this[x2, y2];

        this[x1, y1] = b;
        this[x2, y2] = a;

        WakeChunksOnEdge(x1, y1);
        WakeChunksOnEdge(x2, y2);
    }

    public void MovePixelTo(int x1, int y1, int x2, int y2)
    {
        this[x2, y2] = this[x1, y1];
        this[x1, y1] = Pixels.Air;
        WakeChunksOnEdge(x1, y1);
        WakeChunksOnEdge(x2, y2);
    }

    #endregion

    #region Chunk Logic

    void RefreshChunks()
    {
        var areaRect = GetChunkAreaRect();

        for (int i = Chunks.Count - 1; i >= 0; i--)
        {
            if (!areaRect.HasPoint(Chunks[i].GlobalPosition))
            {
                RemoveChunk(Chunks[i]);
            }
        }

        for (int x = (int)areaRect.Position.X; x < (int)(areaRect.Position.X + areaRect.Size.X); x += ChunkSize)
        {
            for (int y = (int)areaRect.Position.Y; y < (int)(areaRect.Position.Y + areaRect.Size.Y); y += ChunkSize)
            {
                InstantiateChunk(x, y);
            }
        }
    }

    public void InstantiateChunk(int x, int y)
    {
        var index = ChunkIndex(x, y);
        if (ChunkLookup.ContainsKey(index)) return;

        var chunk = new Chunk() { Size = ChunkSize };

        Chunks.Add(chunk);
        ChunkLookup.Add(index, chunk);

        chunk.GlobalPosition = new Vector2(x, y);
        AddChild(chunk);
    }

    public void RemoveChunk(Chunk chunk)
    {
        Chunks.Remove(chunk);
        ChunkLookup.Remove(new Vector2(chunk.X, chunk.Y));
        chunk.QueueFree();
    }

    public Rect2 GetChunkAreaRect()
    {
        var position = (cam.GlobalPosition - ChunkAreaSize / 2).Snapped(new Vector2(ChunkSize, ChunkSize));
        var size = ChunkAreaSize.Snapped(new Vector2(ChunkSize, ChunkSize));

        return new Rect2(position, size);
    }

    public Vector2 ChunkIndex(int x, int y)
    {
        int cx = x >= 0 ? x / ChunkSize : (x - ChunkSize + 1) / ChunkSize;
        int cy = y >= 0 ? y / ChunkSize : (y - ChunkSize + 1) / ChunkSize;
        return new Vector2(cx * ChunkSize, cy * ChunkSize);
    }

    public Chunk ChunkAt(int x, int y)
    {
        ChunkLookup.TryGetValue(ChunkIndex(x, y), out var chunk);
        return chunk;
    }

    public void KeepChunkAlive(int x, int y)
    {
        var chunk = ChunkAt(x, y);
        if (chunk != null) chunk.KeepPixelAlive(x, y);
    }

    public void WakeChunksOnEdge(int x, int y)
    {
        var chunk = ChunkAt(x, y);

        int cx = 0;
        int cy = 0;

        if (x == chunk.X) cx = -1;
        if (x == chunk.X + ChunkSize - 1) cx = 1;

        if (y == chunk.Y) cy = -1;
        if (y == chunk.Y + ChunkSize - 1) cy = 1;

        if (cx != 0) KeepChunkAlive(x + cx, y);
        if (cy != 0) KeepChunkAlive(x, y + cy);
        if (cx != 0 && cy != 0) KeepChunkAlive(x + cx, y + cy);
    }

    #endregion
}
