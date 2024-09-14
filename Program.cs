using Complexity_theory_of_computing_processors_and_structures;
using System;

internal partial class Program
{
    static int range_array = 20;                    //диапазон генерации (массив)
    static int range_graph_vertices = 10;           //диапазон генерации (граф)
    static int range_graph_weight = 10;             //диапазон генерации (граф, веса)
    public struct Union                             //структура для парных значений
    {
        public int first;
        public int second;
    }

    #region Select
    static void SelectSort(CustomArray array)
    {
        for (int i = 0; i < array.Columns; i++)
        {
            int min = i;
            for (int j = i + 1; j < array.Columns; j++)
            {
                if (array[j] < array[min])
                    min = j;
            }

            (array[min], array[i]) = (array[i], array[min]);
        }

        Console.WriteLine("\u2193");
        array.PrintArray();
    }
    #endregion

    #region Bubble
    static void BubbleSort(CustomArray array)
    {
        bool swap = false;
        for (int i = 0; i < array.Columns; i++)
        {
            for (int j = 0; j < array.Columns - 1 - i; j++)
            {
                if (array[j] > array[j + 1])
                {
                    (array[j], array[j + 1]) = (array[j + 1], array[j]);
                    swap = true;
                }
            }
            if (swap)
                swap = false;
            else
                break;
        }

        Console.WriteLine("\u2193");
        array.PrintArray();
    }
    #endregion

    #region Merge
    static void MergeSortNR(CustomArray array)
    {
        CustomArray temp_array = new(array.Columns);
        int right, end;
        int i, j, temp_index, num = array.Columns;

        int delete = CheckN(ref array);
        Console.WriteLine("\u2193");
        Console.ForegroundColor = ConsoleColor.DarkGray;

        for (int k = 1; k < num; k *= 2)
        {
            Console.WriteLine($"k = {k}");
            for (int l = 0; l < array.Columns; l++)
            {
                Console.Write($"{array[l]} ");
                if ((l + 1) % k == 0) Console.Write(" | ");
            }
            Console.WriteLine();

            for (int left = 0; left + k < num; left += k * 2)
            {
                right = left + k;
                end = right + k;
                temp_index = left; 
                i = left; 
                j = right;

                if (end > num)
                    end = num;

                while (i < right && j < end)
                {
                    if (array[i] <= array[j])
                    {
                        temp_array[temp_index] = array[i]; 
                        i++;
                    }
                    else
                    {
                        temp_array[temp_index] = array[j]; 
                        j++;
                    }
                    temp_index++;
                }

                while (i < right)
                {
                    temp_array[temp_index] = array[i];
                    i++; 
                    temp_index++;
                }
                while (j < end)
                {
                    temp_array[temp_index] = array[j];
                    j++; 
                    temp_index++;
                }

                for (temp_index = left; temp_index < end; temp_index++)
                {
                    array[temp_index] = temp_array[temp_index];
                }
            }     
        }

        Console.ResetColor();
        Console.WriteLine("\u2193");
        array.PrintArray();
        if (delete != 0)
        {
            Console.WriteLine("Удаление лишних элементов:");
            array = CustomArray.ResizeArray(array, array.Columns - delete);
            array.PrintArray();
        }
    }    
    
    static int CheckN(ref CustomArray array)
    {
        for (int i = 1; ; i++)
        {
            if (array.Columns == Math.Pow(2, i))
            {
                Console.WriteLine($"Число элементов в массиве = {array.Columns} = 2^k, добавлять элементы не требуется");
                return 0;
            }
            else if (array.Columns < Math.Pow(2, i))
            {
                int to_add = (int)Math.Pow(2, i) - array.Columns;
                int prev_length = array.Columns;
                int Max = FindMax(ref array);

                Console.WriteLine($"Число элементов в массиве = {prev_length} != 2^k, необходимо добавить {to_add}");          
                array = CustomArray.ResizeArray(array, prev_length + to_add);
                for (int j = 1; j <= to_add; j++)
                    array[prev_length - 1 + j] = Max + j;
                Console.WriteLine("Результат:");
                array.PrintArray();

                return to_add;
            }
        }
    }

    static int FindMax(ref CustomArray array)
    {
        int max = array[0];
        for (int i = 0; i < array.Columns; i++)
        {
            if (array[i] > max)
                max = array[i];
        }
        return max;
    }
    #endregion

