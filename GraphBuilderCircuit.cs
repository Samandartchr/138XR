using System;
using System.Collections.Generic;
using LinearAlgebra;

namespace GraphCircuit
{
    public class Builder
    {
        public Dictionary<int, List<(int target, float resistance)>> Nodes = new();

        public void AddNode(int nodeId)
        {
            if (!Nodes.ContainsKey(nodeId))
                Nodes[nodeId] = new List<(int, float)>();
        }

        public void AddComponent(int from, int to, float resistance)
        {
            AddNode(from);
            AddNode(to);
            Nodes[from].Add((to, resistance));
            Nodes[to].Add((from, resistance));
        }

        public void removeComponent(int from, int to)
        {
            if (Nodes.ContainsKey(from))
                Nodes[from].RemoveAll(x => x.target == to);
            if (Nodes.ContainsKey(to))
                Nodes[to].RemoveAll(x => x.target == from);
        }
    }
}
