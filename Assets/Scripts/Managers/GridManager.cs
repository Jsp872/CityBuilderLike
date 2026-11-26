using UnityEngine;
using System;

public class GridManager : MonoBehaviour
{
    [SerializeField] private int chunksX = 3;
    [SerializeField] private int chunksY = 3;
    [SerializeField] private int chunkSize = 16;
    [SerializeField] private float cellSize = 1f;

    private Cell[,] cells;

    private GridChunk[,] chunks;
    public int Width => chunksX * chunkSize;
    public int Height => chunksY * chunkSize;
    public int ChunksX => chunksX;
    public int ChunksY => chunksY;
    public int ChunkSize => chunkSize;
    public float CellSize => cellSize;

    public event Action<GridChunk> OnChunkStateChanged;
    private static readonly Vector2Int[] neighbourDirections =
    {
        Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right
    };

    private void Awake()
    {
        cells = new Cell[Width, Height];
        chunks = new GridChunk[chunksX, chunksY];

        for (int i = 0; i < chunksX; i++)
        {
            for (int j = 0; j < chunksY; j++)
            {
                chunks[i, j] = new GridChunk(new Vector2Int(i, j), chunkSize);
            }
        }

        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                cells[x, y] = new Cell(new Vector2Int(x, y), chunks[x / chunkSize, y / chunkSize]);
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

    public Vector3 ChunkToWorld(Vector2Int chunkCoord)
    {
        return new Vector3((chunkCoord.x + 0.5f) * chunkSize * cellSize, (chunkCoord.y + 0.5f) * chunkSize * cellSize, 0f);
    }

    public Vector2Int WorldToChunk(Vector3 worldPos)
    {
        return new Vector2Int(Mathf.FloorToInt(worldPos.x / (chunkSize * cellSize)), Mathf.FloorToInt(worldPos.y / (chunkSize * cellSize)));
    }

    public bool IsChunkInBounds(Vector2Int coord)
    {
        return coord.x >= 0 && coord.y >= 0 && coord.x < chunksX && coord.y < chunksY;
    }

    public GridChunk GetChunk(Vector2Int coord)
    {
        return IsChunkInBounds(coord) ? chunks[coord.x, coord.y] : null;
    }

    public GridChunk GetChunkAtCell(Vector2Int cellPos)
    {
        return GetCell(cellPos)?.Chunk;
    }

    private void SetChunkState(GridChunk chunk, ChunkState newState)
    {
        if (chunk.State != newState)
        {
            chunk.State = newState;
            OnChunkStateChanged?.Invoke(chunk);
        }
    }

    public void UnlockChunk(Vector2Int coord)
    {
        GridChunk chunk = GetChunk(coord);
        if (chunk == null || chunk.State == ChunkState.Owned)
        {
            return;
        }
        SetChunkState(chunk, ChunkState.Owned);
        foreach (Vector2Int dir in neighbourDirections)
        {
            Vector2Int neighbourCoord = coord + dir;
            GridChunk neighbourChunk = GetChunk(neighbourCoord);
            if (neighbourChunk != null && neighbourChunk.State == ChunkState.Locked)
            {
                SetChunkState(neighbourChunk, ChunkState.Purchasable);
            }
        }
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