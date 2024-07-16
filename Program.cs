using Complexity_theory_of_computing_processors_and_structures;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.InteropServices;

internal class Program
{
    static int range_array = 20;                    //диапазон генерации (массив)
    static int range_graph_vertices = 10;           //диапазон генерации (граф)
    static int range_graph_weight = 10;              //диапазон генерации (граф, веса)
    public struct Union                             //структура для парных значений
    {
        public int first;
        public int second;
    }

    #region Select
    static void SelectSort(CustomArray array)
    {
        int comparision = 0, permutation = 0;

        for (int i = 0; i < array.Columns; i++)
        {
            int min = i;
            for (int j = i + 1; j < array.Columns; j++)
            {
                if (array[j] < array[min])
                    min = j;
                comparision++;
            }

            (array[min], array[i]) = (array[i], array[min]);
            permutation++;
        }

        Console.WriteLine("\u2193");
        array.PrintArray();

        Console.WriteLine($"Теоретическая трудоемкость алгоритма: 1/2*N^2 = {Math.Ceiling((double)Math.Pow(array.Columns, 2) / 2)}");
        Console.WriteLine($"Реальная трудоемкость алгоритма: {comparision + permutation}");
    }
    #endregion

    #region Bubble
    static void BubbleSort(CustomArray array)
    {
        int comparision = 0, permutation = 0;

        for (int i = 0; i < array.Columns; i++)
        {
            for (int j = 0; j < array.Columns - 1 - i; j++)
            {
                if (array[j] > array[j + 1])
                {
                    (array[j], array[j + 1]) = (array[j + 1], array[j]);
                    permutation++;
                }
                comparision++;
            }
        }

        Console.WriteLine("\u2193");
        array.PrintArray();

        Console.WriteLine($"Теоретическая трудоемкость алгоритма: N^2 = {Math.Pow(array.Columns, 2)}");
        Console.WriteLine($"Реальная трудоемкость алгоритма: {comparision + permutation}");
    }
    #endregion

    #region Merge
    static void MergeSort(CustomArray array)
    {
        int comparision = 0, permutation = 0;
        int delete = CheckN(ref array);
        Sort(array, 0, array.Columns - 1, ref comparision, ref permutation);

        Console.WriteLine("\u2193");
        array.PrintArray();
        if (delete != 0)
        {
            Console.WriteLine("Удаление лишних элементов:");
            array = CustomArray.ResizeArray(array, array.Columns - delete);
            array.PrintArray();
        }

        Console.WriteLine($"Теоретическая трудоемкость алгоритма: N*logN = {array.Columns * Math.Ceiling(Math.Log2(array.Columns))}");
        Console.WriteLine($"Реальная трудоемкость алгоритма: {comparision + permutation}");
    }
    static void Sort(CustomArray array, int left, int right, ref int comparision, ref int permutation)
    {

        if (left < right)
        {
            comparision++;

            int middle = left + (right - left) / 2;
            Sort(array, left, middle, ref comparision, ref permutation);
            Sort(array, middle + 1, right, ref comparision, ref permutation);
            Merge(array, left, middle, right, comparision, permutation);

            permutation++;
        }
    }
    static void Merge(CustomArray array, int left, int middle, int right, int comparision, int permutation)
    {
        int size1 = middle - left + 1;
        int size2 = right - middle;
        int i, j;
        List<int> tempLeft = [];
        List<int> tempRight = [];

        for (i = 0; i < size1; i++)
            tempLeft.Add(array[left + i]);
        for (j = 0; j < size2; j++)
            tempRight.Add(array[middle + 1 + j]);

        i = 0; j = 0;
        int k = left;

        while (i < size1 && j < size2)
        {
            if (tempLeft[i] <= tempRight[j])
            {
                array[k] = tempLeft[i];
                i++;
                permutation++;
            }
            else
            {
                array[k] = tempRight[j];
                j++;
                permutation++;
            }

            k++;
            comparision++;
        }

        while (i < size1)
        {
            array[k] = tempLeft[i];
            i++;
            k++;
            permutation++;
        }

        while (j < size2)
        {
            array[k] = tempRight[j];
            j++;
            k++;
            permutation++;
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
                int Max = FindMax(array);

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
    static int FindMax(CustomArray array)
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
        int[] backpack = new int[limit + 1];
        List<int>[] items_sets = new List<int>[limit + 1];

        backpack[0] = 0;
        items_sets[0] = [];

        for (int i = 1; i <= limit; i++)
        {
            int result = FindMaxCostAndFormItemsSet(array, backpack, i, ref items_sets);
            _ = result > 0 ? backpack[i] = result : backpack[i] = backpack[i - 1];
        }

        Console.WriteLine($"Максимальная стоимость набора: {backpack[limit]}");
        Console.WriteLine("Положены товары:");
        foreach (int item in items_sets[limit]) Console.WriteLine($"{array[0, item]}, {array[1, item]}");
    }

    static int FindMaxCostAndFormItemsSet(CustomArray array, int[] backpack, int current_weight, ref List<int>[] items_sets)
    {
        //f - функция максимальной стоимости набора при данном весе
        int items_cost = -1;
        Union last_used_data = new()
        {
            first = -1, //последний добавленный в рюкзак элемент
            second = -1 //индекс f, при котором был добавлен этот элемент
        };
        int f_index;

        for (int i = 0; i < array.Columns; i++)
        {
            f_index = current_weight - array[0, i];
            if (f_index >= 0)
            {
                if (items_cost < backpack[f_index] + array[1, i])
                {
                    items_cost = backpack[f_index] + array[1, i];
                    last_used_data.first = i;
                    last_used_data.second = f_index;
                }
            }
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
        graph.PrintGraph();
        Console.WriteLine("\nМатрица смежности после применения алгоритма Флойда-Уоршалла:");
        graph.PrintWeightMatrix();
    }
    #endregion

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
                array = new(2, 3, true);
                array.PrintArray();
                break;

            case 7:
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
        return graph;
    }

    private static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        while (true)
        {
            Console.Clear();
            Console.Write("1) SelectSort\n2) BubbleSort\n3) MergeSort\n4) Лестница\n5) Шахматная доска\n6) Рюкзак\n7) Флойд\n8) Дейкстра\n9) Форд-Беллман\n0) Выход\n\n6.1) Рюкзак тест 1\n6.2) Рюкзак тест 2\n\nВыбор пункта: ");
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
                    MergeSort(PrepareArray());
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

                case "7":
                    Console.WriteLine("Флойд");
                    Floyd(PrepareGraph());
                    break;

                case "0": return;

                default: break;
            }
            Console.ReadKey();
        }
    }
}


