using Godot;

[Tool]
public partial class World : Node2D
{
    public Pixel this[int x, int y]
    {
        get => ChunkAt(x, y)[x, y];
        set
        {
            var chunk = ChunkAt(x, y);
            chunk[x, y] = value;
            chunk.MarkForUpdate();
        }
    }

    public void Swap(int x1, int y1, int x2, int y2) {
        var swap = this[x1, y1];
        this[x1, y1] = this[x2, y2];
        this[x2, y2] = swap;
    }

    public override void _Process(double delta)
    {
        RefreshChunks();
        QueueRedraw();
    }
}
