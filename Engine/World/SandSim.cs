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
            if (chunk.Dirty) ProcessChunk(chunk);
        }

        foreach (var chunk in World.Chunks)
        {
            chunk.UpdateDirtyRect();
        }

        foreach (var pair in moves)
        {
            var move = pair.Value;
            World.MovePixelTo(move.X1, move.Y1, move.X2, move.Y2);
        }
    }

    void ProcessChunk(Chunk chunk)
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
                if (ProcessPixelAt(x, y)) processed = true;
            }
        }
        chunk.Dirty = processed;
    }

    bool ProcessPixelAt(int x, int y)
    {
        var pixel = World[x, y];

        if (pixel.Behavior == PixelBehavior.Powder)
        {
            return ProcessPowder(x, y);
        }

        if (pixel.Behavior == PixelBehavior.Liquid)
        {
            return ProcessLiquid(x, y);
        }

        return false;
    }

    bool ProcessPowder(int x, int y)
    {
        var pos = new Vector2(x, y);

        if (TryMovingInDirection(x, y, Direction.Down)) return true;

        var dr = World.IsEmpty(pos + Utils.VectorForDirection(Direction.DownRight));
        var dl = World.IsEmpty(pos + Utils.VectorForDirection(Direction.DownLeft));

        if (dr && dl)
        {
            if (Utils.FlipCoin())
            {
                return TryMovingInDirection(x, y, Direction.DownRight);
            }
            else {
                return TryMovingInDirection(x, y, Direction.DownLeft);
            }
        }

        if (TryMovingInDirection(x, y, Direction.DownRight)) return true;
        if (TryMovingInDirection(x, y, Direction.DownLeft)) return true;

        return false;
    }

    bool ProcessLiquid(int x, int y)
    {
        var pos = new Vector2(x, y);

        if (TryMovingInDirection(x, y, Direction.Down)) return true;

        var r = World.IsEmpty(pos + Utils.VectorForDirection(Direction.DownRight));
        var l = World.IsEmpty(pos + Utils.VectorForDirection(Direction.DownLeft));

        if (r && l)
        {
            if (Utils.FlipCoin())
            {
                return TryMovingInDirection(x, y, Direction.Right);
            }
            else {
                return TryMovingInDirection(x, y, Direction.Left);
            }
        }

        if (TryMovingInDirection(x, y, Direction.Left)) return true;
        if (TryMovingInDirection(x, y, Direction.Right)) return true;

        var dr = World.IsEmpty(pos + Utils.VectorForDirection(Direction.DownRight));
        var dl = World.IsEmpty(pos + Utils.VectorForDirection(Direction.DownLeft));

        if (dr && dl)
        {
            if (Utils.FlipCoin())
            {
                return TryMovingInDirection(x, y, Direction.DownRight);
            }
            else {
                return TryMovingInDirection(x, y, Direction.DownLeft);
            }
        }

        if (TryMovingInDirection(x, y, Direction.DownRight)) return true;
        if (TryMovingInDirection(x, y, Direction.DownLeft)) return true;

        return false;
    }

    bool TryMovingInDirection(int x, int y, Direction direction)
    {
        var p = World[x, y];

        var a = new Vector2(x, y);
        var b = Utils.VectorForDirection(direction);

        b = b * p.Velocity + a;

        var points = BresenhamLine.Compute(a, b);
        if (points.Count <= 1) return false;

        int index = 1;
        if (!World.IsEmpty(points[index])) return false;

        while (index < points.Count - 1 && World.IsEmpty(points[index + 1]))
        {
            index++;
        }

        AddMove(x, y, points[index]);
        return true;
    }

    void AddMove(int x1, int y1, int x2, int y2)
    {
        var key = new Vector2(x2, y2);

        if (!moves.ContainsKey(key))
        {
            moves.Add(key, new PixelMove() { X1 = x1, Y1 = y1, X2 = x2, Y2 = y2 });
        }
        else if (Utils.FlipCoin())
        {
            moves[key] = new PixelMove() { X1 = x1, Y1 = y1, X2 = x2, Y2 = y2 };
        }
    }

    void AddMove(int x1, int y1, Vector2 target) => AddMove(x1, y1, (int)target.X, (int)target.Y);
}
