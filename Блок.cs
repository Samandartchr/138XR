using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class HingeLikeRotation : MonoBehaviour
{
    [Header("Настройки вращения")]
    public Vector3 rotationAxis = Vector3.up;  // Ось вращения (например, (0, 1, 0) — вокруг Y)

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;  // Отключаем гравитацию, чтобы объект не падал
        rb.constraints = RigidbodyConstraints.FreezeAll;  // Замораживаем все оси

        // Размораживаем вращение только по выбранной оси
        if (rotationAxis == Vector3.right)
            rb.constraints = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
        else if (rotationAxis == Vector3.up)
            rb.constraints = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        else if (rotationAxis == Vector3.forward)
            rb.constraints = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY;
        else
            Debug.LogError("Некорректная ось вращения! Используйте Vector3.right, Vector3.up или Vector3.forward.");
    }
}
