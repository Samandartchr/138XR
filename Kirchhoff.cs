using System;
using System.Collections.Generic;
using LinearAlgebra;
using Tools;

namespace PhysicsLaws
{
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
                Matrix = new float[l, e];
            else
                Matrix = new float[l + n, e];

            // Fill KVL part
            for (int i = 0; i < l; i++)
            {
                for (int j = 0; j < Loops[i].Count - 1; j++)
                {
                    int from = Loops[i][j];
                    int to = Loops[i][j + 1];
                    int index = graph.EdgeIndex(from, to);

                    if (from < to)
                        Matrix[i, index] = graph.Nodes[from][to].resistance;
                    else
                        Matrix[i, index] = -graph.Nodes[to][from].resistance;
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
    }
}