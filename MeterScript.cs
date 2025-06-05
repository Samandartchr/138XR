using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using TMPro;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR;

public class VRMeasuringTape : MonoBehaviour
{
    public TextMeshPro lengthDisplay;
    public Transform yarnStart; // Fixed point on the core
    public Transform yarnEnd; // Movable point (controlled by VR controller)
    private InputDevice rightController; // Right VR controller

    private bool isGrabbed = false; // Check if the tool is grabbed
    private Vector3 initialYarnStartLocalPosition; // Initial local position of yarn start (relative to its parent)
    private Vector3 initialYarnEndLocalPosition; // Initial local position of yarn end (relative to its parent)
    private Quaternion initialYarnStartLocalRotation; // Initial local rotation of yarn start (relative to its parent)
    private Quaternion initialYarnEndLocalRotation; // Initial local rotation of yarn end (relative to its parent)

    void Start()
    {
        // Store the initial local positions and rotations of both yarn start and yarn end
        initialYarnStartLocalPosition = yarnStart.localPosition;
        initialYarnEndLocalPosition = yarnEnd.localPosition;
        initialYarnStartLocalRotation = yarnStart.localRotation;
        initialYarnEndLocalRotation = yarnEnd.localRotation;

        rightController = InputDevices.GetDeviceAtXRNode(XRNode.RightHand); // Get right controller
    }

    void Update()
    {
        if (isGrabbed)
        {
            UpdateYarnPosition(); // Update yarn position with the controller's position
        }
        UpdateLengthDisplay(); // Update the display with the current length
    }

    // Update the yarn end position based on controller position
    void UpdateYarnPosition()
    {
        Vector3 controllerPosition;
        if (rightController.TryGetFeatureValue(CommonUsages.devicePosition, out controllerPosition))
        {
            yarnEnd.position = controllerPosition; // Move yarn end with controller
        }
    }

    // Update the length display based on the distance between yarn start and end
    void UpdateLengthDisplay()
    {
        float distance = Vector3.Distance(yarnStart.position, yarnEnd.position);
        lengthDisplay.text = "Length: " + distance.ToString("F2") + " m"; // Show distance on display
    }

    // Triggered when the tool is grabbed
    private void HandleGrab(SelectEnterEventArgs arg)
    {
        isGrabbed = true;
    }

    // Triggered when the tool is released
    private void HandleRelease(SelectExitEventArgs arg)
    {
        isGrabbed = false;
        // Reset the yarn end and yarn start to their initial local positions and rotations
        yarnEnd.localPosition = initialYarnEndLocalPosition; // Reset yarn end to initial local position
        yarnEnd.localRotation = initialYarnEndLocalRotation; // Reset yarn end to initial local rotation

        yarnStart.localPosition = initialYarnStartLocalPosition; // Reset yarn start to initial local position
        yarnStart.localRotation = initialYarnStartLocalRotation; // Reset yarn start to initial local rotation

        lengthDisplay.text = "Length: 0.00 m"; // Reset length display
    }

    // Register listeners for the grab and release events
    private void OnEnable()
    {
        GetComponent<XRGrabInteractable>().selectEntered.AddListener(HandleGrab);
        GetComponent<XRGrabInteractable>().selectExited.AddListener(HandleRelease);
    }

    // Unregister listeners when the script is disabled
    private void OnDisable()
    {
        GetComponent<XRGrabInteractable>().selectEntered.RemoveListener(HandleGrab);
        GetComponent<XRGrabInteractable>().selectExited.RemoveListener(HandleRelease);
    }
}
