using System;
using System.Collections.Generic;

namespace Codes
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

        public void PrintGraph()
        {
            foreach (var kvp in Nodes)
            {
                Console.Write($"Node {kvp.Key}: ");
                foreach (var (target, resistance) in kvp.Value)
                {
                    Console.Write($"-> Node {target} (R={resistance}) ");
                }
                Console.WriteLine();
    }
}


    }
    public class MatrixSolver
    {

        public static float[] SolveLeastSquares(float[,] A, float[] C)
        {
            int m = A.GetLength(0); // Rows
            int n = A.GetLength(1); // Columns
            float[,] ATA = new float[n, n];
            for (int i = 0; i < n; i++)
                for (int j = 0; j < n; j++)
                    for (int k = 0; k < m; k++)
                        ATA[i, j] += A[k, i] * A[k, j];
            float[] ATC = new float[n];
            for (int i = 0; i < n; i++)
                for (int k = 0; k < m; k++)
                    ATC[i] += A[k, i] * C[k];
            return SolveGaussian(ATA, ATC);
        }
        public static float[] SolveGaussian(float[,] A, float[] b)
        {
            int n = A.GetLength(0);
            float[,] M = (float[,])A.Clone();
            float[] x = (float[])b.Clone();
            for (int i = 0; i < n; i++)
            {
                if (Math.Abs(M[i, i]) < 1e-6f)
                    throw new Exception("Zero pivot");
                for (int j = i + 1; j < n; j++)
                {
                    float factor = M[j, i] / M[i, i];
                    for (int k = i; k < n; k++)
                        M[j, k] -= factor * M[i, k];
                    x[j] -= factor * x[i];
                }
            }
            float[] result = new float[n];
            for (int i = n - 1; i >= 0; i--)
            {
                float sum = x[i];
                for (int j = i + 1; j < n; j++)
                    sum -= M[i, j] * result[j];
                result[i] = sum / M[i, i];
            }
            return result;
        }
    }
    public class Kirchhoff
    {
        public Graph graph = new Graph();

        public float[,] BuildMatrix()
        {
            int e = graph.EdgeCount;
            var Loops = graph.FindLoops(0, 1);
            int l = Loops.Count;
            int n = graph.NodeCount;

            float[,] Matrix;

            if (l >= e)
            {
                Matrix = new float[l, e];
                for (int i = 0; i < l; i++)
                {
                    for (int j = 0; j < e; j++)
                    {
                        Matrix[i, j] = 0;
                    }
                }
            }
            else
            {
                Matrix = new float[l + n, e];
                for (int i = 0; i < l + n; i++)
                {
                    for (int j = 0; j < e; j++)
                    {
                        Matrix[i, j] = 0;
                    }
                }
            }

            // Fill KVL part
            for (int i = 0; i < l; i++)
            {
                for (int j = 0; j < Loops[i].Count - 1; j++)
                {
                    int from = Loops[i][j];
                    int to = Loops[i][j + 1];
                    int index = graph.EdgeIndex(from, to);

                    if (from < to)
                    {
                        var edge = graph.Nodes[from].Find(e => e.target == to);
                        Matrix[i, index] = edge.resistance; //here is a bug
                    }
                    else
                    {
                        var edge = graph.Nodes[to].Find(e => e.target == from);
                        Matrix[i, index] = -edge.resistance;
                    }
                }
            }

            // Add KCL part (only if needed)
            if (l < e)
            {
                for (int i = l; i < l + n; i++)
                {
                    int node = i - l;
                    if (!graph.Nodes.ContainsKey(node)) continue;

                    foreach (var edge in graph.Nodes[node])
                    {
                        int from = node;
                        int to = edge.target;
                        int index = graph.EdgeIndex(from, to);

                        if (from < to)
                            Matrix[i, index] = 1;
                        else
                            Matrix[i, index] = -1;
                    }
                }
            }

            return Matrix;
        }

        public float[] BuildValues(float Voltage)
        {
            int e = graph.EdgeCount;
            var Loops = graph.FindLoops(0, 1);
            int l = Loops.Count;
            int n = graph.NodeCount;

            float[] Values;
            if (l >= e)
                Values = new float[l];
            else
                Values = new float[l + n];

            for (int i = 0; i < l; i++)
            {
                Values[i] = Voltage;
            }
            for (int i = l; i < l + n; i++)
            {
                Values[i] = 0; // KCL nodes have no voltage source
            }
            return Values;
        }

        public float[] Currents(float Voltage)
        {
            float[,] matrix = BuildMatrix();
            float[] values = BuildValues(Voltage);
            return MatrixSolver.SolveLeastSquares(matrix, values);
        }
        
        public void PrintMatrix()
        {
            float[,] matrix = BuildMatrix();
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    Console.Write($"{matrix[i, j]:F2} ");
                }
                Console.WriteLine();
            }
        }
    }
    
}