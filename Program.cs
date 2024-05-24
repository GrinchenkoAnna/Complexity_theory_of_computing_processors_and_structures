using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;

int N = 0, M = 1;
const int output_width = 5;
int[,] array_2d = new int[0, 0];
int[] array_1d = [];
bool positive = false;
int range = 20;
int weight = 0;

#region Select
void SelectSort()
{
    int comparision = 0, permutation = 0;

    for (int i = 0; i < array_1d.Length; i++)
    {
        int min = i;
        for (int j = i + 1; j < array_1d.Length; j++)
        {
            if (array_1d[j] < array_1d[min]) min = j;
            comparision++;
        }
        (array_1d[i], array_1d[min]) = (array_1d[min], array_1d[i]);
        permutation++;
    }

    Console.WriteLine("\u2193");
    Print1DArray();

    Console.WriteLine($"Теоретическая трудоемкость алгоритма: 1/2*N^2 = {Math.Ceiling((double)(Math.Pow(array_1d.Length, 2)) / 2)}");
    Console.WriteLine($"Реальная трудоемкость алгоритма: {comparision + permutation}");
}
#endregion

#region Bubble
void BubbleSort()
{
    int comparision = 0, permutation = 0;

    for (int i = 0; i < array_1d.Length; i++)
    {
        for (int j = 0; j < array_1d.Length - 1 - i; j++)
        {
            if (array_1d[j] > array_1d[j + 1])
            {
                (array_1d[j], array_1d[j + 1]) = (array_1d[j + 1], array_1d[j]);
                permutation++;
            }
            comparision++;
        }
    }

    Console.WriteLine("\u2193");
    Print1DArray();

    Console.WriteLine($"Теоретическая трудоемкость алгоритма: N^2 = {Math.Pow(array_1d.Length, 2)}");
    Console.WriteLine($"Реальная трудоемкость алгоритма: {comparision + permutation}");
}
#endregion

#region Merge
void MergeSort()
{
    int comparision = 0, permutation = 0;
    int delete = CheckN();
    Console.WriteLine($"delete = {delete}");
    Sort(0, array_1d.Length - 1, ref comparision, ref permutation);

    Console.WriteLine("\u2193");
    Print1DArray();
    if (delete != 0)
    {
        Console.WriteLine("Удаление лишних элементов:");
        Array.Resize(ref array_1d, array_1d.Length - delete);
        Print1DArray();
    }

    Console.WriteLine($"Теоретическая трудоемкость алгоритма: N*logN = {array_1d.Length * Math.Ceiling(Math.Log2(array_1d.Length))}");
    Console.WriteLine($"Реальная трудоемкость алгоритма: {comparision + permutation}");
}
void Sort(int left, int right, ref int comparision, ref int permutation)
{

    if (left < right)
    {
        comparision++;

        int middle = left + (right - left) / 2;
        Sort(left, middle, ref comparision, ref permutation);
        Sort(middle + 1, right, ref comparision, ref permutation);
        Merge(left, middle, right, comparision, permutation);

        permutation++;
    }
}
void Merge(int left, int middle, int right, int comparision, int permutation)
{
    int size1 = middle - left + 1;
    int size2 = right - middle;
    int i = 0, j = 0;

    List<int> tempLeft = [];
    List<int> tempRight = [];

    for (i = 0; i < size1; i++) tempLeft.Add(array_1d[left + i]);
    for (j = 0; j < size2; j++) tempRight.Add(array_1d[middle + 1 + j]);

    i = 0; j = 0;
    int k = left;

    while (i < size1 && j < size2)
    {
        if (tempLeft[i] <= tempRight[j]) { array_1d[k] = tempLeft[i]; i++; permutation++; }
        else { array_1d[k] = tempRight[j]; j++; permutation++; }
        k++; comparision++;
    }
    while (i < size1) { array_1d[k] = tempLeft[i]; i++; k++; permutation++; }
    while (j < size2) { array_1d[k] = tempRight[j]; j++; k++; permutation++; }
}
int CheckN()
{
    for (int i = 1; ; i++)
    {
        if (array_1d.Length == Math.Pow(2, i))
        {
            Console.WriteLine($"Число элементов в массиве = {array_1d.Length} = 2^k, добавлять элементы не требуется");
            return 0;
        }
        else if (array_1d.Length < Math.Pow(2, i))
        {
            int to_add = (int)Math.Pow(2, i) - array_1d.Length;
            int prev_length = array_1d.Length;
            Console.WriteLine($"Число элементов в массиве = {array_1d.Length} != 2^k, необходимо добавить {to_add}");
            int M = FindMax();
            Array.Resize(ref array_1d, array_1d.Length + to_add);
            for (int j = 1; j <= to_add; j++) array_1d[prev_length - 1 + j] = M + j; 
            Console.WriteLine("Результат:");
            Print1DArray();

            return to_add;
        }
    }
}
int FindMax()
{
    int max = array_1d[0];
    for (int i = 0; i < array_1d.Length; i++)
    {
        if (array_1d[i] > max) max = array_1d[i];
    }
    return max;
}
#endregion

