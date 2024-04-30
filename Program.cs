void SelectSort(List<int> array, int N)
{
    int comparision = 0, permutation = 0;
    for (int i = 0; i < N; i++)
    {
        int min = i;
        for (int j = i + 1; j < N; j++)
        {
            if (array[j] < array[min]) min = j;
            comparision++;
        }
        (array[i], array[min]) = (array[min], array[i]);
        permutation++;
    }

    Console.WriteLine("\u2193");
    PrintArray(array);

    Console.WriteLine($"Теоретическая трудоемкость алгоритма: 1/2*N^2 = {Math.Ceiling((double)(Math.Pow(N, 2))/2)}");
    Console.WriteLine($"Реальная трудоемкость алгоритма: {comparision + permutation}");
}

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
    Console.Write("1) SelectSort\n2) BubbleSort\n3) MergeSort\n4) Лестница\n5) Шахматная доска\n6) Рюкзак\n7) Флойд\n8) Дейкстра\n9) Форд-Беллман\n10) Выход\n\nВыбор пункта: ");
    int choice = Convert.ToInt16(Console.ReadLine());
    Console.WriteLine();

    switch (choice)
    {
        case 1: 
            Console.WriteLine("Выбран SelectSort");   
            DoMethod(() => SelectSort(array, N)); 
            break;

        case 10: return;
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
    Console.WriteLine();
}