    #region Stairs
    static void Stairs(CustomArray array)
    {
        int N = array.Columns;
        int[] summa = new int[N];
        int[] path = new int[N];

        summa[0] = array[0];
        summa[1] = Math.Max(array[1] + summa[0], array[1]);
        for (int i = 2; i < N; i++)
            summa[i] = array[i] + Math.Max(summa[i - 1], summa[i - 2]);

        path[0] = N - 1;
        int path_index = 1;
        for (int i = N - 1; i - 2 >= 0;)
        {
            int step = Math.Max(summa[i - 1], summa[i - 2]);
            i = Array.LastIndexOf(summa, step, i - 1);
            path[path_index] = i;
            path_index++;
            if (i == 1 && summa[0] > 0)
            {
                path[path_index] = 0;
                path_index++;
            }
                
        }
        Array.Resize(ref path, path_index);
        Array.Reverse(path);

        Console.WriteLine("Путь: ");
        array.PrintPath(path);
        Console.WriteLine($"Сумма: {summa[N - 1]}");
    }
    #endregion

    #region ChessBoard
    static void ChessBoard(CustomArray array)
    {
        int M = array.Rows, N = array.Columns;
        int[,] summa = new int[M, N];
        Union[] path = new Union[M + N - 1];

        //сумма
        for (int i = 0; i < M; i++)
        {
            for (int j = 0; j < N; j++)
            {
                if (i > 0 && j > 0)
                    summa[i, j] = array[i, j] + Math.Max(summa[i, j - 1], summa[i - 1, j]);
                else if (i > 0 && j == 0)
                    summa[i, j] = array[i, j] + summa[i - 1, j];
                else if (i == 0 && j > 0)
                    summa[i, j] = array[i, j] + summa[i, j - 1];
                else
                    summa[0, 0] = array[0, 0];
            }
        }

        //путь
        path[0].first = M - 1;
        path[0].second = N - 1;
        int path_index = 1, x = M - 1, y = N - 1;
        while ((x != 0 || y != 0) && path_index < M + N - 2)
        {
            path[path_index].first = x;
            path[path_index].second = y;

            if (x > 0 && y > 0)
            {
                if (summa[x, y - 1] >= summa[x - 1, y])
                {
                    path[path_index].second -= 1;
                    y -= 1;
                }
                else
                {
                    path[path_index].first -= 1;
                    x -= 1;
                }
            }
            else if (x > 0 && y == 0)
            {
                path[path_index].first -= 1;
                x -= 1;
            }
            else if (x == 0 && y > 0)
            {
                path[path_index].second -= 1;
                y -= 1;
            }

            path_index++;
        }
        path[path_index].first = 0;
        path[path_index].second = 0;

        Array.Reverse(path);

        Console.WriteLine("Путь: ");
        array.PrintPath(path);
        Console.WriteLine($"Сумма: {summa[M - 1, N - 1]}");

    }
    #endregion

    #region Backpack
    static void Backpack(CustomArray array, int limit)
    {        
        int[] backpack = new int[limit + 1]; //стоимость для каждого веса
        List<int>[] items_sets = new List<int>[limit + 1]; //содержимое рюкзака

        backpack[0] = 0;
        items_sets[0] = [];

        for (int i = 1; i <= limit; i++)
        {
            Console.Write($"f({i})".PadRight(5));            
            Console.Write($" = max("); 
            Console.ForegroundColor = ConsoleColor.DarkGray;

            int result = FindMaxCostAndFormItemsSet(ref array, ref backpack, i, ref items_sets);
            //если вещь подходит, учитываем ее стоимость, если нет - стоимость не меняется
            _ = result > 0 ? backpack[i] = result : backpack[i] = backpack[i - 1];

            Console.ResetColor();
            Console.Write(")");            
            Console.WriteLine($" = {backpack[i]}");
        }

        Console.WriteLine($"Максимальная стоимость набора: {backpack[limit]}");
        Console.WriteLine("Положены товары:");
        foreach (int item in items_sets[limit]) Console.WriteLine($"{array[0, item]}, {array[1, item]}");
    }