#region Stairs
void Stairs()
{
    int[] summa = new int[N];
    int[] path = new int[N];

    summa[0] = array_1d[0];
    summa[1] = Math.Max(array_1d[1] + summa[0], array_1d[1]);
    for (int i = 2; i < N; i++) summa[i] = array_1d[i] + Math.Max(summa[i - 1], summa[i - 2]);

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
    PrintPath_1DArray(path);
    Console.WriteLine($"Сумма: {summa[N - 1]}");
}
#endregion

#region ChessBoard
void ChessBoard()
{
    int[,] summa = new int[M, N]; 
    Union[] path = new Union[M + N - 1];
    
    for (int i = 0; i < M; i++)
    {
        for (int j = 0; j < N; j++)
        {
            if (i > 0 && j > 0) summa[i, j] = array_2d[i, j] + Math.Max(summa[i, j - 1], summa[i - 1, j]);
            else if (i > 0 && j == 0) summa[i, j] = array_2d[i, j] + summa[i - 1, j];
            else if (i == 0 && j > 0) summa[i, j] = array_2d[i, j] + summa[i, j - 1];
            else summa[0, 0] = array_2d[0, 0];
        }
    }

    path[0].first_member = M - 1;
    path[0].second_member = N - 1;
    int path_index = 1, x = M - 1, y = N - 1;
    while ((x != 0 || y != 0) && path_index < M + N - 2)
    {
        path[path_index].first_member = x;
        path[path_index].second_member = y;

        if (x > 0 && y > 0)
        {
            if (summa[x, y - 1] >= summa[x - 1, y]) { path[path_index].second_member -= 1; y -= 1; }
            else { path[path_index].first_member -= 1; x -= 1; }
        }
        else if (x > 0 && y == 0) { path[path_index].first_member -= 1; x -= 1; }
        else if (x == 0 && y > 0) { path[path_index].second_member -= 1; y -= 1; }

        path_index++;
    }
    path[path_index].first_member = 0;
    path[path_index].second_member = 0;

    Array.Reverse(path);

    Console.WriteLine("Путь: ");
    PrintPath_2DArray(path);
    Console.WriteLine($"Сумма: {summa[M - 1, N - 1]}");

}
#endregion

#region Basackpack
void Backpack(int max_weight)
{    
    //Union = weight, price
    Union[] items = new Union[max_weight];
    List<int> candidats = new List<int>();
    int unit = 0;
    int filled = 0;

    for (int j = 0; j < N; j++)
    {
        items[j].first_member = array_2d[0, j];
        items[j].second_member = array_2d[1, j];
    }
    Print2DArray();

    filled = N;
    //ничего не добавляется...
    for (int k = array_2d[0, N - 1] + 1; k <= max_weight; k++)
    {
        for (int j = 0; j < filled; j++)
        {
            unit = items[j].first_member;
            Console.WriteLine($"unit = {unit}, k - unit = {k - unit}");
            if (items.Any(x => x.first_member == k - unit))
            {
                candidats.Add(items[k - unit].second_member + items[unit].second_member);
            }
        }

        foreach (Union item in items)
        {
            Console.Write($"weight - {item.first_member}, price - {item.second_member}");
            Console.WriteLine();
        }

        if (candidats.Count != 0)
        {
            items[k].second_member = candidats.Max();
            filled++;
            candidats.Clear();
        }            
    }

    foreach (Union item in items) 
    { 
        Console.Write($"weight - {item.first_member}, price - {item.second_member}");
        Console.WriteLine();
    }
}
#endregion
void Print1DArray()
{
    Console.ForegroundColor = ConsoleColor.Red;
    for (int i = 0; i < array_1d.Length; i++) Console.Write($"{array_1d[i]}".PadRight(output_width));
    Console.ResetColor();
    Console.WriteLine();
}

