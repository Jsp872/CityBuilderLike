using UnityEngine;

// Phase choosingChunk : surligne la parcelle sous la souris,
// au clic elle devient au joueur (gratuitement) et la partie commence.
public class StartChunkSelector : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private GridManager gridManager;
    [SerializeField] private GridCursor gridCursor;
    [SerializeField] private ChunkRenderer chunkRenderer;
    [SerializeField] private CameraController cameraController;

    private void Start()
    {
        // Si un champ n'est pas rempli dans l'Inspector, on le cherche dans la scene
        if (gameManager == null) gameManager = FindAnyObjectByType<GameManager>();
        if (gridManager == null) gridManager = FindAnyObjectByType<GridManager>();
        if (gridCursor == null) gridCursor = FindAnyObjectByType<GridCursor>();
        if (chunkRenderer == null) chunkRenderer = FindAnyObjectByType<ChunkRenderer>();
        if (cameraController == null) cameraController = FindAnyObjectByType<CameraController>();

        gridCursor.OnCellClicked += HandleCellClicked;
    }

    private void OnDestroy()
    {
        if (gridCursor != null)
        {
            gridCursor.OnCellClicked -= HandleCellClicked;
        }
    }

    private void Update()
    {
        if (gameManager.CurrentPhase != GamePhase.choosingChunk)
        {
            return;
        }

        GridChunk hovered = gridCursor.IsHoveringGrid ? gridManager.GetChunkAtCell(gridCursor.HoveredCell) : null;
        chunkRenderer.SetHighlight(hovered);
    }

    private void HandleCellClicked(Vector2Int cell)
    {
        // Le clic ne sert a choisir la parcelle que pendant cette phase
        if (gameManager.CurrentPhase != GamePhase.choosingChunk)
        {
            return;
        }

        GridChunk chunk = gridManager.GetChunkAtCell(cell);
        if (chunk == null)
        {
            return;
        }

        chunkRenderer.SetHighlight(null);
        gridManager.UnlockChunk(chunk.Coord);

        if (cameraController != null)
        {
            cameraController.FocusOn(gridManager.ChunkToWorld(chunk.Coord));
        }

        gameManager.SetPhase(GamePhase.playing);
    }
}
