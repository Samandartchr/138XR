using UnityEngine;

public class RotationChanger : MonoBehaviour
{
    private GameObject RotationObject;

    //Later we can add Hinge Joints or other components to control rotation
    public float minRotation = 0f;
    public float maxRotation = 360f;

    public enum RotationAxis { X, Y, Z }
    public RotationAxis rotationAxis;

    public bool direction = false;

    public float Value { get; private set; }

    void Start()
    {
        RotationObject = this.gameObject;
    }

    void Update()
    {
        float angle = GetRotationAngle();

        // Normalize to 0–1 based on min and max
        Value = Mathf.InverseLerp(minRotation, maxRotation, angle);
    }

    float GetRotationAngle()
    {
        float angle = 0f;
        switch (rotationAxis)
        {
            case RotationAxis.X:
                angle = RotationObject.transform.localEulerAngles.x;
                break;
            case RotationAxis.Y:
                angle = RotationObject.transform.localEulerAngles.y;
                break;
            case RotationAxis.Z:
                angle = RotationObject.transform.localEulerAngles.z;
                break;
        }

        if (direction)
        {
            // Flip direction relative to 360
            angle = 360f - angle;
        }

        // Keep in 0-360 range
        angle = angle % 360f;

        return angle;
    }
}