    static int FindMaxCostAndFormItemsSet(ref CustomArray array, ref int[] backpack, int current_weight, ref List<int>[] items_sets)
    {
        //f - функция максимальной стоимости набора при данном весе
        int items_cost = -1;
        Union last_used_data = new()
        {
            first = -1, //последний добавленный в рюкзак элемент
            second = -1 //индекс f, при котором был добавлен этот элемент
        };
        int f_index; //индекс, для которого будет считаться f

        for (int i = 0; i < array.Columns; i++)
        {
            f_index = current_weight - array[0, i];
            Console.Write($"f({current_weight} - {array[0, i]}) + {array[1, i]}");

            if (f_index >= 0)
            {
                Console.Write(" = ");
                Console.ResetColor();
                Console.Write($"{backpack[f_index] + array[1, i]}");

                if (items_cost < backpack[f_index] + array[1, i]) /*1. кладем первую вещь*/
                {                                                 /*2. если для другой вещи условия лучше, меняем на другую*/ 
                    items_cost = backpack[f_index] + array[1, i];
                    last_used_data.first = i;
                    last_used_data.second = f_index;
                }
            }

            if (i < array.Columns - 1)
                Console.Write(", ");
            Console.ForegroundColor = ConsoleColor.DarkGray;
        }

        if (last_used_data.first >= 0)
        {
            items_sets[current_weight] = new List<int>(items_sets[last_used_data.second])
            {
                last_used_data.first
            };
        }
        else items_sets[current_weight] = [];

        return items_cost;
    }
    #endregion

    #region Floyd
    static void Floyd(CustomGraph graph)
    {
        int N = graph.Vertices;
        bool is_switched = false;

        for (int i = 0; i < graph.Vertices; i++)
        {
            if (graph.weight_matrix[i, i].Count != 0)
                graph.weight_matrix[i, i][0] = 0;
            else
                graph.weight_matrix[i, i].Add(0);
        }
        graph.PrintWeightMatrix();

        if (N > 1)
        {
            for (int k = 0; k < N; k++)
                for (int i = 0; i < N; i++)
                    for (int j = 0; j < N; j++)
                    {
                        if (graph.weight_matrix[i, k].Count != 0 && graph.weight_matrix[k, j].Count != 0 && graph.weight_matrix[i, j].Count != 0 && !(i == j && k == i))
                        {
                            if (graph.weight_matrix[i, j][0] > graph.weight_matrix[i, k][0] + graph.weight_matrix[k, j][0])
                            {
                                graph.weight_matrix[i, j][0] = graph.weight_matrix[i, k][0] + graph.weight_matrix[k, j][0];
                                is_switched = true;
                            }

                            if (is_switched)
                            {
                                switch (graph.weight_matrix[i, j].Count)
                                {
                                    case 1:
                                        graph.weight_matrix[i, j].Add(k);
                                        break;

                                    case 2:
                                        graph.weight_matrix[i, j][1] = k;
                                        break;

                                    default:
                                        throw new Exception("path overflow");
                                }
                            }
                            is_switched = false;
                        }
                    }
        }

        Console.WriteLine("Матрица смежности:");
        graph.PrintGraph(8);
        Console.WriteLine("\nМатрица смежности после применения алгоритма Флойда-Уоршелла:");
        graph.PrintWeightMatrix(8);

        //MakeTheWay(ref graph);
    }
    #endregion

    #region Path in graph (Floyd)
    //возможно, не нужно
    static void MakeTheWay(ref CustomGraph graph) 
    {
        int start = 0;
        int end = graph.Vertices - 1;

        List<int>[] prices = CountPricesForVertices(ref graph); 
        List<int> path = [];

        path.Add(end);
        for (int i = end; i > 0;)
        {
            path.Add(prices[i][1]);            
            i = prices[i][1];
        }
        path.Reverse();

        for (int i = 0; i < graph.Vertices; i++)
        {
            for (int j = 0; j < graph.Vertices; j++)
            {
                if (graph.weight_matrix[i, j].Count == 2)
                {
                    for (int k = 0; k < path.Count - 1; k++)
                    {
                        if (path[k] == i && path[k + 1] == j)
                        {
                            path.Insert(k + 1, graph.weight_matrix[i, j][1]);
                            break;
                        }                            
                    }
                }
            }
        }

        start = path[0];

        Console.WriteLine($"\nКратчайший путь от вершины {start} до вершины {end}: ");
        for (int i = 0; i < path.Count - 1; i++)
            Console.Write($"{path[i]} → ");
        Console.WriteLine($"{path.Last()}\n");

        graph.PrintWeightMatrix(9, path);

        Console.WriteLine($"\nМаксимальная стоимость пути: {prices.Last()[0]}");
    }

