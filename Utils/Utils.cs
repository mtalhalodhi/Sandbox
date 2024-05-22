using Godot;

public enum Direction { Up, Down, Left, Right, UpLeft, UpRight, DownLeft, DownRight };

public static class Utils
{
    public static bool FlipCoin()
    {
        return GD.Randi() % 2 == 0;
    }

    public static Vector2 VectorForDirection(Direction direction) {

        switch (direction)
        {
            case Direction.Up: return Vector2.Up;
            case Direction.Down: return Vector2.Down;
            case Direction.Left: return Vector2.Left;
            case Direction.Right: return Vector2.Right;
            case Direction.UpLeft: return new Vector2(-1, -1);
            case Direction.UpRight: return new Vector2(1, -1);
            case Direction.DownLeft: return new Vector2(-1, 1);
            case Direction.DownRight: return new Vector2(1, 1);
        }

        return Vector2.Zero;
    }
}
