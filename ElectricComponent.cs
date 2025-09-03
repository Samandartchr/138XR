using System;
using System.Collections.Generic;
using UnityEngine;

public class ElectricComponent : MonoBehaviour
{
    public float Resistance = 0.1f; // Example resistance value
    public Transform PortA;
    public Transform PortB;

    private float current;
    private float voltage = Current * Resistance; // Ohm's Law: V = I * R
    public float Current => current; // Getter only
    public float Voltage => voltage; // Getter only

    public void SetCurrent(float value)
    {
        current = value;
    }

}