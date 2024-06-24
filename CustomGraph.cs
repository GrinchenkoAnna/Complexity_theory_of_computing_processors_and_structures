using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Complexity_theory_of_computing_processors_and_structures
{
    class CustomGraph
    {
        private int num_vertices;
        private int?[,] adjacency;

        public int NumVertices
        {
            get => num_vertices;
            set => num_vertices = value >= 0 ? value : throw new ArgumentException("Недопустимый размер графа");
        }        

        public CustomGraph() { }

        public CustomGraph(int size, bool random = false)
        {
            NumVertices = size;
            adjacency = new int?[NumVertices, NumVertices];
            if (!random)
            {
                for (int i = 0; i < NumVertices; i++)
                    for (int j = 0; j < NumVertices; j++)
                        adjacency[i, j] = null;
            }
            else
            {
                //рандомно заполнить (как?)
            }            
        }

        public void AddEdge(int start, int end, int weight, bool oriented = false)
        {
            adjacency[start, end] = weight;
            if (!oriented)
                adjacency[end, start] = weight;
        }

        public void RemoveEdge(int start, int end)
        {
            adjacency[start, end] = null;
        }

        public void PrintGraph(int output_width = 5)
        {
            for (int i = 0; i < NumVertices; i++)
            {
                for (int j = 0; j < NumVertices; j++)
                {
                    if (adjacency[i, j] != null)
                        Console.Write($"{adjacency[i, j]}".PadLeft(output_width));
                    else Console.Write($"INF".PadLeft(output_width));
                }
                Console.WriteLine();
            }
        }
    }
}