void Print2DArray()
{
    Console.ForegroundColor = ConsoleColor.Red;
    for (int i = 0; i < M; i++)
    {
        for (int j = 0; j < N; j++) Console.Write($"{array_2d[i, j]}".PadRight(output_width));
        Console.WriteLine();
    }
    Console.ResetColor();
    Console.WriteLine();
}

void PrintPath_1DArray(int[] path)
{
    for (int i = 0, k = 0; i < array_1d.Length; i++)
    {
        if (i == path[k])
        {
            Console.ForegroundColor = ConsoleColor.Green;
            k++;
        }
        else Console.ForegroundColor = ConsoleColor.Red;
        Console.Write($"{array_1d[i]}".PadRight(5));
    }
    Console.ResetColor();
    Console.WriteLine();
}

void PrintPath_2DArray(Union[] path)
{
    //Union = x, y
    int k = 0;
    for (int i = 0; i < M && k < M + N - 1; i++)
    {
        for (int j = 0; j < N && k < M + N - 1; j++)
        {
            if (i == path[k].first_member && j == path[k].second_member)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                k++;
            }
            else Console.ForegroundColor = ConsoleColor.Red;
            Console.Write($"{array_2d[i, j]}".PadRight(output_width));
        }
        Console.WriteLine();
    }    
    Console.ResetColor();
    Console.WriteLine();
}

Console.OutputEncoding = System.Text.Encoding.UTF8;

while (true) 
{
    Console.Clear();
    Console.Write("1) SelectSort\n2) BubbleSort\n3) MergeSort\n4) Лестница\n5) Шахматная доска\n6) Рюкзак\n7) Флойд\n8) Дейкстра\n9) Форд-Беллман\n0) Выход\n\nВыбор пункта: ");
    string choice = Console.ReadLine();
    Console.WriteLine();

    if (positive == true) positive = false;

    switch (choice)
    {
        case "1":
            Console.WriteLine("SelectSort");
            PrepareData(1);
            SelectSort();
            break;

        case "2":
            Console.WriteLine("BubbleSort");
            PrepareData(2);
            BubbleSort();
            break;

        case "3":
            Console.WriteLine("MergeSort");
            PrepareData(3);
            MergeSort();
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
            Backpack(PrepareData(6));
            break;

        case "0": return;

        default: break;
    }
    Console.ReadKey();
}

int PrepareData(int choice)
{
    N = 0; M = 1; weight = 0;
    //Console.Write("Введите N (количество столбцов): ");
    //N = Convert.ToInt16(Console.ReadLine());

    Random random = new();
    int right_border, left_border;
    switch (positive)
    {
        case false:
            right_border = -10; break;

        case true:
            right_border = 1; break;
    }
    left_border = right_border + range;

    switch (choice)
    {
        case 1:
        case 2:
        case 3:
        case 4:
            Array.Clear(array_1d);
            array_1d = new int[N];
            for (int i = 0; i < N; i++) array_1d[i] = random.Next(right_border, left_border);
            Print1DArray();
            break;
            
        case 5:
        case 6:
            if (choice == 6)
            {
                M = 2; N = 3;
                //Console.Write("Максимальный вес: ");
                //weight = Convert.ToInt16(Console.ReadLine());
                weight = 20;
                array_2d = new int[2, 3];
                array_2d[0, 0] = 3;
                array_2d[0, 1] = 5;
                array_2d[0, 2] = 7;
                array_2d[1, 0] = 8;
                array_2d[1, 1] = 14;
                array_2d[1, 2] = 20;
                break;
            }
            else
            {
                Console.Write("Введите M (количество строк): ");
                M = Convert.ToInt16(Console.ReadLine());
            } 
            Array.Clear(array_2d);
            array_2d = new int[M, N];
            for (int i = 0; i < M; i++)
            {
                for (int j = 0; j < N; j++) array_2d[i, j] = random.Next(right_border, left_border);
            }
            Print2DArray();
            break;
    }

    return weight;
}

struct Union
{
    public int first_member;
    public int second_member;
}