    static List<int>[] CountPricesForVertices(ref CustomGraph graph)
    {
        List<int>[] prices = new List<int>[graph.Vertices]; //0 - вес, 1 - предыдущая вершина
        bool has_edge = false;
        int max_weight = -int.MaxValue;

        for (int i = 0; i < graph.Vertices; i++)
        {
            for (int j = 0; j < graph.Vertices; j++)
            {
                if (graph.weight_matrix[i, j].Count != 0 && graph.weight_matrix[i, j][0] > max_weight)
                    max_weight = graph.weight_matrix[i, j][0];
            }
        }
        max_weight++;

        prices[0] = [ 0, 0 ];
        for (int j = 1; j < graph.Vertices; j++)
        {
            prices[j] =
            [
                max_weight*20, 
                -1,
            ];
        }
        
        for (int j = 0; j < graph.Vertices; j++)
        { 
            has_edge = false;
            for (int i = 0; i < graph.Vertices; i++)
            {
                if (!(i == 0 && j == 0) && (i < j)
                    && graph.weight_matrix[i, j].Count != 0 
                    && graph.weight_matrix[i, j][0] + prices[i][0] < prices[j][0])
                {
                    prices[j][0] = graph.weight_matrix[i, j][0] + prices[i][0];
                    prices[j][1] = i;
                    has_edge = true;
                }
            }   
            if (has_edge == false)
            {
                prices[j][0] = 0;
            }
        }

        return prices;
    }
    #endregion

    #region Dijkstra
    static void Dijkstra(CustomGraph graph, int v0)
    {
        List<int> S = [];
        List<int> V = [];
        int[] D = new int[graph.Vertices];
        List<int>[] paths = new List<int>[graph.Vertices];
        int chosen_vertex = v0;

        graph.PrintWeightMatrix();
        Console.WriteLine();

        for (int i = 0; i < graph.Vertices; i++)
        {
            if (i == v0) 
                continue;
            else
                V.Add(i);
        }
        S.Add(v0);
        D[v0] = 0;       
        for (int j = 0; j < graph.Vertices; j++)
        {
            if (j == v0)
                continue;
            else if (graph.weight_matrix[v0, j].Count != 0)
                D[j] = graph.weight_matrix[v0, j][0];
            else 
                D[j] = int.MaxValue;
        }

        #region print
        Console.Write("S".PadRight(graph.Vertices * 2));
        Console.Write("| w".PadRight(5));
        Console.Write("|D(w)".PadRight(5));
        for (int i = 0; i < graph.Vertices; i++)
        {
            if (i == v0)
                continue;
            else
                Console.Write($"| D({i})".PadRight(5));
        }
        Console.WriteLine();
        #endregion

        int iter = V.Count + 1;
        while (iter != 0)
        {
            #region print
            foreach (int s in S) Console.Write($"{s} ");
            Console.Write("".PadRight(graph.Vertices * 2 - S.Count * 2)); 
            if (chosen_vertex == v0)
            {
                Console.Write($"| -".PadRight(5));
                Console.Write($"| -".PadRight(5));
            }                
            else
            {
                Console.Write($"| {chosen_vertex}".PadRight(5));
                Console.Write($"| {D[chosen_vertex]}".PadRight(5));
            }         
            #endregion

            chosen_vertex = FindMin(D, S, V, v0);
            if (!S.Contains(chosen_vertex))
                S.Add(chosen_vertex);

            #region print
            for (int j = 0; j < graph.Vertices; j++)
            {
                if (j == v0)
                    continue;
                else if (!S.Contains(j))
                {
                    if (D[j] != int.MaxValue)
                        Console.Write($"| {D[j]}".PadRight(6));
                    else
                        Console.Write("| INF".PadRight(6));
                }                    

                else if (S.Last() == j && V.Count > 0)
                {
                    Console.Write("| ");
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write($"{D[j]}".PadRight(4));
                    Console.ResetColor();
                }
                else
                    Console.Write($"| -".PadRight(6));
            }
            Console.WriteLine();
            #endregion         

            for (int j = 0; j < graph.Vertices; j++)
            {
                if (graph.weight_matrix[v0, chosen_vertex].Count 
                    * graph.weight_matrix[chosen_vertex, j].Count != 0)
                {
                    if (D[j] > graph.weight_matrix[v0, chosen_vertex][0] + graph.weight_matrix[chosen_vertex, j][0])
                    {
                        D[j] = graph.weight_matrix[v0, chosen_vertex][0] + graph.weight_matrix[chosen_vertex, j][0];
                        //вес
                        if (graph.weight_matrix[v0, j].Count != 0)
                            graph.weight_matrix[v0, j][0] = D[j];
                        else
                            graph.weight_matrix[v0, j].Add(D[j]);

                        //путь
                        if (graph.weight_matrix[v0, j].Count == 1)
                            graph.weight_matrix[v0, j].Add(chosen_vertex);
                        else if (graph.weight_matrix[v0, j].Count == 2)
                            graph.weight_matrix[v0, j][1] = chosen_vertex;
                    }                    
                }

                if (!graph.Oriented)
                {
                    if (graph.weight_matrix[v0, chosen_vertex].Count
                        * graph.weight_matrix[chosen_vertex, j].Count != 0)
                    {
                        if (D[j] > graph.weight_matrix[j, chosen_vertex][0] + graph.weight_matrix[chosen_vertex, v0][0])
                        {
                            D[j] = graph.weight_matrix[j, chosen_vertex][0] + graph.weight_matrix[chosen_vertex, v0][0];
                            if (graph.weight_matrix[j, v0].Count != 0)
                                graph.weight_matrix[j, v0][0] = D[j];
                            else
                                graph.weight_matrix[j, v0].Add(D[j]);

                            if (graph.weight_matrix[v0, j].Count == 1)
                                graph.weight_matrix[v0, j].Add(chosen_vertex);
                            else if (graph.weight_matrix[v0, j].Count == 2)
                                graph.weight_matrix[v0, j][1] = chosen_vertex;
                        }                            
                    }
                }    
            }

            V.Remove(chosen_vertex);
            iter--;
        }
        Console.WriteLine();

        //пути        
        FindShortestPath(ref graph, v0, ref paths);
    }    

