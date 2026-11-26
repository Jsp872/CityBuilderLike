using UnityEngine;

public class ChunkRenderer : MonoBehaviour
{
    [SerializeField] private GridManager gridManager;
    [SerializeField] private SpriteRenderer overlayPrefab;
    [SerializeField] private Color lockedColor = new Color(0f, 0f, 0f, 0.55f);
    [SerializeField] private Color purchasableColor = new Color(1f, 0.85f, 0.2f, 0.25f);
    [SerializeField] private Color highlightColor = new Color(1f, 1f, 1f, 0.35f);

    private GridChunk highlighted;
    private SpriteRenderer[,] overlays;

    private void Start()
    {
        overlays = new SpriteRenderer[gridManager.ChunksX, gridManager.ChunksY];
        float size = gridManager.ChunkSize * gridManager.CellSize;

        for (int x = 0; x < gridManager.ChunksX; x++)
        {
            for (int y = 0; y < gridManager.ChunksY; y++)
            {
                GridChunk chunk = gridManager.GetChunk(new Vector2Int(x, y));

                SpriteRenderer overlay = Instantiate(overlayPrefab, transform);
                overlay.transform.position = gridManager.ChunkToWorld(chunk.Coord);
                overlay.transform.localScale = new Vector3(size, size, 1f);

                overlays[x, y] = overlay;
                RefreshChunk(chunk);
            }
        }

        gridManager.OnChunkStateChanged += RefreshChunk;
    }

    private void RefreshChunk(GridChunk chunk)
    {
        Vector2Int coord = chunk.Coord;
        SpriteRenderer overlay = overlays[coord.x, coord.y];

        // La surbrillance passe avant la couleur de l'etat
        if (chunk == highlighted)
        {
            overlay.color = highlightColor;
            overlay.gameObject.SetActive(true);
            return;
        }

        if (chunk.State == ChunkState.Locked)
        {
            overlay.color = lockedColor;
            overlay.gameObject.SetActive(true);
        }
        else if (chunk.State == ChunkState.Purchasable)
        {
            overlay.color = purchasableColor;
            overlay.gameObject.SetActive(true);
        }
        else
        {
            overlay.gameObject.SetActive(false);
        }
    }

    // Surligne une parcelle (null = plus aucune surbrillance)
    public void SetHighlight(GridChunk chunk)
    {
        if (chunk == highlighted)
        {
            return;
        }

        GridChunk previous = highlighted;
        highlighted = chunk;

        if (previous != null)
        {
            RefreshChunk(previous);
        }
        if (chunk != null)
        {
            RefreshChunk(chunk);
        }
    }

    private void OnDestroy()
    {
        if (gridManager != null)
            gridManager.OnChunkStateChanged -= RefreshChunk;
    }
}
