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

        #region Generation of Graph
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
        #endregion

        //тест
        public CustomGraph(bool test1, bool test2)
        {
            if (test1 == true)
            {
                Vertices = 17;
                Edges = 27;
                adjacency = new int?[Vertices, Vertices];
                weight_matrix = new List<int>[Vertices, Vertices];
                for (int i = 0; i < Vertices; i++)
                    for (int j = 0; j < Vertices; j++)
                        weight_matrix[i, j] = [];

                adjacency[0, 1] = 4;
                adjacency[0, 2] = 5;
                adjacency[0, 3] = 6;
                adjacency[1, 4] = 6;
                adjacency[1, 5] = 8;
                adjacency[1, 6] = 7;
                adjacency[2, 6] = 8;
                adjacency[2, 7] = 9;
                adjacency[3, 6] = 6;
                adjacency[3, 7] = 7;
                adjacency[4, 8] = 4;
                adjacency[5, 8] = 5;
                adjacency[5, 9] = 5;
                adjacency[6, 9] = 6;
                adjacency[6, 10] = 5;
                adjacency[6, 11] = 6;
                adjacency[7, 10] = 4;
                adjacency[7, 11] = 5;
                adjacency[8, 12] = 7;
                adjacency[9, 12] = 6;
                adjacency[9, 13] = 8;
                adjacency[10, 13] = 7;
                adjacency[10, 14] = 6;
                adjacency[10, 16] = 15;
                adjacency[11, 14] = 7;
                adjacency[12, 15] = 9;
                adjacency[13, 15] = 8;
                adjacency[14, 16] = 7;

                for (int i = 0; i < Vertices; i++)
                    for (int j = 0; j < Vertices; j++)
                        if (adjacency[i, j] != null)
                            weight_matrix[i, j].Add((int)adjacency[i, j]);
            }
            else if (test2 == true)
            {
                Vertices = 10;
                Edges = 17;
                adjacency = new int?[Vertices, Vertices];
                weight_matrix = new List<int>[Vertices, Vertices];
                for (int i = 0; i < Vertices; i++)
                    for (int j = 0; j < Vertices; j++)
                        weight_matrix[i, j] = [];

                //adjacency[0, 1] = 2;
                //adjacency[0, 2] = 3;
                //adjacency[0, 3] = 1;
                //adjacency[1, 4] = 1;
                //adjacency[1, 5] = 3;
                //adjacency[2, 4] = 2;
                //adjacency[2, 5] = 2;
                //adjacency[2, 6] = 2;
                //adjacency[3, 5] = 3;
                //adjacency[3, 6] = 4;
                //adjacency[4, 7] = 1;
                //adjacency[5, 7] = 1;
                //adjacency[5, 8] = 1;
                //adjacency[6, 8] = 2;
                //adjacency[7, 8] = 1;

                adjacency[0, 1] = 2;
                adjacency[0, 2] = 3;
                adjacency[0, 3] = 1;
                adjacency[1, 4] = 1;
                adjacency[1, 5] = 3;
                adjacency[2, 4] = 2;
                adjacency[2, 5] = 2;
                adjacency[2, 6] = 2;
                adjacency[3, 5] = 3;
                adjacency[3, 6] = 4;
                adjacency[4, 8] = 5;
                adjacency[5, 7] = 1;
                adjacency[5, 8] = 1;
                adjacency[5, 9] = 3;
                adjacency[6, 9] = 2;
                adjacency[7, 9] = 1;
                adjacency[8, 9] = 1;

                for (int i = 0; i < Vertices; i++)
                    for (int j = 0; j < Vertices; j++)
                        if (adjacency[i, j] != null)
                            weight_matrix[i, j].Add((int)adjacency[i, j]);
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

        public void PrintGraph(int output_width = 12, List<int> path = null)
        {
            Console.Write("".PadLeft(output_width));
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write($"{0}".PadLeft(output_width));
            for (int j = 1; j < Vertices; j++) Console.Write($"{j}".PadLeft(output_width));
            Console.ResetColor();
            Console.WriteLine();

            int path_index = 0;

            for (int i = 0; i < Vertices; i++)
            {
                for (int j = 0; j < Vertices; j++)
                {
                    if (j == 0)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write($"{i}".PadLeft(output_width));
                        Console.ResetColor();
                    }

                    if (adjacency[i, j] != null)
                    {
                        if (weight_matrix[i, j].Count == 2)
                        {
                            if (path == null)
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.Write($"{adjacency[i, j]}".PadLeft(output_width));
                                Console.ResetColor();
                            }
                            else
                                Console.Write($"{adjacency[i, j]}".PadLeft(output_width));
                        }
                        else if (weight_matrix[i, j].Count == 1)
                        {
                            if (path != null && path_index < path.Count - 1)
                            {
                                if (i == path[path_index] && j == path[path_index + 1])
                                {
                                    Console.ForegroundColor = ConsoleColor.Magenta;
                                    Console.Write($"{adjacency[i, j]}".PadLeft(output_width));
                                    Console.ResetColor();
                                    path_index++;
                                }
                                else
                                    Console.Write($"{adjacency[i, j]}".PadLeft(output_width));
                            }
                            else
                                Console.Write($"{adjacency[i, j]}".PadLeft(output_width));
                        }
                    }                        
                    else 
                        Console.Write($"INF".PadLeft(output_width));
                }
                Console.WriteLine();
            }
        }

        public void PrintWeightMatrix(int output_width = 12, List<int> path = null)
        {
            Console.Write("".PadLeft(output_width));
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write($"{0}".PadLeft(output_width));
            for (int j = 1; j < Vertices; j++) Console.Write($"{j}".PadLeft(output_width));
            Console.ResetColor();
            Console.WriteLine();

            int path_index = 0;

            for (int i = 0; i < Vertices; i++)
            {
                for (int j = 0; j < Vertices; j++)
                {
                    if (j == 0)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write($"{i}".PadLeft(output_width));
                        Console.ResetColor();
                    }
                        
                    switch (weight_matrix[i, j].Count)
                    { 
                        case 0:
                            Console.Write($"INF".PadLeft(output_width));
                            break;

                        case 1:          
                            if (path != null && path_index < path.Count - 1)
                            {
                                if (i == path[path_index] && j == path[path_index + 1])
                                {
                                    Console.ForegroundColor = ConsoleColor.Magenta;
                                    Console.Write($"{weight_matrix[i, j][0]}".PadLeft(output_width));
                                    Console.ResetColor();
                                    path_index++;
                                }
                                else
                                    Console.Write($"{weight_matrix[i, j][0]}".PadLeft(output_width));
                            }
                            else
                                Console.Write($"{weight_matrix[i, j][0]}".PadLeft(output_width));
                            break;

                        case 2:
                            if (path != null && path_index < path.Count - 1)
                            {
                                if (i == path[path_index] && j == path[path_index + 1])
                                {
                                    Console.ForegroundColor = ConsoleColor.Magenta;
                                    Console.Write($"{weight_matrix[i, j][0]}(→в.{weight_matrix[i, j][1]})".PadLeft(output_width));
                                    Console.ResetColor();
                                    path_index++;
                                }
                                else
                                    Console.Write($"{weight_matrix[i, j][0]}".PadLeft(output_width));
                            }
                            else
                            {
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.Write($"{weight_matrix[i, j][0]}(→в.{weight_matrix[i, j][1]})".PadLeft(output_width));
                                Console.ResetColor();
                            }                                
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
