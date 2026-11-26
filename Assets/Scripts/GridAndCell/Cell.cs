using UnityEngine;

public class Cell
{
    public Vector2Int Position { get; private set; }

    public Cell(Vector2Int position)
    {
        Position = position;
    }
}