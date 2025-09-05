using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Math;
using PhysicsLaws;
using Tools;

public class ElectricCircuit : MonoBehaviour
{
    public float ResistanceOfDCPowerSupply = 0.1f; // Example resistance value

    public Transform ZerothNode;
    public Transform FirstNode;

    private List<List<Transform>> nodes = new List<List<Transform>>();
    private List<Transform> FlatSet = new List<Transform>();

    private Graph circuit = new Graph();

    private void Start()
    {
        Initializer();
    }

    float timer = 0f;
    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= 2f)
        {
            ListBuilder(ZerothNode, FirstNode);
            ListToGraph();

            float voltage = GetComponent<DCPowerSupply>().Voltage;
            float[] Currents = Kirchhoff.Currents(circuit, voltage);

            List<Transform> Checked = new List<Transform>();
            foreach (Transform port in FlatSet)
            {
                if (Checked.Contains(port))
                    continue;

                int i = nodes.FindIndex(node => node.Contains(port));
                Transform secondPort = port.GetSecondPort();
                int j = nodes.FindIndex(node => node.Contains(secondPort));

                int index = circuit.EdgeIndex(i, j);
                if (index < 0 || index >= Currents.Length) continue;
                port.GetComponent<ElectricPort>().ElectricComponent.GetComponent<ElectricComponent>().SetCurrent(Currents[index]);

                Checked.Add(port);
                Checked.Add(secondPort);
            }

            timer = 0f;
        }
    }

    private void Initializer()
    {
        nodes.Clear();
        circuit.Nodes.Clear();
        FlatSet.Clear();
        circuit.AddEdge(0, 1, ResistanceOfDCPowerSupply);
    }

    private void ListToGraph()
    {

        for (int i = 0; i < nodes.Count; i++)
        {
            foreach (Transform port in nodes[i])
            {
                Transform secondPort = port.GetSecondPort();
                if (secondPort == null)
                    continue;

                // Find which node group contains secondPort
                int j = nodes.FindIndex(node => node.Contains(secondPort));

                // Only add edge if j is valid and not same node
                if (j >= 0 && j != i)
                {
                    float resistance = port.GetResistance();
                    circuit.AddEdge(i, j, resistance);
                }
            }
        }
    }


    private void ListBuilder(Transform portA, Transform portB)
    {
        Initializer();
        List<Transform> visited = new List<Transform>();
        List<Transform> executionOrder = new List<Transform>();

        executionOrder.Add(portA);
        executionOrder.Add(portB);

        while (executionOrder.Count > 0)
        {
            List<Transform> executionOrderNew = new List<Transform>();

            foreach (Transform port in executionOrder)
            {
                List<Transform> newPorts = port.GetPortsSameNode();
                nodes.Add(newPorts);
                visited.AddRange(newPorts);
                executionOrderNew.AddRange(newPorts);
            }

            executionOrder.Clear();
            executionOrder.AddRange(executionOrderNew);
            executionOrderNew.Clear();

            foreach (Transform port in executionOrder)
            {
                Transform secondPort = port.GetSecondPort();
                if (secondPort != null && !visited.Contains(secondPort))
                {
                    executionOrderNew.Add(secondPort);
                }
            }

            executionOrder.Clear();
            executionOrder.AddRange(executionOrderNew);
            executionOrderNew.Clear();
        }
        
        FlatSet = nodes.SelectMany(node => node).Distinct().ToList();
    }
}

public static class XRInteractionExtensions
{
    public static GameObject GrabbedObject(this Transform t)
    {
        var socket = t.GetComponent<XRSocketInteractor>();
        if (socket == null) return null;

        var grabbed = socket.firstInteractableSelected as XRGrabInteractable;
        return grabbed != null ? grabbed.gameObject : null;
    }

    public static GameObject GrabbingObject(this Transform t)
    {
        var grab = t.GetComponent<XRGrabInteractable>();
        if (grab == null) return null;

        foreach (var interactor in grab.interactorsSelecting)
        {
            if (interactor is XRSocketInteractor socket)
                return socket.gameObject;
        }
        return null;
    }

    public static List<Transform> GetPortsSameNode(this Transform t)
    {
        List<Transform> ports = new List<Transform>();
        ports.Add(t);

        Transform forward = t;
        while (forward.GrabbedObject() != null)
        {
            forward = forward.GrabbedObject().transform;
            if (!ports.Contains(forward))
                ports.Add(forward);
            else
                break; // Avoid loop
        }

        Transform backward = t;
        while (backward.GrabbingObject() != null)
        {
            backward = backward.GrabbingObject().transform;
            if (!ports.Contains(backward))
                ports.Add(backward);
            else
                break; // Avoid loop
        }

        return ports;
    }

    public static float GetResistance(this Transform t)
    {
        float resistance;
        return resistance = t.GetComponent<ElectricPort>().ElectricComponent.GetComponent<ElectricComponent>().Resistance;
    }

    public static Transform GetSecondPort(this Transform t)
    {
        Transform secondPort = t.GetComponent<ElectricPort>().SecondPort;
        return secondPort;
    }
}