    static int FindMin(int[] D, List<int> S, List<int> V, int v0)
    {

        if (V.Count == 1)
            return V.Last();

        int min = 0;        
        int min_index = 0;
        
        //для установки начальных значений
        for (int i = 0; i < D.Length; i++)
        {
            if (i == v0)
                continue;
            else if (!S.Contains(i) && D[i] > 0)
            {
                min = D[i];
                min_index = i;
                break;
            }
        } 

        //поиск min
        for (int i = 0; i < D.Length; i++)
        {
            if (i == v0)
                continue;
            else if (!S.Contains(i) && D[i] < min && D[i] > 0)
            {
                min = D[i];
                min_index = i;
            }
        }

        return min_index;
    }

    static List<int> PathToAnotherVertices(ref CustomGraph graph, int i, int v0)
    {
        List<int> path = [];
        int current_child = i;

        path.Add(v0);
        if (i != v0)
            path.Add(i);

        while (HasParent(ref graph, ref current_child, i, v0))
        {
            path.Insert(1, current_child);
            i = current_child;
        }

        return path;
    }

    static bool HasParent(ref CustomGraph graph, ref int current_child, int child, int v0)
    {
        switch (graph.weight_matrix[v0, child].Count)
        {
            case 2:
                current_child = graph.weight_matrix[v0, child][1];
                return true;
            default:
                return false;
        }
    }
    #endregion

