using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class MassMeasureTool : MonoBehaviour
{
    public TextMeshPro massText;
    private float totalMass = 0f;
    private HashSet<Rigidbody> objectsOnScale = new HashSet<Rigidbody>();

    private void OnCollisionEnter(Collision collision)
    {
        Rigidbody rb = collision.rigidbody;
        if (rb != null && !objectsOnScale.Contains(rb))
        {
            objectsOnScale.Add(rb);
            totalMass += rb.mass;
            UpdateMassDisplay();
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        Rigidbody rb = collision.rigidbody;
        if (rb != null && objectsOnScale.Contains(rb))
        {
            objectsOnScale.Remove(rb);
            totalMass -= rb.mass;
            UpdateMassDisplay();
        }
    }

    private void UpdateMassDisplay()
    {
        if (massText != null)
        {
            massText.text = "Mass: " + totalMass.ToString("F2") + " kg";
        }
    }
}
