using UnityEngine;

public class CameraFollow2D : MonoBehaviour
{
    [SerializeField] private Transform target; // Your player
    [SerializeField] private Vector3 offset = new Vector3(0, 0, -10); // Camera offset
    [SerializeField] private float smoothSpeed = 5f; // Smoothing

    void LateUpdate()
    {
        if (target != null)
        {
            Vector3 desiredPosition = target.position + offset;
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
            transform.position = smoothedPosition;
        }
    }
}
