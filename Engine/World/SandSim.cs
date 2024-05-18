using Godot;
using System.Collections.Generic;

public partial class SandSim
{
    struct PixelMove { public int X1, Y1, X2, Y2; }

    Dictionary<Vector2, PixelMove> moves = new Dictionary<Vector2, PixelMove>();

    public World World;

    const double SPF = 1.0 / 60.0;
    double timeSinceUpdate = 0;

    public void ProcessSandSim(double delta)
    {
        timeSinceUpdate += delta;
        if (timeSinceUpdate <= SPF) return;
        timeSinceUpdate = 0;

        moves.Clear();

        foreach (var chunk in World.Chunks)
        {
            if (chunk.Dirty) processChunk(chunk);
        }

        foreach (var chunk in World.Chunks)
        {
            chunk.UpdateDirtyRect();
        }

        foreach (var pair in moves)
        {
            var move = pair.Value;
            World.SwapPixels(move.X1, move.Y1, move.X2, move.Y2);
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
        var down = World[x, y + 1];
        var downLeft = World[x - 1, y + 1];
        var downRight = World[x + 1, y + 1];

        if (pixel.Behavior == PixelBehavior.Powder)
        {
            if (down.Behavior == PixelBehavior.None)
            {
                addMove(x, y, x, y + 1);
                processed = true;
            }
            else if (downLeft.Behavior == PixelBehavior.None && downRight.Behavior == PixelBehavior.None)
            {
                if (Utils.FlipCoin())
                {
                    addMove(x, y, x - 1, y + 1);
                }
                else
                {
                    addMove(x, y, x + 1, y + 1);
                }
                processed = true;
            }
            else if (downLeft.Behavior == PixelBehavior.None)
            {
                addMove(x, y, x - 1, y + 1);
                processed = true;
            }
            else if (downRight.Behavior == PixelBehavior.None)
            {
                addMove(x, y, x + 1, y + 1);
                processed = true;
            }
        }

        return processed;
    }

    void addMove(int X1, int Y1, int X2, int Y2)
    {
        var key = new Vector2(X2, Y2);

        if (!moves.ContainsKey(key))
        {
            moves.Add(new Vector2(X2, Y2), new PixelMove() { X1 = X1, Y1 = Y1, X2 = X2, Y2 = Y2 });
        }
        else if (Utils.FlipCoin())
        {
            moves[key] = new PixelMove() { X1 = X1, Y1 = Y1, X2 = X2, Y2 = Y2 };
        }
    }
}
