using Complexity_theory_of_computing_processors_and_structures;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.InteropServices;

internal class Program
{
    static int N_to_delete = 0, M_to_delete = 1;    //размеры матриц (старое, временно здесь чтобы не было ошибок)
    static int[,] array_2d = new int[0, 0];         //массивы        (старое, временно здесь чтобы не было ошибок)
    static int[] array_1d = [];
    static int range = 20;                          //диапазон генерации
    static int max_weight = 0;                      //для рюкзака: максимальный вес

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
        array.PrintCustomArray();

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
        array.PrintCustomArray();

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
        array.PrintCustomArray();
        if (delete != 0)
        {
            Console.WriteLine("Удаление лишних элементов:");
            array = CustomArray.ResizeCustomArray(array, array.Columns - delete);
            array.PrintCustomArray();
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
                Console.WriteLine($"Число элементов в массиве = {array_1d.Length} = 2^k, добавлять элементы не требуется");
                return 0;
            }
            else if (array.Columns < Math.Pow(2, i))
            {
                int to_add = (int)Math.Pow(2, i) - array.Columns;
                int prev_length = array.Columns;
                int Max = FindMax(array);

                Console.WriteLine($"Число элементов в массиве = {prev_length} != 2^k, необходимо добавить {to_add}");          
                array = CustomArray.ResizeCustomArray(array, prev_length + to_add);
                for (int j = 1; j <= to_add; j++)
                    array[prev_length - 1 + j] = Max + j;
                Console.WriteLine("Результат:");
                array.PrintCustomArray();

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
    static void Stairs()
    {
        int[] summa = new int[N_to_delete];
        int[] path = new int[N_to_delete];

        summa[0] = array_1d[0];
        summa[1] = Math.Max(array_1d[1] + summa[0], array_1d[1]);
        for (int i = 2; i < N_to_delete; i++)
            summa[i] = array_1d[i] + Math.Max(summa[i - 1], summa[i - 2]);

        path[0] = N_to_delete - 1;
        int path_index = 1;
        for (int i = N_to_delete - 1; i - 2 >= 0;)
        {
            int step = Math.Max(summa[i - 1], summa[i - 2]);
            i = Array.LastIndexOf(summa, step, i - 1);
            path[path_index] = i;
            path_index++;
            if (i == 1 && summa[0] > 0)
                path[path_index] = 0;
        }
        Array.Resize(ref path, path_index);
        Array.Reverse(path);

        Console.WriteLine("Путь: ");
        //PrintPath_1DArray(path);
        Console.WriteLine($"Сумма: {summa[N_to_delete - 1]}");
    }
    #endregion

    #region ChessBoard
    static void ChessBoard()
    {
        int[,] summa = new int[M_to_delete, N_to_delete];
        Union[] path = new Union[M_to_delete + N_to_delete - 1];

        for (int i = 0; i < M_to_delete; i++)
        {
            for (int j = 0; j < N_to_delete; j++)
            {
                if (i > 0 && j > 0)
                    summa[i, j] = array_2d[i, j] + Math.Max(summa[i, j - 1], summa[i - 1, j]);
                else if (i > 0 && j == 0)
                    summa[i, j] = array_2d[i, j] + summa[i - 1, j];
                else if (i == 0 && j > 0)
                    summa[i, j] = array_2d[i, j] + summa[i, j - 1];
                else
                    summa[0, 0] = array_2d[0, 0];
            }
        }

        path[0].first = M_to_delete - 1;
        path[0].second = N_to_delete - 1;
        int path_index = 1, x = M_to_delete - 1, y = N_to_delete - 1;
        while ((x != 0 || y != 0) && path_index < M_to_delete + N_to_delete - 2)
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
        //PrintPath_2DArray(path);
        Console.WriteLine($"Сумма: {summa[M_to_delete - 1, N_to_delete - 1]}");

    }
    #endregion

    #region Backpack
    static void Backpack()
    {
        //f - функция максимальной стоимости набора при данном весе
        int[] backpack = new int[max_weight + 1];
        List<int>[] items_sets = new List<int>[max_weight + 1];

        backpack[0] = 0;
        items_sets[0] = [];

        for (int i = 1; i <= max_weight; i++)
        {
            int result = FindMaxCostAndFormItemsSet(backpack, i, ref items_sets);
            _ = result > 0 ? backpack[i] = result : backpack[i] = backpack[i - 1];
        }

        Console.WriteLine($"Максимальная стоимость набора: {backpack[max_weight]}");
        Console.WriteLine("Положены товары:");
        foreach (int item in items_sets[max_weight]) Console.WriteLine($"{array_2d[0, item]}, {array_2d[1, item]}");
    }

    static int FindMaxCostAndFormItemsSet(int[] backpack, int current_weight, ref List<int>[] items_sets)
    {
        int items_cost = -1;
        Union last_used_data = new()
        {
            first = -1, //последний добавленный в рюкзак элемент
            second = -1 //индекс f, при котором был добавлен этот элемент
        };
        int f_index;

        for (int i = 0; i < N_to_delete; i++)
        {
            f_index = current_weight - array_2d[0, i];
            if (f_index >= 0)
            {
                if (items_cost < backpack[f_index] + array_2d[1, i])
                {
                    items_cost = backpack[f_index] + array_2d[1, i];
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
    void Floyd()
    {

    }
    #endregion

    #region PrintPath
    void PrintPath_1DArray(int[] path)
    {
        for (int i = 0, k = 0; i < array_1d.Length; i++)
        {
            if (i == path[k])
            {
                Console.ForegroundColor = ConsoleColor.Green;
                k++;
            }
            else
                Console.ForegroundColor = ConsoleColor.Red;
            Console.Write($"{array_1d[i]}".PadRight(5));
        }
        Console.ResetColor();
        Console.WriteLine();
    }

    void PrintPath_2DArray(Union[] path)
    {
        //Union = x, y
        int k = 0;
        for (int i = 0; i < M_to_delete && k < M_to_delete + N_to_delete - 1; i++)
        {
            for (int j = 0; j < N_to_delete && k < M_to_delete + N_to_delete - 1; j++)
            {
                if (i == path[k].first && j == path[k].second)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    k++;
                }
                else
                    Console.ForegroundColor = ConsoleColor.Red;
                Console.Write($"{array_2d[i, j]}".PadRight(5));
            }
            Console.WriteLine();
        }
        Console.ResetColor();
        Console.WriteLine();
    }
    #endregion

    static CustomArray PrepareArray(bool positive, int M = 1)
    {
        CustomArray array;
        int N, max, min;

        Console.Write("Введите N (количество столбцов): ");
        N = Convert.ToInt16(Console.ReadLine());

        _ = positive ? min = -10 : min = 1;
        max = min + range;

        if (M > 1) 
            array = new(M, N, min, max);
        else
            array = new(N, min, max);

        array.PrintCustomArray();

        return array;
    }

    private static void Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        while (true)
        {
            Console.Clear();
            Console.Write("1) SelectSort\n2) BubbleSort\n3) MergeSort\n4) Лестница\n5) Шахматная доска\n6) Рюкзак\n7) Флойд\n8) Дейкстра\n9) Форд-Беллман\n0) Выход\n\nВыбор пункта: ");
            string choice = Console.ReadLine();
            Console.WriteLine();

            max_weight = 0;

            switch (choice)
            {
                case "1":
                    Console.WriteLine("SelectSort");                    
                    SelectSort(PrepareArray(false));
                    break;

                case "2":
                    Console.WriteLine("BubbleSort");
                    BubbleSort(PrepareArray(false));
                    break;

                case "3":
                    Console.WriteLine("MergeSort");
                    MergeSort(PrepareArray(false));
                    break;

                case "4":
                    Console.WriteLine("Лестница");
                    PrepareData(4);
                    Stairs();
                    break;

                case "5":
                    Console.WriteLine("Шахматная доска");
                    PrepareData(5);
                    ChessBoard();
                    break;

                case "6":
                    Console.WriteLine("Рюкзак");
                    positive = true;
                    PrepareData(6);
                    Backpack();
                    break;

                case "7":
                    Console.WriteLine("Флойд");
                    PrepareData(7);
                    Backpack();
                    break;

                case "0": return;

                default: break;
            }
            Console.ReadKey();
        }

        void PrepareData(int choice)
        {
            //N = 0; M = 1; max_weight = 0;
            //Console.Write("Введите N (количество столбцов): ");
            //N = Convert.ToInt16(Console.ReadLine());

            //Random random = new();
            //int right_border, left_border;
            //switch (positive)
            //{
            //    case false:
            //        right_border = -10;
            //        break;

            //    case true:
            //        right_border = 1;
            //        break;
            //}
            //left_border = right_border + range;

            //switch (choice)
            //{
            //    case 1:
            //    case 2:
            //    case 3:
            //    case 4:
            //        CustomArray array = new(N, right_border, left_border);
            //        array.PrintCustomArray(output_width);
            //        break;

            //    case 5:
            //    case 6:
            //        if (choice == 6)
            //        {
            //            M = 2;
            //            Console.Write("Максимальный вес: ");
            //            max_weight = Convert.ToInt16(Console.ReadLine());
            //            //пример из методички
            //            //max_weight = 13;
            //            //array_2d = new int[2, 3];
            //            //array_2d[0, 0] = 3;
            //            //array_2d[0, 1] = 5;
            //            //array_2d[0, 2] = 8;
            //            //array_2d[1, 0] = 8;
            //            //array_2d[1, 1] = 14;
            //            //array_2d[1, 2] = 23;
            //            //break;
            //        }
            //        else
            //        {
            //            Console.Write("Введите M (количество строк): ");
            //            M = Convert.ToInt16(Console.ReadLine());
            //        }
            //        Array.Clear(array_2d);
            //        array_2d = new int[M, N];
            //        for (int i = 0; i < M; i++)
            //        {
            //            for (int j = 0; j < N; j++)
            //                array_2d[i, j] = random.Next(right_border, left_border);
            //        }
            //        //Print2DArray();
            //        break;
            //}
        }
    }
}

//структура для парных значений
struct Union
{
    public int first;
    public int second;
}
