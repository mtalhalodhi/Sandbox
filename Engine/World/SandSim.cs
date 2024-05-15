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

        bool processed = false;

        for (int x = chunk.X; x < xEnd; x++)
        {
            for (int y = chunk.Y; y < yEnd; y++)
            {
                if (processPixelAt(x, y)) processed = true;
            }
        }

        chunk.Dirty = processed;
    }

    bool processPixelAt(int x, int y)
    {
        var processed = false;

        var pixel = this[x, y];
        var down = this[x, y + 1];
        var downLeft = this[x - 1, y + 1];
        var downRight = this[x + 1, y + 1];

        if (pixel.Material == PixelMaterial.Powder)
        {
            if (down.Material == PixelMaterial.None) {
                SwapPixels(x, y, x, y + 1);
                processed = true;
            }
            else if (downLeft.Material == PixelMaterial.None) {
                SwapPixels(x, y, x - 1, y + 1);
                processed = true;
            }
            else if (downRight.Material == PixelMaterial.None) {
                SwapPixels(x, y, x + 1, y + 1);
                processed = true;
            }
        }

        return processed;
    }
}
