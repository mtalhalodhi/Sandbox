using Godot;

public partial class PhysicsSim
{
    public World World;

    const double SPF = 30.0 / 60.0;
    double timeSinceUpdate = 0;

    public void Process(double delta)
    {
        timeSinceUpdate += delta;
        if (timeSinceUpdate <= SPF) return;
        timeSinceUpdate = 0;

        var chunks = World.Chunks;

        foreach (var chunk in chunks)
        {
            if (chunk.CollidersNeedRefresh)
            {
                chunk.GenerateColliders();
            }
        }
    }
}
