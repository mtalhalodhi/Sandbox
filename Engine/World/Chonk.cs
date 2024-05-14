using Godot;
using System.Collections.Generic;

public partial class World : Node2D
{
    [Export] private Camera2D cam;

    [Export] public int ChunkSize = 64;
    [Export] public Vector2 ChunkAreaSize = new Vector2(256, 160);


    public List<Chunk> Chunks = new List<Chunk>();
    public Dictionary<(int x, int y), Chunk> ChunkLookup = new Dictionary<(int x, int y), Chunk>();


    public override void _Draw()
    {
        // if (!Engine.IsEditorHint()) return;

        var areaRect = GetChunkAreaRect();

        DrawRect(areaRect, Colors.WhiteSmoke, false);

        var mouse = GetGlobalMousePosition();

        var chunk = ChunkAt((int)mouse.X, (int)mouse.Y);

        if (chunk != null) DrawRect(new Rect2(chunk.X, chunk.Y, ChunkSize, ChunkSize), new Color(1, 1, 1, 0.04f));
    }

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

    void InstantiateChunk(int x, int y)
    {
        var index = ChunkIndex(x, y);
        if (ChunkLookup.ContainsKey(index)) return;

        var chunk = new Chunk() { Size = ChunkSize };

        Chunks.Add(chunk);
        ChunkLookup.Add(index, chunk);

        chunk.GlobalPosition = new Vector2(x, y);
        AddChild(chunk);
    }

    void RemoveChunk(Chunk chunk)
    {
        Chunks.Remove(chunk);
        ChunkLookup.Remove((chunk.X, chunk.Y));
        chunk.QueueFree();
    }

    public Rect2 GetChunkAreaRect()
    {
        var position = (cam.GlobalPosition - ChunkAreaSize / 2).Snapped(new Vector2(ChunkSize, ChunkSize));
        var size = ChunkAreaSize.Snapped(new Vector2(ChunkSize, ChunkSize));

        return new Rect2(position, size);
    }

    public (int x, int y) ChunkIndex(int x, int y)
    {
        int cx = x >= 0 ? x / ChunkSize : (x - ChunkSize + 1) / ChunkSize;
        int cy = y >= 0 ? y / ChunkSize : (y - ChunkSize + 1) / ChunkSize;
        return (cx, cy);
    }

    public Chunk ChunkAt(int x, int y)
    {
        ChunkLookup.TryGetValue(ChunkIndex(x, y), out var chunk);
        return chunk;
    }


}