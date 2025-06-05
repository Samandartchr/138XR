using UnityEngine;

public class DCPowerSupply : MonoBehaviour
{
    public Transform positive;
    public Transform negative;
    public Transform voltageChanger;

    public enum voltageChangerType { Rotation, Slider }
    public voltageChangerType changerType;

    public float maxVoltage = 30f;

    private float voltage;
    public float Voltage => voltage;

    private float Value;

    private RotationChanger rotationChanger;
    private SliderChangerGO sliderChanger;

    private void Start()
    {
        // Cache reference to avoid GetComponent every frame
        if (changerType == voltageChangerType.Rotation)
            rotationChanger = voltageChanger.GetComponent<RotationChanger>();
        else if (changerType == voltageChangerType.Slider)
            sliderChanger = voltageChanger.GetComponent<SliderChangerGO>();
    }

    private void Update()
    {
        ValueSet();
        voltage = Mathf.Clamp01(Value) * maxVoltage;
    }

    private void ValueSet()
    {
        switch (changerType)
        {
            case voltageChangerType.Rotation:
                Value = rotationChanger?.Value ?? 0f;
                break;
            case voltageChangerType.Slider:
                Value = sliderChanger?.Value ?? 0f;
                break;
        }
    }
}
