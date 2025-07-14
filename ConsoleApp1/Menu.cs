using System;
using System.Collections.Generic;
using System.Diagnostics;
using Codes;

namespace Menu
{
    public static class TerminalMenu
    {
        public static Kirchhoff circuit = new Kirchhoff();
        public static Stopwatch stopwatch = new Stopwatch();

        public static void Menu()
        {
            Console.WriteLine("1: Add Component");
            Console.WriteLine("2: Show Circuit");
            Console.WriteLine("3: Solve Circuit");
            Console.WriteLine("4: Show Matrix");

            int input = Convert.ToInt32(Console.ReadLine());

            switch (input)
            {
                case 1:
                    Console.Write("from: ");
                    int from = Convert.ToInt32(Console.ReadLine());

                    Console.Write("to: ");
                    int to = Convert.ToInt32(Console.ReadLine());

                    Console.Write("resistance: ");
                    float resistance = Convert.ToSingle(Console.ReadLine());
                    
                    Console.WriteLine("Adding component...");
                    stopwatch.Start();
                    circuit.graph.AddEdge(from, to, resistance);
                    stopwatch.Stop();

                    Console.WriteLine($"Component added successfully. Time: {stopwatch.ElapsedMilliseconds} ms");
                    Menu();

                    break;
                case 2:
                    Console.WriteLine("Showing circuit...");
                    circuit.graph.PrintGraph();
                    Menu();
                    break;
                case 3:
                    Console.Write("Input voltage: ");
                    float inputVoltage = Convert.ToSingle(Console.ReadLine());
                    Console.WriteLine("Solving circuit...");
                    stopwatch.Start();
                    float[] result = circuit.Currents(inputVoltage);
                    stopwatch.Stop();
                    Console.WriteLine($"Calculation completed in {stopwatch.ElapsedMilliseconds} ms");
                    Console.WriteLine("Currents:");
                    for (int i = 0; i < circuit.graph.EdgeCount; i++)
                    {
                        for (int j = i + 1; j < circuit.graph.EdgeCount; j++)
                        {
                            if (circuit.graph.EdgeIndex(i, j) != -1)
                            {
                                Console.WriteLine($"{i} -> {j}: {result[circuit.graph.EdgeIndex(i, j)]} A");
                            }
                        }
                    }

                    Menu();
                    break;
                case 4:
                    Console.WriteLine("Showing matrix...");
                    circuit.PrintMatrix();
                    Menu();
                    break;
                default:
                    Console.WriteLine("Invalid option. Please try again.");
                    Menu();
                    break;
            }
        }
    }
}