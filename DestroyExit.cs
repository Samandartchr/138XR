using UnityEngine;
using System.Collections;

public class DestroyExit : MonoBehaviour
{
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "Ion") { Destroy(other.gameObject); }
    }
}
