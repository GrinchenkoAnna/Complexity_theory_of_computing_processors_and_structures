using System.IO;
using System.Runtime.InteropServices;

int N = 0, M = 1;
const int output_width = 5;

#region Select
void SelectSort(int[] array)
{
    int comparision = 0, permutation = 0;

    for (int i = 0; i < array.Length; i++)
    {
        int min = i;
        for (int j = i + 1; j < array.Length; j++)
        {
            if (array[j] < array[min]) min = j;
            comparision++;
        }
        (array[i], array[min]) = (array[min], array[i]);
        permutation++;
    }

    Console.WriteLine("\u2193");
    Print1DArray(array);

    Console.WriteLine($"Теоретическая трудоемкость алгоритма: 1/2*N^2 = {Math.Ceiling((double)(Math.Pow(array.Length, 2)) / 2)}");
    Console.WriteLine($"Реальная трудоемкость алгоритма: {comparision + permutation}");
}
#endregion

#region Bubble
void BubbleSort(int[] array)
{
    int comparision = 0, permutation = 0;

    for (int i = 0; i < array.Length; i++)
    {
        for (int j = 0; j < array.Length - 1 - i; j++)
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
    Print1DArray(array);

    Console.WriteLine($"Теоретическая трудоемкость алгоритма: N^2 = {Math.Pow(array.Length, 2)}");
    Console.WriteLine($"Реальная трудоемкость алгоритма: {comparision + permutation}");
}
#endregion

#region Merge
void MergeSort(int[] array)
{
    int comparision = 0, permutation = 0;
    int delete = CheckN(ref array);
    Console.WriteLine($"delete = {delete}");
    Sort(array, 0, array.Length - 1, ref comparision, ref permutation);

    Console.WriteLine("\u2193");
    Print1DArray(array);
    if (delete != 0)
    {
        Console.WriteLine("Удаление лишних элементов:");
        Array.Resize<int>(ref array, array.Length - delete);
        Print1DArray(array);
    }

    Console.WriteLine($"Теоретическая трудоемкость алгоритма: N*logN = {array.Length * Math.Ceiling(Math.Log2(array.Length))}");
    Console.WriteLine($"Реальная трудоемкость алгоритма: {comparision + permutation}");
}
void Sort(int[] array, int left, int right, ref int comparision, ref int permutation)
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
void Merge(int[] array, int left, int middle, int right, int comparision, int permutation)
{
    int size1 = middle - left + 1;
    int size2 = right - middle;
    int i = 0, j = 0;

    List<int> tempLeft = [];
    List<int> tempRight = [];

    for (i = 0; i < size1; i++) tempLeft.Add(array[left + i]);
    for (j = 0; j < size2; j++) tempRight.Add(array[middle + 1 + j]);

    i = 0; j = 0;
    int k = left;

    while (i < size1 && j < size2)
    {
        if (tempLeft[i] <= tempRight[j]) { array[k] = tempLeft[i]; i++; permutation++; }
        else { array[k] = tempRight[j]; j++; permutation++; }
        k++; comparision++;
    }
    while (i < size1) { array[k] = tempLeft[i]; i++; k++; permutation++; }
    while (j < size2) { array[k] = tempRight[j]; j++; k++; permutation++; }
}
int CheckN(ref int[] array)
{
    for (int i = 1; ; i++)
    {
        if (array.Length == Math.Pow(2, i))
        {
            Console.WriteLine($"Число элементов в массиве = {array.Length} = 2^k, добавлять элементы не требуется");
            return 0;
        }
        else if (array.Length < Math.Pow(2, i))
        {
            int to_add = (int)Math.Pow(2, i) - array.Length;
            int prev_length = array.Length;
            Console.WriteLine($"Число элементов в массиве = {array.Length} != 2^k, необходимо добавить {to_add}");
            int M = FindMax(array);
            Array.Resize<int>(ref array, array.Length + to_add);
            for (int j = 1; j <= to_add; j++) array[prev_length - 1 + j] = M + j; 
            Console.WriteLine("Результат:");
            Print1DArray(array);

            return to_add;
        }
    }
}
int FindMax(int[] array)
{
    int max = array[0];
    for (int i = 0; i < array.Length; i++)
    {
        if (array[i] > max) max = array[i];
    }
    return max;
}
#endregion

#region Stairs
void Stairs(int[] array)
{
    int[] summa = new int[N];
    int[] path = new int[N];

    summa[0] = array[0];
    summa[1] = Math.Max(array[1] + summa[0], array[1]);
    for (int i = 2; i < N; i++) summa[i] = array[i] + Math.Max(summa[i - 1], summa[i - 2]);

    path[0] = N - 1;
    int path_index = 1;
    for (int i = N - 1; i - 2 >= 0;)
    {
        int step = Math.Max(summa[i - 1], summa[i - 2]);
        i = Array.LastIndexOf(summa, step, i - 1);
        path[path_index] = i;
        path_index++;
        if (i == 1 && summa[0] > 0) path[path_index] = 0;            
    }
    Array.Resize(ref path, path_index);
    Array.Reverse(path);

    Console.WriteLine("Путь: ");
    PrintPath_1DArray(array, path);
    Console.WriteLine($"Сумма: {summa[N - 1]}");
}
#endregion

#region ChessBoard
void ChessBoard(int[,] array)
{
    int[,] summa = new int[M, N]; 
    Path[] path = new Path[M + N - 1];
    
    for (int i = 0; i < M; i++)
    {
        for (int j = 0; j < N; j++)
        {
            if (i > 0 && j > 0) summa[i, j] = array[i, j] + Math.Max(summa[i, j - 1], summa[i - 1, j]);
            else if (i > 0 && j == 0) summa[i, j] = array[i, j] + summa[i - 1, j];
            else if (i == 0 && j > 0) summa[i, j] = array[i, j] + summa[i, j - 1];
            else summa[0, 0] = array[0, 0];
        }
    }

    path[0].path_x = M - 1;
    path[0].path_y = N - 1;
    int path_index = 1, x = M - 1, y = N - 1;
    while ((x != 0 || y != 0) && path_index < M + N - 2)
    {
        path[path_index].path_x = x;
        path[path_index].path_y = y;

        if (x > 0 && y > 0)
        {
            if (summa[x, y - 1] >= summa[x - 1, y]) { path[path_index].path_y -= 1; y -= 1; }
            else { path[path_index].path_x -= 1; x -= 1; }
        }
        else if (x > 0 && y == 0) { path[path_index].path_x -= 1; x -= 1; }
        else if (x == 0 && y > 0) { path[path_index].path_y -= 1; y -= 1; }

        path_index++;
    }
    path[path_index].path_x = 0;
    path[path_index].path_y = 0;

    Array.Reverse(path);

    Console.WriteLine("Путь: ");
    PrintPath_2DArray(array, path);
    Console.WriteLine($"Сумма: {summa[M - 1, N - 1]}");

}
#endregion
void Print1DArray(int[] array)
{
    Console.ForegroundColor = ConsoleColor.Red;
    for (int i = 0; i < array.Length; i++) Console.Write($"{array[i]}".PadRight(output_width));
    Console.ResetColor();
    Console.WriteLine();
}

void Print2DArray(int[,] array)
{
    Console.ForegroundColor = ConsoleColor.Red;
    for (int i = 0; i < M; i++)
    {
        for (int j = 0; j < N; j++) Console.Write($"{array[i, j]}".PadRight(output_width));
        Console.WriteLine();
    }
    Console.ResetColor();
    Console.WriteLine();
}

void PrintPath_1DArray(int[] array, int[] path)
{
    for (int i = 0, k = 0; i < array.Length; i++)
    {
        if (i == path[k])
        {
            Console.ForegroundColor = ConsoleColor.Green;
            k++;
        }
        else Console.ForegroundColor = ConsoleColor.Red;
        Console.Write($"{array[i]}".PadRight(5));
    }
    Console.ResetColor();
    Console.WriteLine();
}

void PrintPath_2DArray(int[,] array, Path[] path)
{
    int k = 0;
    for (int i = 0; i < M && k < M + N - 1; i++)
    {
        for (int j = 0; j < N && k < M + N - 1; j++)
        {
            if (i == path[k].path_x && j == path[k].path_y)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                k++;
            }
            else Console.ForegroundColor = ConsoleColor.Red;
            Console.Write($"{array[i, j]}".PadRight(output_width));
        }
        Console.WriteLine();
    }    
    Console.ResetColor();
    Console.WriteLine();
}


