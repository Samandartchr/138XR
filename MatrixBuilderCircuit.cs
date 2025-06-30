using System;
using System.Collections.Generic;
using LinearAlgebra;

public class MatrixBuilderCircuit
{
    public class Component
    {
        public int Index;
        public int FromNodeIndex;
        public int ToNodeIndex;
        public float Resistance;
    }

    public class Node
    {
        public int Index;
        public float[] ConnectedComponentsIndex;

    }

    public List<Component> Components = new List<Component>();
    public List<Node> Nodes = new List<Node>();
}