    #region Ford-Bellman
    static void FordBellman(CustomGraph graph, int v0)
    {
        //-1 == INF
        int[] D = new int[graph.Vertices];
        List<int>[] paths = new List<int>[graph.Vertices];
        int stabilized = 0;
        int n_iter = 0;

        for (int i = 0; i < D.Length; i++)
            D[i] = (i == v0) ? 0 : -1;

        for (int i = 0; i < graph.Vertices; i++)
        {
            if (graph.weight_matrix[i, i].Count != 0)
                graph.weight_matrix[i, i][0] = 0;
            else
                graph.weight_matrix[i, i].Add(0);
        }            
        graph.PrintWeightMatrix();

        PrintD(ref D);

        while (stabilized != graph.Vertices)
        {            
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write($"Итерация {n_iter++}:".PadRight(13));
            Console.ResetColor();
            Console.WriteLine();
           
            RecountD(ref D, ref graph, ref stabilized, v0);

            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write($"D({n_iter}) = ");
            PrintD(ref D);
            Console.ResetColor();
            Console.WriteLine();
        }

        graph.PrintWeightMatrix();

        //пути
        FindShortestPath(ref graph, v0, ref paths);
    }
    static void FindShortestPath(ref CustomGraph graph, int v0, ref List<int>[] paths)
    {
        for (int i = 0; i < graph.Vertices; i++)
        {
            paths[i] = [];
            if (i != v0)
            {
                paths[i] = PathToAnotherVertices(ref graph, i, v0);
                Console.WriteLine($"\nКратчайший путь от вершины {v0} до вершины {i}: ");
                for (int k = 0; k < paths[i].Count - 1; k++)
                    Console.Write($"{paths[i][k]} → ");
                Console.WriteLine($"{paths[i].Last()}");
            }
            else
                paths[i].Add(v0);
        }
    }

    static void RecountD(ref int[] D, ref CustomGraph graph, ref int stabilized, int v0)
    {
        int[] prev_D = new int[D.Length];
        D.CopyTo(prev_D, 0);

        for (int i = 0; i < D.Length; i++)
        {
            if (i != v0)
            {
                Console.Write($"D[{i}] = min(");
                D[i] = FindMin(ref prev_D, ref graph, i, v0);
                Console.WriteLine((D[i] == -1) ? ") = INF" : $") = {D[i]}");
            }
            else
                D[i] = 0;
        }

        stabilized = 0;
        for (int i = 0; i < D.Length; i++)
            if (D[i] == prev_D[i])
                stabilized++;
    }

    static int FindMin(ref int[] D, ref CustomGraph graph, int index, int v0)
    {
        nint min = int.MaxValue;
        int temp;

        for (int i = 0; i < D.Length; i++)
        {
            if (graph.adjacency[i, index] != null)
            {
                if ((temp = D[i] + graph.weight_matrix[i, index][0]) < min && D[i] != -1)
                {
                    min = temp;

                    //путь 
                    if (i != v0 && i != index)
                    {
                        switch (graph.weight_matrix[v0, index].Count)
                        {
                            case 0:
                                graph.weight_matrix[v0, index].Add((int)min);
                                graph.weight_matrix[v0, index].Add(i);
                                if (!graph.Oriented)
                                {
                                    graph.weight_matrix[index, v0].Add((int)min);
                                    graph.weight_matrix[index, v0].Add(i);
                                }
                                break;

                            case 1:
                                if (graph.weight_matrix[v0, index][0] > min)
                                {
                                    graph.weight_matrix[v0, index][0] = (int)min;
                                    graph.weight_matrix[v0, index].Add(i);
                                    if (!graph.Oriented)
                                    {
                                        graph.weight_matrix[index, v0][0] = (int)min;
                                        graph.weight_matrix[index, v0].Add(i);
                                    }
                                }
                                break;

                            case 2:
                                if (graph.weight_matrix[v0, index][0] > min)
                                {
                                    graph.weight_matrix[v0, index][0] = (int)min;
                                    graph.weight_matrix[v0, index][1] = i;
                                    if (!graph.Oriented)
                                    {
                                        graph.weight_matrix[index, v0][0] = (int)min;
                                        graph.weight_matrix[index, v0][1] = i;
                                    }
                                }
                                break;

                            default:
                                throw new Exception("Лишние значения в weight_matrix");
                        }
                    }
                }

                Console.Write((D[i] == -1) ? "INF" : $"{D[i]}");
                Console.Write($" + {graph.weight_matrix[i, index][0]}");
            }
            else
                Console.Write((D[i] == -1) ? "INF + INF" : $"{D[i]} + INF");

            if (i != D.Length - 1)
                Console.Write(", ");
        }

        return min == int.MaxValue ? -1 : (int)min;
    }

    static void PrintD(ref int[] D)
    {
        Console.Write('(');
        foreach (int d in D)
        {
            switch (d)
            {
                case -1:
                    Console.Write("INF".PadRight(4));
                    break;

                default:
                    Console.Write((D.Last() == d) ? $"{d}" : $"{d}".PadRight(4));
                    break;
            }
        }
        Console.WriteLine(')');
    }
    #endregion

