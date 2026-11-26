using UnityEngine;

public class Cell
{
    public Vector2Int Position { get; private set; }

    public GridChunk Chunk { get; private set; }

    public Cell(Vector2Int position, GridChunk chunk)
    {
        Position = position;
        Chunk = chunk;
    }
}