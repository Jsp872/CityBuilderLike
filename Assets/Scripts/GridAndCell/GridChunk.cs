using UnityEngine;

public class GridChunk
{
    public Vector2Int Coord { get; private set; }
    public RectInt CellBounds { get; private set; }
    public ChunkState State { get; set; } = ChunkState.Locked;

    public GridChunk(Vector2Int coord, int chunkSize)
    {
        Coord = coord;
        CellBounds = new RectInt(coord.x * chunkSize, coord.y * chunkSize, chunkSize, chunkSize);
    }

    public bool ContainsCell(Vector2Int cell) => CellBounds.Contains(cell);
}