    #region Prepare Data
    static CustomArray PrepareArray(bool is_positive = false, bool is_two_dimensional = false)
    {
        CustomArray array;
        int M, N, max, min;

        Console.Write("Введите N (количество столбцов): ");
        N = Convert.ToInt16(Console.ReadLine());

        _ = is_positive ? min = 1 : min = -10;
        max = min + range_array;

        if (is_two_dimensional)
        {
            Console.Write("Введите M (количество строк): ");
            M = Convert.ToInt16(Console.ReadLine());
            array = new(M, N, min, max);
        }            
        else
            array = new(N, min, max);

        array.PrintArray();

        return array;
    }
    
    static CustomArray PrepareTestArray(int choice) //примеры из методички
    {
        CustomArray array = new();

        switch (choice)
        {
            case 6:
                array = new(2, 3, true, 6);
                array.PrintArray();
                break;

            case 7:
                array = new(2, 3, true, 7);
                array.PrintArray();
                break;

            default:
                Console.WriteLine("Для данного выбора нет тестовой задачи");
                break;
        }

        return array;
    }

    static CustomGraph PrepareGraph()
    {
        CustomGraph graph = new(range_graph_vertices, range_graph_weight);
        //graph.PrintGraph();
        Console.WriteLine();
        return graph;
    }

    static CustomGraph PrepareTestGraph(int test)
    {
        CustomGraph graph = new(test);
        //graph.PrintGraph();
        Console.WriteLine();
        return graph;
    }
    #endregion

    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        while (true)
        {
            Console.Clear();
            Console.Write("1) SelectSort\n2) BubbleSort\n3) MergeSort\n4) Лестница\n5) Шахматная доска\n6) Рюкзак\n0) Выход\n\n6.1) Рюкзак тест 1\n6.2) Рюкзак тест 2\n6.3) Рюкзак тест 3\n7.1) Флойд тест 1\n7.2) Флойд тест 2\n8.1) Дейкстра тест 1\n8.2) Дейкстра тест 2\n9.1) Форд-Беллман тест 1\n9.2) Форд-Беллман тест 2\n\nВыбор пункта: ");
            string? choice = Console.ReadLine();
            Console.WriteLine();

            switch (choice)
            {
                case "1":
                    Console.WriteLine("SelectSort");                    
                    SelectSort(PrepareArray());
                    break;

                case "2":
                    Console.WriteLine("BubbleSort");
                    BubbleSort(PrepareArray());
                    break;

                case "3":
                    Console.WriteLine("MergeSort");
                    MergeSortNR(PrepareArray());
                    break;

                case "4":
                    Console.WriteLine("Лестница");
                    Stairs(PrepareArray());
                    break;

                case "5":
                    Console.WriteLine("Шахматная доска");
                    ChessBoard(PrepareArray(false, true));
                    break;

                case "6":
                    Console.WriteLine("Рюкзак");
                    Console.Write("Введите максимальный вес для рюкзака: ");
                    int limit = Convert.ToInt16(Console.ReadLine());
                    Backpack(PrepareArray(true, true), limit);
                    break;

                case "6.1":
                    Console.WriteLine("Рюкзак");
                    Backpack(PrepareTestArray(6), 23);
                    break;

                case "6.2":
                    Console.WriteLine("Рюкзак");
                    Backpack(PrepareTestArray(6), 24);
                    break;

                case "6.3":
                    Console.WriteLine("Рюкзак");
                    Backpack(PrepareTestArray(7), 20);
                    break;

                case "7.1":
                    Console.WriteLine("Флойд");
                    Floyd(PrepareTestGraph(1));
                    break;

                case "7.2":
                    Console.WriteLine("Флойд");
                    Floyd(PrepareTestGraph(2));
                    break;

                case "8.1":
                    Console.WriteLine("Дейкстра");
                    Dijkstra(PrepareTestGraph(3), 0);
                    break;

                case "8.2":
                    Console.WriteLine("Дейкстра");
                    Dijkstra(PrepareTestGraph(4), 2);
                    break;

                case "9.1":
                    Console.WriteLine("Форд-Беллман");
                    FordBellman(PrepareTestGraph(3), 0);
                    break;

                case "9.2":
                    Console.WriteLine("Форд-Беллман");
                    FordBellman(PrepareTestGraph(4), 2);
                    break;

                case "0": return;

                default: break;
            }
            Console.ReadKey();
        }
    }
}


