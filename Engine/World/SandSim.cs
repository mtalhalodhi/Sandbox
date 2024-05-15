using Godot;

public partial class World : Node2D
{
    void processSandSim()
    {
        foreach (var chunk in Chunks)
        {
            // processChunk(chunk);
        }
    }

    void processChunk(Chunk chunk)
    {
        var xEnd = chunk.X + ChunkSize;
        var yEnd = chunk.Y + ChunkSize;

        for (int x = chunk.X; x < xEnd; x++)
        {
            for (int y = chunk.Y; y < yEnd; y++)
            {
                processPixelAt(x, y);
            }
        }
    }

    void processPixelAt(int x, int y)
    {
        var pixel = this[x, y];
        var down = this[x, y + 1];

        if (pixel.Material == PixelMaterial.Powder)
        {
            if (down.Material == PixelMaterial.None || down.Material == PixelMaterial.Gas) {
                Swap(x, y, x, y + 1);
            }
        }
    }
}
