using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using TMPro;

public class VRStopwatch : MonoBehaviour
{
    public TextMeshPro timerText;
    private float startTime;
    private bool isRunning = false;
    private bool isGrabbed = false;
    private float elapsedTime = 0f;

    void Update()
    {
        if (isRunning && !isGrabbed)
        {
            elapsedTime += Time.deltaTime;
            timerText.text = FormatTime(elapsedTime);
        }
    }

    public void StartStopwatch()
    {
        startTime = Time.time;
        isRunning = true;
    }

    public void StopStopwatch()
    {
        isRunning = false;
    }

    private void OnEnable()
    {
        GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>().selectEntered.AddListener(HandleGrab);
        GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>().selectExited.AddListener(HandleRelease);
    }

    private void OnDisable()
    {
        GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>().selectEntered.RemoveListener(HandleGrab);
        GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>().selectExited.RemoveListener(HandleRelease);
    }

    private void HandleGrab(SelectEnterEventArgs arg)
    {
        isGrabbed = true;
        StopStopwatch();
        timerText.text = "00:00:000"; // Шарды ұстағанда нөлді көрсету
    }

    private void HandleRelease(SelectExitEventArgs arg)
    {
        isGrabbed = false;
        elapsedTime = 0f; // Таймерді нөлге қою
        StartStopwatch();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Cylinder"))
        {
            StopStopwatch();
            Debug.Log("Соқтығысқандағы уақыт: " + FormatTime(elapsedTime));
        }
    }

    private string FormatTime(float time)
    {
        int minutes = (int)time / 60;
        int seconds = (int)time % 60;
        int milliseconds = (int)((time * 1000) % 1000);
        return string.Format("{00}:{01}:{02}", minutes.ToString("00"), seconds.ToString("00"), milliseconds.ToString("000"));
    }
}