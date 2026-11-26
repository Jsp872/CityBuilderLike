using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class GridCursor : MonoBehaviour
{
    [SerializeField] private GridManager gridManager;
    [SerializeField] private Transform highlight;

    private Camera cam;
    private Vector2 pointerScreenPos;

    public Vector2Int HoveredCell { get; private set; }
    public bool IsHoveringGrid { get; private set; }


    public event Action<Vector2Int> OnCellClicked;

    private void Awake()
    {
        cam = Camera.main;
    }

    private void Update()
    {
        Vector3 world = cam.ScreenToWorldPoint(pointerScreenPos);
        world.z = 0f;
        HoveredCell = gridManager.WorldToCell(world);
        IsHoveringGrid = gridManager.IsInBounds(HoveredCell);

        if (IsHoveringGrid)
        {
            highlight.position = gridManager.CellToWorld(HoveredCell);
            highlight.gameObject.SetActive(true);
        }
        else
        {
            highlight.gameObject.SetActive(false);
        }
    }

    public void OnPoint(InputAction.CallbackContext context)
    {
        pointerScreenPos = context.ReadValue<Vector2>();
    }

    public void OnClick(InputAction.CallbackContext context)
    {
        if (context.performed && IsHoveringGrid)
        {
            OnCellClicked?.Invoke(HoveredCell);
        }
    }
}
