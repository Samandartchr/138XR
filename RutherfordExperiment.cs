using UnityEngine;
using static UnityEditor.PlayerSettings;

public class RutherfordExperiment : MonoBehaviour
{
    private bool isScattered;
    private GameObject ION;
    public Transform LLs;

    private float[] RightLeft = {0,  0.1f, -0.1f,  0.2f, -0.2f, 0.3f, -0.3f, 
                              0.5f, -0.5f,  0.7f, -0.7f,     1,   -1,     3, 
                                -3,     6,    -6,     6,    -6,    5,    -5 };

    private float[] FrontBack = {0,     0,     0,     0,     0,    0,     0, 
                              0.3f,  0.3f,  0.5f,  0.5f,  0.5f, 0.5f,     1, 
                                 1,     3,     3,   -10,  -10,   -15, -15};

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Ion")
        {
            isScattered = true;
            ION = other.gameObject;
            int index = Random.Range(0, RightLeft.Length);
            float ForceLR = RightLeft[index];
            float ForceFB = FrontBack[index];
            ION.GetComponent<Rigidbody>().AddForce(ForceLR * 0.1f, 0, ForceFB * 0.2f, ForceMode.Impulse);
        }
    }

    private void Start()
    {
        
    }

    // Update is called once per frame  
    private void FixedUpdate()
    {
        if (isScattered)
        {
            if (ION != null)
            {
                Destroy(ION, 1f);
            }
        }
    }
}
