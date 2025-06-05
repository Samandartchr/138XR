using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class YarnStartJoint : MonoBehaviour
{
    public Rigidbody yarnRigidbody;
    public Rigidbody toolRigidbody; // Assign Tool's Rigidbody in the Inspector

    private FixedJoint fixedJoint;
    private XRGrabInteractable grabInteractable;

    void Start()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();

        if (yarnRigidbody == null)
            yarnRigidbody = GetComponent<Rigidbody>();

        CreateJoint();
    }

    void CreateJoint()
    {
        if (toolRigidbody != null)
        {
            fixedJoint = gameObject.AddComponent<FixedJoint>();
            fixedJoint.connectedBody = toolRigidbody;
            fixedJoint.breakForce = Mathf.Infinity;
            fixedJoint.breakTorque = Mathf.Infinity;
        }
    }

    private void OnEnable()
    {
        grabInteractable.selectEntered.AddListener(OnGrab);
        grabInteractable.selectExited.AddListener(OnRelease);
    }

    private void OnDisable()
    {
        grabInteractable.selectEntered.RemoveListener(OnGrab);
        grabInteractable.selectExited.RemoveListener(OnRelease);
    }

    private void OnGrab(SelectEnterEventArgs arg)
    {
        if (fixedJoint != null)
        {
            Destroy(fixedJoint); // Remove the joint so Yarn Start moves freely
        }
    }

    private void OnRelease(SelectExitEventArgs arg)
    {
        CreateJoint(); // Reconnect Yarn Start to the Tool
    }
}
