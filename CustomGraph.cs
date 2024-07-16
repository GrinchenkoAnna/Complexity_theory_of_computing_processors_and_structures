using System;
using System.Collections;
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
        public List<int>[,] weight_matrix; //0 - weight; 1 - промежуточная вершина, если есть

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
            Vertices = random_graph.Next(4, range_vertices);
            Edges = 0;
            adjacency = new int?[Vertices, Vertices];
            weight_matrix = new List<int>[Vertices, Vertices];
            for (int i = 0; i < Vertices; i++)
                for (int j = 0; j < Vertices; j++)
                    weight_matrix[i, j] = [];            

            BuildSpanningTree(range_weight);            

            for (int i = 0; i < Vertices; i++)
                for (int j = 0; j < Vertices; j++)
                    if (adjacency[i, j] != null)
                        weight_matrix[i, j].Add((int)adjacency[i, j]);            
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

        public void BuildSpanningTree(int range_weight, bool oriented = false)
        {
            int min_weight = random_graph.Next(range_weight);
            int max_weight = min_weight + range_weight;
            List<int> tree = [];
            List<int> not_in_tree = [];

            for (int i = 1; i < Vertices; i++)
                not_in_tree.Add(i);
            tree.Add(0);

            while (not_in_tree.Count > 0)
                ChooseEdge(min_weight, max_weight, ref not_in_tree, ref tree);

            AddMoreEdges(min_weight / 3, max_weight / 3);
        }

        public void ChooseEdge(int min, int max, ref List<int> not_in_tree, ref List<int> tree, bool oriented = false)
        {
            int start, end, weight;

            start = random_graph.Next(Vertices);
            end = random_graph.Next(Vertices);
            weight = random_graph.Next(min, max);

            if (tree.Contains(start) && !tree.Contains(end))
            {
                AddEdge(start, end, weight, oriented);
                tree.Add(end);
                not_in_tree.Remove(start);
                not_in_tree.Remove(end);
                Edges++;
            }
        }

        public void AddMoreEdges(int min, int max, bool oriented = false)
        {
            int extra_edges = 2 * Vertices;
            for (int i = 0; i < extra_edges; i++)
            {
                int start = random_graph.Next(Vertices);
                int end = random_graph.Next(Vertices);
                int weight = random_graph.Next(min, max);
                if (adjacency[start, end] == null && start != end)
                {
                    AddEdge(start, end, weight, oriented);
                    Edges++;          
                }
            }             
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
            weight_matrix[start, end].Clear();
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

        public void PrintGraph(int output_width = 12)
        {
            Console.Write("".PadLeft(output_width));
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write($"{0}");
            for (int j = 1; j < Vertices; j++) Console.Write($"{j}".PadLeft(output_width));
            Console.ResetColor();
            Console.WriteLine();

            for (int i = 0; i < Vertices; i++)
            {
                for (int j = 0; j < Vertices; j++)
                {
                    if (j == 0)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write($"{i}");
                        Console.ResetColor();
                    }

                    if (adjacency[i, j] != null)
                    {
                        if (weight_matrix[i, j].Count == 2)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Write($"{adjacency[i, j]}".PadLeft(output_width));
                            Console.ResetColor();
                        }
                        else if (weight_matrix[i, j].Count == 1)
                            Console.Write($"{adjacency[i, j]}".PadLeft(output_width));
                    }                        
                    else 
                        Console.Write($"INF".PadLeft(output_width));
                }
                Console.WriteLine();
            }
        }

        public void PrintWeightMatrix(int output_width = 12)
        {
            Console.Write("".PadLeft(output_width));
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write($"{0}");
            for (int j = 1; j < Vertices; j++) Console.Write($"{j}".PadLeft(output_width));
            Console.ResetColor();
            Console.WriteLine();

            for (int i = 0; i < Vertices; i++)
            {
                for (int j = 0; j < Vertices; j++)
                {
                    if (j == 0)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write($"{i}");
                        Console.ResetColor();
                    }
                        
                    switch (weight_matrix[i, j].Count)
                    { 
                        case 0:
                            Console.Write($"INF".PadLeft(output_width));
                            break;

                        case 1:                            
                            Console.Write($"{weight_matrix[i, j][0]}".PadLeft(output_width));                            
                            break;

                        case 2:
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.Write($"{weight_matrix[i, j][0]}(→в.{weight_matrix[i, j][1]})".PadLeft(output_width));
                            Console.ResetColor();
                            break;

                        default:
                            throw new Exception("path overflow");
                    }                    
                }
                Console.WriteLine();
            }
        }
    }
}
