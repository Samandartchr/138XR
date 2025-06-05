using UnityEngine;
using System;

public class Electrofor: MonoBehaviour
{
    //variables
    public Transform SourceObject;
    public Transform TargetObject1;
    public Transform TargetObject2;
    public Transform TargetObject3;

    public float SourceRotation;
    public float Target1Rotation;

    private void Start()
    {
        SourceRotation = SourceObject.localEulerAngles.z;
        Target1Rotation = TargetObject1.localEulerAngles.x;
    }

    private void Update()
    {
        SourceRotation = SourceObject.localEulerAngles.z;
        // Calculate target rotations
        Target1Rotation = SourceRotation * 0.5f;
        // Apply to Transforms
        TargetObject1.localEulerAngles = new Vector3(Target1Rotation, -90, 90);
        TargetObject2.localEulerAngles = new Vector3(-Target1Rotation, -90, 90);
        TargetObject3.localEulerAngles = new Vector3(0, 0, SourceRotation);
    }
}