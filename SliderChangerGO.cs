using UnityEngine;

public class SliderChangerGO : MonoBehaviour
{
    private GameObject SliderObject;

    public enum SliderAxis { X, Y, Z }
    public SliderAxis sliderAxis;

    public bool direction = false;

    public float minValue;
    public float maxValue;

    // Normalized slider value [0,1]
    public float Value { get; private set; }

    void Start()
    {
        SliderObject = this.gameObject;
    }

    void Update()
    {
        float pos = 0f;

        switch (sliderAxis)
        {
            case SliderAxis.X:
                pos = SliderObject.transform.localPosition.x;
                break;
            case SliderAxis.Y:
                pos = SliderObject.transform.localPosition.y;
                break;
            case SliderAxis.Z:
                pos = SliderObject.transform.localPosition.z;
                break;
        }

        if (direction)
        {
            // Invert slider direction
            pos = -pos;
        }

        // Clamp position within min and max
        pos = Mathf.Clamp(pos, minValue, maxValue);

        // Normalize to 0..1 range
        Value = Mathf.InverseLerp(minValue, maxValue, pos);
    }
}