int[,] array_2d = new int[0, 0];
int[] array_1d = [];

Console.OutputEncoding = System.Text.Encoding.UTF8;

while (true) 
{
    Console.Clear();
    Console.Write("1) SelectSort\n2) BubbleSort\n3) MergeSort\n4) Лестница\n5) Шахматная доска\n6) Рюкзак\n7) Флойд\n8) Дейкстра\n9) Форд-Беллман\n0) Выход\n\nВыбор пункта: ");
    string choice = Console.ReadLine();
    Console.WriteLine();

    switch (choice)
    {
        case "1":
            Console.WriteLine("SelectSort");
            PrepareMethod(1, () => SelectSort(array_1d));
            break;

        case "2":
            Console.WriteLine("BubbleSort");
            PrepareMethod(1, () => BubbleSort(array_1d));
            break;

        case "3":
            Console.WriteLine("MergeSort");
            PrepareMethod(1, () => MergeSort(array_1d));
            break;

        case "4":
            Console.WriteLine("Лестница");
            PrepareMethod(1, () => Stairs(array_1d));
            break;

        case "5":
            Console.WriteLine("Шахматная доска");
            PrepareMethod(2, () => ChessBoard(array_2d));
            break;

        case "0": return;

        default: break;
    }
}

void PrepareMethod(int dimension, Action method)
{
    N = 0; M = 1;
    Random random = new();
    if (dimension > 1)
    {
        Console.Write("Введите M: ");
        M = Convert.ToInt16(Console.ReadLine());
    }
    Console.Write("Введите N: ");
    N = Convert.ToInt16(Console.ReadLine());

    if (dimension > 1)
    {
        Array.Clear(array_2d);
        array_2d = new int[M, N];
        for (int i = 0; i < M; i++)
        {
            for (int j = 0; j < N; j++) array_2d[i, j] = random.Next(-10, 10);
        }
        Print2DArray(array_2d);
        if (array_2d.Length >= 2) method();
    }
    else
    {
        Array.Clear(array_1d);
        array_1d = new int[N];
        for (int i = 0; i < N; i++) array_1d[i] = random.Next(-10, 10);
        Print1DArray(array_1d);
        if (array_1d.Length >= 2) method();
    }

    Console.ReadKey();
}

struct Path
{
    public int path_x;
    public int path_y;
}
