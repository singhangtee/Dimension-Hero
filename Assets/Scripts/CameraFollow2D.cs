using UnityEngine;

public class CameraFollow2DWithClamp : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset = new Vector3(0, 0, -10);
    [SerializeField] private float smoothSpeed = 5f;

    [Header("Zoom Settings")]
    [SerializeField] private float zoomSpeed = 5f;
    [SerializeField] private float minZoom = 5f;
    [SerializeField] private float maxZoom = 15f;

    [Header("Mouse Look-Ahead")]
    [SerializeField] private float lookAheadStrength = 0.25f;
    [SerializeField] private float screenEdgeBuffer = 0.9f; // 90% of screen before clamping

    private Camera cam;

    void Start()
    {
        cam = GetComponent<Camera>();
    }

    void LateUpdate()
    {
        if (target == null) return;

        // Zoom control (scroll wheel)
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (Mathf.Abs(scroll) > 0.01f)
        {
            cam.orthographicSize -= scroll * zoomSpeed;
            cam.orthographicSize = Mathf.Clamp(cam.orthographicSize, minZoom, maxZoom);
        }

        // Mouse-based look-ahead
        Vector3 mouseWorldPos = cam.ScreenToWorldPoint(Input.mousePosition);
        Vector3 mouseOffset = (mouseWorldPos - target.position);
        mouseOffset.z = 0f;

        Vector3 desiredPosition = target.position + offset + mouseOffset * lookAheadStrength;

        // Clamp camera so player stays within view
        desiredPosition = ClampCameraPosition(desiredPosition, target.position);

        // Smooth movement
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        transform.position = smoothedPosition;
    }

    Vector3 ClampCameraPosition(Vector3 camPos, Vector3 playerPos)
    {
        float vertExtent = cam.orthographicSize;
        float horzExtent = vertExtent * cam.aspect;

        float maxOffsetX = horzExtent * screenEdgeBuffer;
        float maxOffsetY = vertExtent * screenEdgeBuffer;

        Vector3 offsetFromPlayer = camPos - playerPos;

        offsetFromPlayer.x = Mathf.Clamp(offsetFromPlayer.x, -maxOffsetX, maxOffsetX);
        offsetFromPlayer.y = Mathf.Clamp(offsetFromPlayer.y, -maxOffsetY, maxOffsetY);

        return playerPos + offsetFromPlayer + offset;
    }
}
