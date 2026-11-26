using UnityEngine;
using UnityEngine.Tilemaps;

public class GroundRenderer : MonoBehaviour
{
    [SerializeField] private GridManager gridManager;
    [SerializeField] private Tilemap groundTilemap;
    [SerializeField] private TileBase groundTile;
    [SerializeField] private Color colorA = Color.green;
    [SerializeField] private Color colorB = Color.darkGreen;

    private void Start()
    {
        RenderGround();
    }

    private void RenderGround()
    {
        for (int x = 0; x < gridManager.Width; x++)
        {
            for (int y = 0; y < gridManager.Height; y++)
            {
                Vector3Int tilePosition = new Vector3Int(x, y, 0);
                groundTilemap.SetTile(tilePosition, groundTile);

                Color tileColor = (x + y) % 2 == 0 ? colorA : colorB;
                groundTilemap.SetColor(tilePosition, tileColor);
            }
        }
    }
}
