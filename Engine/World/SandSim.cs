using Godot;

public partial class World : Node2D
{
    void processSandSim()
    {
        foreach (var chunk in Chunks)
        {
            if (chunk.Dirty) processChunk(chunk);
        }
    }

    void processChunk(Chunk chunk)
    {
        var xEnd = chunk.X + ChunkSize;
        var yEnd = chunk.Y + ChunkSize;

        bool dirty = false;

        for (int x = chunk.X; x < xEnd; x++)
        {
            for (int y = chunk.Y; y < yEnd; y++)
            {
                if (!processPixelAt(x, y)) {
                    dirty = true;
                }
            }
        }

        chunk.Dirty = dirty;
    }

    bool processPixelAt(int x, int y)
    {
        var updated = false;

        var pixel = this[x, y];
        var down = this[x, y + 1];

        if (pixel.Material == PixelMaterial.Powder)
        {
            if (down.Material == PixelMaterial.None || down.Material == PixelMaterial.Gas) {
                SwapPixels(x, y, x, y + 1);
                updated = true;
            }
        }

        return updated;
    }
}
