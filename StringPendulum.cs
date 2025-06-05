using UnityEngine;

public class CustomSpringJoint : MonoBehaviour
{
    public Transform connectedObject; // The object the spring is attached to
    public float springStrength = 10f; // Stiffness (k)
    public float damping = 0.1f; // Controls energy loss (set low for infinite oscillations)
    public float restLength = 2f; // Natural length of the spring

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if (connectedObject == null) return;

        // Calculate displacement vector (current length - rest length)
        Vector3 direction = transform.position - connectedObject.position;
        float currentLength = direction.magnitude;
        Vector3 forceDirection = direction.normalized;

        // Hooke�s Law: F = -k * x
        float stretch = currentLength - restLength;
        Vector3 springForce = -springStrength * stretch * forceDirection;

        // Apply damping force to reduce excessive energy loss
        Vector3 dampingForce = -damping * rb.linearVelocity;

        // Apply the forces
        rb.AddForce(springForce + dampingForce);
    }
}
