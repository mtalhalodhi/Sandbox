using Godot;
using System.Collections.Generic;

public partial class World : Node2D
{
    struct PixelMove { public int X1, Y1, X2, Y2; }

    Dictionary<Vector2, PixelMove> moves = new Dictionary<Vector2, PixelMove>();

    void processSandSim()
    {
        moves.Clear();

        foreach (var chunk in Chunks)
        {
            if (chunk.Dirty) processChunk(chunk);
        }

        foreach (var pair in moves)
        {
            var move = pair.Value;
            SwapPixels(move.X1, move.Y1, move.X2, move.Y2);
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
        var left = this[x - 1, y];
        var right = this[x + 1, y];
        var downLeft = this[x - 1, y + 1];
        var downRight = this[x + 1, y + 1];

        if (pixel.Material == PixelMaterial.Powder)
        {
            if (down.Material == PixelMaterial.None || down.Material == PixelMaterial.Liquid)
            {
                addMove(x, y, x, y + 1);
                processed = true;
            }
            else if (downLeft.Material == PixelMaterial.None || down.Material == PixelMaterial.Liquid)
            {
                addMove(x, y, x - 1, y + 1);
                processed = true;
            }
            else if (downRight.Material == PixelMaterial.None || down.Material == PixelMaterial.Liquid)
            {
                addMove(x, y, x + 1, y + 1);
                processed = true;
            }
        }

        if (pixel.Material == PixelMaterial.Liquid)
        {
            if (down.Material == PixelMaterial.None)
            {
                addMove(x, y, x, y + 1);
                processed = true;
            }
            else if (downLeft.Material == PixelMaterial.None)
            {
                addMove(x, y, x - 1, y + 1);
                processed = true;
            }
            else if (downRight.Material == PixelMaterial.None)
            {
                addMove(x, y, x + 1, y + 1);
                processed = true;
            }
            else if (left.Material == PixelMaterial.None)
            {
                addMove(x, y, x - 1, y);
                processed = true;
            }
            else if (right.Material == PixelMaterial.None)
            {
                addMove(x, y, x + 1, y);
                processed = true;
            }
        }

        return processed;
    }

    void addMove(int X1, int Y1, int X2, int Y2)
    {
        moves.TryAdd(new Vector2(X2, Y2), new PixelMove()
        {
            X1 = X1,
            Y1 = Y1,
            X2 = X2,
            Y2 = Y2
        });
    }
}
