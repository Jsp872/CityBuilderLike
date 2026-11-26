using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    [Header("Move")]
    [SerializeField] private float moveSpeed = 10f;

    [Header("Rotate")]
    [SerializeField] private float rotateSpeed = 90f;

    [Header("Zoom")]
    [SerializeField] private float zoomStep = 1f;
    [SerializeField] private float minZoom = 3f;
    [SerializeField] private float maxZoom = 30f;

    private Camera cam;
    private Vector2 moveInput;
    private float rotateInput;

    private void Awake()
    {
        cam = GetComponent<Camera>();
    }

    private void Update()
    {
        HandleMovement();
        HandleRotation();
    }

    private void HandleMovement()
    {
        float speed = moveSpeed * cam.orthographicSize / 10f;

        transform.Translate(moveInput * speed * Time.unscaledDeltaTime, Space.Self);
    }

    private void HandleRotation()
    {
        transform.Rotate(Vector3.forward, rotateInput * rotateSpeed * Time.unscaledDeltaTime);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void OnRotate(InputAction.CallbackContext context)
    {
        rotateInput = context.ReadValue<float>();
    }

    public void OnZoom(InputAction.CallbackContext context)
    {
        float scroll = context.ReadValue<float>();
        if (scroll == 0f)
        {
            return;
        }
            

        cam.orthographicSize -= Mathf.Sign(scroll) * zoomStep;
        cam.orthographicSize = Mathf.Clamp(cam.orthographicSize, minZoom, maxZoom);
    }
}