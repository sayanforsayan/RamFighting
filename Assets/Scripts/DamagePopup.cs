using TMPro;
using UnityEngine;

public class DamagePopup : MonoBehaviour
{
    public TextMeshPro textMesh;
    private Vector3 moveDirection;
    private float speed = 1f;
    private Camera cam;

    void Start()
    {
        cam = Camera.main;

        // Random horizontal offset to reduce exact overlap
        float randomX = Random.Range(-0.3f, 0.3f);
        transform.position += new Vector3(randomX, 0, 0);

        // Move direction: always up + small random side move
        moveDirection = new Vector3(randomX, 1f, 0).normalized;
    }

    public void Show(int amount, Color color, bool isHealing)
    {
        textMesh.text = (isHealing ? "+" : "-") + amount;
        textMesh.color = color;
        Destroy(gameObject, 1.5f);
    }

    void Update()
    {
        // Move popup smoothly upward and slightly sideways
        transform.position += moveDirection * speed * Time.deltaTime;

        // Face the camera always
        transform.LookAt(cam.transform);

        // Clamp popup inside camera viewport
        Vector3 viewportPos = cam.WorldToViewportPoint(transform.position);
        viewportPos.x = Mathf.Clamp01(viewportPos.x);
        viewportPos.y = Mathf.Clamp01(viewportPos.y);
        viewportPos.z = Mathf.Max(viewportPos.z, 0.1f); // keep in front

        transform.position = cam.ViewportToWorldPoint(viewportPos);
    }
}
