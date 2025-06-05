using UnityEngine;
using System.Collections;

public class Destroy : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Ion") { Destroy(other.gameObject); }
    }
}