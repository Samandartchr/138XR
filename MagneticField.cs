using UnityEngine;
using System.Collections;

public class ExampleClass : MonoBehaviour
{
    private float charge;
    private Vector3 MM;
    public Transform LLs;
    private bool isActive;
    private GameObject ION;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Ion")
        {
            isActive = true;
            ION = other.gameObject;
        }
    }

    void OnTriggerExit(Collider other) // Fixed: Added 'void' return type  
    {
        if (other.gameObject.name == "Ion")
        {
            isActive = false;
            ION = other.gameObject;
        }
    }

    private void Start()
    {
        LorentzSimulation simulation = LLs.GetComponent<LorentzSimulation>();
        charge = simulation.Charge;
        MM = simulation.MagneticField;

    }
    private void FixedUpdate()
    {
        if (isActive)
        {
            if (ION != null)
            { 
                // Calculate the Lorentz force
                Vector3 velocity = ION.GetComponent<Rigidbody>().linearVelocity;
                Vector3 lorentzForce = charge * Vector3.Cross(velocity, MM);
                // Apply the force to the ion
                ION.GetComponent<Rigidbody>().AddForce(lorentzForce, ForceMode.Force);
            }
        }

        if (!isActive)
        {
            if (ION != null)
            {
                Vector3 Free = new Vector3(0, 0, 0);
                // Stop applying the force when the ion exits the trigger
                ION.GetComponent<Rigidbody>().AddForce(Free, ForceMode.Force);

                Destroy(ION, 1.5f);
            }
        }
    }
}
