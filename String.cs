using UnityEngine;

public class DynamicString : MonoBehaviour
{
    public Transform objectA; // First object
    public Transform objectB; // Second object
    private float initialDistance; // Store the starting distance

    void Start()
    {
        if (objectA != null && objectB != null)
        {
            initialDistance = Vector3.Distance(objectA.position, objectB.position); // Get initial distance
        }
    }

    void Update()
    {
        if (objectA != null && objectB != null)
        {
            float currentDistance = Vector3.Distance(objectA.position, objectB.position); // Calculate current distance
            float scaleY = currentDistance / initialDistance; // Scale Y proportionally
            transform.localScale = new Vector3(transform.localScale.x, scaleY, transform.localScale.z); // Apply scale change
        }
    }
}
