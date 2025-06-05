using UnityEngine;

public class ConnectObjectsWithLine : MonoBehaviour
{
    public Transform objectA; // First object
    public Transform objectB; // Second object
    public Color lineColor = Color.red; // Line color
    public float lineWidth = 0.05f; // Line width

    private LineRenderer lineRenderer;

    void Start()
    {
        // Add a LineRenderer component if not already attached
        lineRenderer = gameObject.AddComponent<LineRenderer>();

        // Set the material to a default one if necessary
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));

        // Set the initial properties of the line
        lineRenderer.startColor = lineColor;
        lineRenderer.endColor = lineColor;
        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;
        lineRenderer.positionCount = 2; // Only two points needed
    }

    void Update()
    {
        if (objectA != null && objectB != null)
        {
            lineRenderer.SetPosition(0, objectA.position); // Start point
            lineRenderer.SetPosition(1, objectB.position); // End point
        }

        // Update line properties in case they change from the Inspector
        lineRenderer.startColor = lineColor;
        lineRenderer.endColor = lineColor;
        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;
    }
}
