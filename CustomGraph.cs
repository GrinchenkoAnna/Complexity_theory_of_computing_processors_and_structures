using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Complexity_theory_of_computing_processors_and_structures
{
    class CustomGraph
    {
        private int vertices;
        private int edges;
        private int?[,] adjacency;

        readonly Random random_graph = new();

        public int Vertices
        {
            get => vertices;
            set => vertices = value >= 0 ? value : throw new ArgumentException($"Недопустиммое количество вершин: {value}");
        } 
        public int Edges
        {
            get => edges;
            set => edges = value >= 0 ? value : throw new ArgumentException($"Недопустимое количество ребер: {value}");
        }        

        public CustomGraph() { }

        public CustomGraph(int range_vertices, int range_weight, bool oriented = false)
        {
            Vertices = random_graph.Next(1, range_vertices);
            Edges = random_graph.Next(range_vertices);
            adjacency = new int?[Vertices, Vertices];

            int min_weight = random_graph.Next(range_weight);
            int max_weight = min_weight + range_weight;

            for (int i = 0; i < Edges; i++)
            {
                int start = random_graph.Next(Vertices);
                int end = random_graph.Next(Vertices);
                int weight = random_graph.Next(min_weight, max_weight);
                AddEdge(start, end, weight, oriented);
            }
        }

        public CustomGraph(int num_vertices, int num_edges, int min_weight, int max_weight, bool oriented = false)
        {
            Vertices = num_vertices;
            Edges = num_edges;
            adjacency = new int?[Vertices, Vertices];

            for (int i = 0; i < Vertices; i++)
                for (int j = 0; j < Vertices; j++)
                    adjacency[i, j] = null;
        }

        public void AddEdge(int start, int end, int weight, bool oriented)
        {
            adjacency[start, end] = weight;
            if (!oriented)
                adjacency[end, start] = weight;
        }

        public void RemoveEdge(int start, int end)
        {
            adjacency[start, end] = null;
        }

        public void AddVertice(int vertice)
        {
            if (vertice > Vertices)
            {
                int?[,] temp = new int?[Vertices + 1, Vertices + 1];
                for (int i = 0; i < adjacency.GetLength(0); i++)
                    for (int j = 0; j < adjacency.GetLength(1); j++)
                        temp[i, j] = adjacency[i, j];

                adjacency = temp;
                Vertices++;

                for (int i = 0; i < Vertices; i++)
                    adjacency[i, Vertices - 1] = null;

                for (int j = 0; j < Vertices; j++)
                    adjacency[Vertices - 1, j] = null;
            }
            else
                throw new ArgumentException($"Такая вершина уже есть. Количество вершин: {Vertices}");
        }

        public void RemoveVertice(int vertice)
        {
            if (vertice <= Vertices)
            {
                int?[,] temp = new int?[Vertices - 1, Vertices - 1];
                int i_new, j_new, i_old, j_old;

                for (i_new = 0, i_old = 0; i_new < Vertices - 1 && i_old < Vertices; i_new++, i_old++)
                    for (j_new = 0, j_old = 0; i_new < Vertices - 1 && j_old < Vertices; j_new++, j_old++)
                    {
                        if (i_old == vertice) i_old++;
                        if (j_old == vertice) j_old++;
                        temp[i_new, j_new] = adjacency[i_old, j_old];
                    }

                adjacency = temp;
                Vertices--;
            }
            else
                throw new ArgumentException($"Такой вершины нет. Количество вершин: {Vertices}");
        }

        public void PrintGraph(int output_width = 5)
        {
            for (int i = 0; i < Vertices; i++)
            {
                for (int j = 0; j < Vertices; j++)
                {
                    if (adjacency[i, j] != null)
                        Console.Write($"{adjacency[i, j]}".PadLeft(output_width));
                    else 
                        Console.Write($"INF".PadLeft(output_width));
                }
                Console.WriteLine();
            }
        }
    }
}
