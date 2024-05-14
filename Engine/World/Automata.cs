using Godot;
using System;
using System.Collections.Generic;

public partial class World : Node2D
{
    public struct AutomataMove
    {
        public Pixel Pixel;

        public Chunk ChunkA;
        public Chunk ChunkB;

        public Vector2 From;
        public Vector2 To;
    }

    [Export] double AutomataTicksPerSecond = 15;
    double secondsSinceLastAutomataUpdate = 0;

    private List<AutomataMove> automataMoves = new List<AutomataMove>();

    public void ProcessAutomata(double delta)
    {
        double secondsPerTick = 1 / AutomataTicksPerSecond;
        secondsSinceLastAutomataUpdate += delta;
        if (secondsSinceLastAutomataUpdate < secondsPerTick) return;
        secondsSinceLastAutomataUpdate = 0;

        for (int i = 0; i < Chunks.Count; i++)
        {
            ProcessAutomataForChunk(Chunks[i]);
            Chunks[i].MarkForUpdate();
        }
    }

    public void ProcessAutomataForChunk(Chunk chunk)
    {
        var chunkPos = chunk.GlobalPosition;
        bool changed = false;

        for (int x = chunk.X; x < chunk.X + ChunkSize; x++)
        {
            for (int y = chunk.Y; y < chunk.Y + ChunkSize; y++)
            {
                // var pixel = chunk[x, y];
            }
        }

        if (changed) chunk.MarkForUpdate();
    }
}
