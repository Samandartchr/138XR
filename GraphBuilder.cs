using System;
using System.Collections.Generic;
using LinearAlgebra;

namespace Tools
{
    public class Graph
    {
        public Dictionary<int, List<(int target, float resistance)>> Nodes = new();
        public int NodeCount => Nodes.Count;
        public int EdgeCount
        {
            get
            {
                int count = 0;
                foreach (var edges in Nodes.Values)
                {
                    count += edges.Count;
                }
                if (count % 2 == 0)
                {
                    return count / 2;
                }
                else { return 0; }
            }
        }
        public int EdgeIndex(int from, int to)
        {
            if (from < to)
            {
                int index = 0;
                for (int i = 0; i < Nodes[from].Count - 1; i++)
                {
                    index += Nodes[i].Count;
                }
                foreach (var i in Nodes[from])
                {
                    if (i.target == to)
                    {
                        return index;
                    }
                    index++; // Might be displaced
                }
            }
            else if (from > to)
            {
                int index = 0;
                for (int i = 0; i < Nodes[to].Count - 1; i++)
                {
                    index += Nodes[i].Count;
                }
                foreach (var i in Nodes[to])
                {
                    if (i.target == from)
                    {
                        return index;
                    }
                    index++; // Might be displaced
                }
            }
            return -1;
        }

        public void AddNode(int nodeId)
        {
            if (!Nodes.ContainsKey(nodeId))
            {
                Nodes[nodeId] = new List<(int, float)>();
            }
        }

        public void AddEdge(int from, int to, float resistance)
        {
            AddNode(from);
            AddNode(to);
            Nodes[from].Add((to, resistance));
            Nodes[to].Add((from, resistance));
        }

        public void RemoveNode(int nodeId) // It is less used
        {
            if (Nodes.ContainsKey(nodeId))
            {
                Nodes.Remove(nodeId);
                foreach (var key in Nodes.Keys)
                {
                    Nodes[key].RemoveAll(x => x.target == nodeId);
                }
                for (int i = nodeId; i < NodeCount; i++)
                {
                    if (Nodes.ContainsKey(i))
                    {
                        Nodes[i - 1] = Nodes[i];
                    }
                }
                foreach (var Value in Nodes.Values)
                {
                    if (Value[0].target > nodeId)
                    {
                        for (int i = 0; i < Value.Count; i++)
                        {
                            if (Value[i].target > nodeId)
                            {
                                Value[i] = (Value[i].target - 1, Value[i].resistance);
                            }
                        }
                    }
                }
            }
        }

        public void RemoveEdge(int from, int to)
        {
            if (Nodes.ContainsKey(from))
                Nodes[from].RemoveAll(x => x.target == to);
            if (Nodes.ContainsKey(to))
                Nodes[to].RemoveAll(x => x.target == from);
        }

        public void JoinNodes(int from, int to)
        {
            if (from < to)
            {
                if (Nodes.ContainsKey(from) && Nodes.ContainsKey(to))
                {
                    foreach (var edge in Nodes[to])
                    {
                        if (edge.target != from)
                        {
                            AddEdge(from, edge.target, edge.resistance);
                        }
                    }
                    RemoveNode(to);
                }
            }
            else
            {
                JoinNodes(to, from);
            }
        }
        

        public List<List<int>> FindLoops(int Start, int End)
        {
            var allPaths = new List<List<int>>();
            var visited = new HashSet<int>();
            var currentPath = new List<int>();

            void DFS(int current)
            {
                currentPath.Add(current);
                visited.Add(current);

                if (current == End)
                {
                    allPaths.Add(new List<int>(currentPath));
                }
                else
                {
                    foreach (var (neighbor, _) in Nodes[current])
                    {
                        if (!visited.Contains(neighbor))
                            DFS(neighbor);
                    }
                }

                currentPath.RemoveAt(currentPath.Count - 1);
                visited.Remove(current);
            }

            DFS(Start);
            return allPaths;
        }

    }
}
