using UnityEngine;

public class CameraFollowZoom : MonoBehaviour
{
    public Transform target1;
    public Transform target2;
    public float smoothTime = 0.5f;
    public Vector3 offset = new Vector3(0, 10, -10); // Adjust based on your current setup
    public float minDistance = 10f;
    public float maxDistance = 20f;
    public float zoomLimiter = 50f;

    private Vector3 velocity;

    void LateUpdate()
    {
        if (target1 == null || target2 == null) return;

        MoveCamera();
    }

    void MoveCamera()
    {
        Vector3 centerPoint = GetCenterPoint();
        Vector3 newPosition = centerPoint + offset;

        // Smooth movement
        transform.position = Vector3.SmoothDamp(transform.position, newPosition, ref velocity, smoothTime);

        // Optional: adjust FOV based on distance between targets
        float distance = Vector3.Distance(target1.position, target2.position);
        Camera.main.fieldOfView = Mathf.Lerp(40f, 60f, distance / zoomLimiter); // Adjust FOV range as needed
    }

    Vector3 GetCenterPoint()
    {
        return (target1.position + target2.position) / 2f;
    }
}
