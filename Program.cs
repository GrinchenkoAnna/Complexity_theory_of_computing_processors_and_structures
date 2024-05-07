﻿using System;
using System.Reflection;
using System.Runtime.CompilerServices;

#region Select
void SelectSort(List<int> array)
{
    int comparision = 0, permutation = 0;

    for (int i = 0; i < array.Count; i++)
    {
        int min = i;
        for (int j = i + 1; j < array.Count; j++)
        {
            if (array[j] < array[min]) min = j;
            comparision++;
        }
        (array[i], array[min]) = (array[min], array[i]);
        permutation++;
    }

    Console.WriteLine("\u2193");
    PrintArray(array);

    Console.WriteLine($"Теоретическая трудоемкость алгоритма: 1/2*N^2 = {Math.Ceiling((double)(Math.Pow(array.Count, 2)) / 2)}");
    Console.WriteLine($"Реальная трудоемкость алгоритма: {comparision + permutation}");
}
#endregion

#region Bubble
void BubbleSort(List<int> array)
{
    int comparision = 0, permutation = 0;

    for (int i = 0; i < array.Count; i++)
    {
        for (int j = 0; j < array.Count - 1 - i; j++)
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
    PrintArray(array);

    Console.WriteLine($"Теоретическая трудоемкость алгоритма: N^2 = {Math.Pow(array.Count, 2)}");
    Console.WriteLine($"Реальная трудоемкость алгоритма: {comparision + permutation}");
}
#endregion

#region Merge
void MergeSort(List<int> array)
{
    int comparision = 0, permutation = 0;
    int delete = CheckN(array);
    Sort(array, 0, array.Count - 1, ref comparision, ref permutation);

    Console.WriteLine("\u2193");
    PrintArray(array);
    if (delete != 0)
    {
        Console.WriteLine("Удаление лишних элементов:");
        for (int i = array.Count - 1; delete > 0; i--, delete--) array.Remove(array[i]);
        PrintArray(array);
    }

    Console.WriteLine($"Теоретическая трудоемкость алгоритма: N*logN = {array.Count * Math.Ceiling(Math.Log2(array.Count))}");
    Console.WriteLine($"Реальная трудоемкость алгоритма: {comparision + permutation}");
}
void Sort(List<int> array, int left, int right, ref int comparision, ref int permutation)
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
void Merge(List<int> array, int left, int middle, int right, int comparision, int permutation)
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
int CheckN(List<int> array)
{
    for (int i = 1; ; i++)
    {
        if (array.Count == Math.Pow(2, i))
        {
            Console.WriteLine($"Число элементов в массиве = {array.Count} = 2^k, добавлять элементы не требуется");
            return 0;
        }
        else if (array.Count < Math.Pow(2, i))
        {
            int to_add = (int)Math.Pow(2, i) - array.Count;
            Console.WriteLine($"Число элементов в массиве = {array.Count} != 2^k, необходимо добавить {to_add}");
            int M = FindMax(array);
            for (int j = 1; j <= to_add; j++) array.Add(M + j);
            Console.WriteLine("Результат:");
            PrintArray(array);

            return to_add;
        }
    }
}
int FindMax(List<int> array)
{
    int max = array[0];
    for (int i = 0; i < array.Count; i++)
    {
        if (array[i] > max) max = array[i];
    }
    return max;
}
#endregion

#region Stairs
void Stairs(List<int> array)
{
    int summa = 0;
    List<int> path = [];
    StairsF(array, array.Count - 1, path);
    Console.WriteLine("Путь: ");
    foreach(int item in path)
    {
        summa += array[item];
        Console.Write($"{item} ");
    }
    Console.WriteLine($"\nСумма: {summa}"); 
}

int StairsF(List<int> array, int number, List<int> path)
{
    Console.WriteLine($"number = {number}, array[number] = {array[number]}");
    path.Add(number);
    if (number == 0)
    {
        return 0;
    }
    if (number == 1)
    {
        return array[1];
    }
    return array[number] + Math.Max(StairsF(array, number - 1, path), StairsF(array, number - 2, path));
}
#endregion

static void PrintArray(List<int> array)
{
    Console.ForegroundColor = ConsoleColor.Red;
    foreach (int item in array) Console.Write($"{item} ");
    Console.ResetColor();
    Console.WriteLine();
}


List<int> array = [];
int N = 0;
Console.OutputEncoding = System.Text.Encoding.UTF8;

while (true) 
{
    Console.Clear();
    Console.Write("1) SelectSort\n2) BubbleSort\n3) MergeSort\n4) Лестница\n5) Шахматная доска\n6) Рюкзак\n7) Флойд\n8) Дейкстра\n9) Форд-Беллман\n0) Выход\n\nВыбор пункта: ");
    int choice = Convert.ToInt16(Console.ReadLine());
    Console.WriteLine();

    switch (choice)
    {
        case 1: 
            Console.WriteLine("SelectSort");   
            DoMethod(() => SelectSort(array)); 
            break;

        case 2:
            Console.WriteLine("BubbleSort");
            DoMethod(() => BubbleSort(array));
            break;

        case 3:
            Console.WriteLine("MergeSort");
            DoMethod(() => MergeSort(array));
            break;

        case 4:
            Console.WriteLine("Лестница");
            DoMethod(() => Stairs(array));
            break;

        case 0: return;

        default: break;
    }
}

void DoMethod(Action method)
{
    array.Clear();
    Console.Write("Введите N: ");
    N = Convert.ToInt16(Console.ReadLine());
    Random random = new();
    for (int i = 0; i < N; i++) array.Add(random.Next(0, 20));

    PrintArray(array);
    method();
    Console.ReadKey();
}
