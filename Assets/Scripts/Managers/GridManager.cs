using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] private int chunksX = 3;
    [SerializeField] private int chunksY = 3;
    [SerializeField] private int chunkSize = 16;
    [SerializeField] private float cellSize = 1f;

    private Cell[,] cells;
    public int Width => chunksX * chunkSize;
    public int Height => chunksY * chunkSize;
    public float CellSize => cellSize;

    private void Awake()
    {
        cells = new Cell[Width, Height];

        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                cells[x, y] = new Cell(new Vector2Int(x, y));
            }
        }
    }

    public bool IsInBounds(Vector2Int pos)
    {
        return pos.x >= 0 && pos.y >= 0 && pos.x < Width && pos.y < Height;
    }

    public Cell GetCell(Vector2Int pos)
    {
        return IsInBounds(pos) ? cells[pos.x, pos.y] : null;
    }

    public Vector3 CellToWorld(Vector2Int pos)
    {
        return new Vector3((pos.x + 0.5f) * cellSize, (pos.y + 0.5f) * cellSize, 0f);
    }

    public Vector2Int WorldToCell(Vector3 worldPos)
    {
        return new Vector2Int(Mathf.FloorToInt(worldPos.x / cellSize),Mathf.FloorToInt(worldPos.y / cellSize));
    }

    private void OnDrawGizmos()
    {
        Vector3 size = new Vector3(cellSize, cellSize, 0f);

        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                Gizmos.color = new Color(1f, 1f, 1f, 0.15f);
                Gizmos.DrawWireCube(CellToWorld(new Vector2Int(x, y)), size);
            }
        }

        Gizmos.color = Color.yellow;
        Vector3 chunkWorldSize = new Vector3(chunkSize * cellSize, chunkSize * cellSize, 0f);
        for (int cx = 0; cx < chunksX; cx++)
        {
            for (int cy = 0; cy < chunksY; cy++)
            {
                Vector3 center = new Vector3((cx + 0.5f) * chunkWorldSize.x, (cy + 0.5f) * chunkWorldSize.y, 0f);
                Gizmos.DrawWireCube(center, chunkWorldSize);
            }
        }
    }
}