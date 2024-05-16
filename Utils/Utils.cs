using Godot;

public static class Utils
{
    public static bool FlipCoin()
    {
        return GD.Randi() % 2 == 0;
    }
}
