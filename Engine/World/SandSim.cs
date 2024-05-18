using Godot;

public partial class SandSim
{
    public World World;

    const double SPF = 1.0 / 60.0;
    double timeSinceUpdate = 0;

    public void ProcessSandSim(double delta)
    {
        timeSinceUpdate += delta;
        if (timeSinceUpdate <= SPF) return;
        timeSinceUpdate = 0;

        foreach (var chunk in World.Chunks)
        {
            if (chunk.Dirty) processChunk(chunk);
        }

        foreach (var chunk in World.Chunks)
        {
            chunk.UpdateDirtyRect();
        }
    }

    void processChunk(Chunk chunk)
    {
        var xStart = (int)chunk.DirtyRectMin.X;
        var yStart = (int)chunk.DirtyRectMin.Y;

        var xEnd = (int)chunk.DirtyRectMax.X;
        var yEnd = (int)chunk.DirtyRectMax.Y;

        bool processed = false;
        for (int x = xStart; x < xEnd; x++)
        {
            for (int y = yStart; y < yEnd; y++)
            {
                if (processPixelAt(x, y)) processed = true;
            }
        }
        chunk.Dirty = processed;
    }

    bool processPixelAt(int x, int y)
    {
        var processed = false;

        var pixel = World[x, y];

        var dn = World[x, y + 1];
        var dl = World[x - 1, y + 1];
        var dr = World[x + 1, y + 1];

        if (pixel.Behavior == PixelBehavior.Powder)
        {
            if (dn.Behavior == PixelBehavior.None)
            {
                World.SwapPixels(x, y, x, y + 1);
                processed = true;
            }
            else if (dl.Type == PixelType.None)
            {
                World.SwapPixels(x, y, x - 1, y + 1);
                processed = true;
            }
            else if (dr.Type == PixelType.None)
            {
                World.SwapPixels(x, y, x + 1, y + 1);
                processed = true;
            }
        }

        return processed;
    }
}
