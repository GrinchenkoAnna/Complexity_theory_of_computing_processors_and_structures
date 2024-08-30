using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Complexity_theory_of_computing_processors_and_structures
{
    class CustomArray
    {
        private short dimension;       //размерность массива - 1 или 2
        private int columns, rows;     //размеры массива (столбцов, строк)

        public int[] custom_array1;    //одномерный массив
        public int[,] custom_array2;   //двумерный массив

        public int this[int i]
        {
            get => custom_array1[i];
            set => custom_array1[i] = value;
        }
        public int this[int i, int j]
        {
            get => custom_array2[i, j];
            set => custom_array2[i, j] = value;
        }

        public int Columns
        {
            get => columns;
            set => columns = value >= 0 ? value : throw new ArgumentException($"Недопустимое количество столбцов: {value}");
        }
        public int Rows
        {
            get => rows;
            set => rows = value >= 0 ? value : throw new ArgumentException($"Недопустимое количество строк: {value}");
        }

        public CustomArray() { }

        public CustomArray(int size)
        {
            dimension = 1;
            Columns = size;
            custom_array1 = new int[Columns];
        }
        public CustomArray(int size, int min, int max) : this(size)
        {
            Random random_arr = new();
            for (int i = 0; i < Columns; i++)
                custom_array1[i] = random_arr.Next(min, max);
        }

        public CustomArray(int size1, int size2, bool test = false, int num = 0)
        {
            dimension = 2;
            Rows = size1;
            Columns = size2;
            custom_array2 = new int[Rows, Columns];

            if (test)
            {
                switch (num)
                {
                    case 6:
                        custom_array2[0, 0] = 5;
                        custom_array2[0, 1] = 7;
                        custom_array2[0, 2] = 11;
                        custom_array2[1, 0] = 9;
                        custom_array2[1, 1] = 13;
                        custom_array2[1, 2] = 21;
                        break;

                    case 7:
                        custom_array2[0, 0] = 3;
                        custom_array2[0, 1] = 5;
                        custom_array2[0, 2] = 7;
                        custom_array2[1, 0] = 8;
                        custom_array2[1, 1] = 14;
                        custom_array2[1, 2] = 20;
                        break;

                    default:
                        break;
                }
            }            
        }

        public CustomArray(int size1, int size2, int min, int max) : this(size1, size2)
        {
            Random random = new();
            for (int i = 0; i < Rows; i++)
                for (int j = 0; j < Columns; j++)
                    custom_array2[i, j] = random.Next(min, max);
        }

        public void PrintSingleElement(int i)
        { Console.Write(custom_array1[i]); }

        public void PrintSingleElement(int i, int j)
        { Console.Write(custom_array2[i, j]); }

        public void PrintArray(int output_width = 5)
        {
            Console.ForegroundColor = ConsoleColor.Red;

            switch (dimension)
            {
                case 1:
                    for (int i = 0; i < Columns; i++)
                        Console.Write($"{custom_array1[i]}".PadRight(output_width));
                    break;

                case 2:
                    for (int i = 0; i < Rows; i++)
                    {
                        for (int j = 0; j < Columns; j++)
                            Console.Write($"{custom_array2[i, j]}".PadLeft(output_width));
                        Console.WriteLine();
                    }
                    break;
            }

            Console.ResetColor();
            Console.WriteLine();
        }        

        public void PrintPath(int[] path, int output_width = 5)
        {
            for (int i = 0, k = 0; i < Columns; i++)
            {
                if (i == path[k])
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    k++;
                }
                else
                    Console.ForegroundColor = ConsoleColor.Red;
                Console.Write($"{custom_array1[i]}".PadRight(output_width));
            }
            Console.ResetColor();
            Console.WriteLine();
        } 
        
        public void PrintPath(Program.Union[] path, int output_width = 5)
        {
            //Union = x, y
            int k = 0;
            for (int i = 0; i < Rows && k < Rows + Columns - 1; i++)
            {
                for (int j = 0; j < Columns && k < Rows + Columns - 1; j++)
                {
                    if (i == path[k].first && j == path[k].second)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        k++;
                    }
                    else
                        Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write($"{custom_array2[i, j]}".PadLeft(output_width));
                }
                Console.WriteLine();
            }
            Console.ResetColor();
            Console.WriteLine();
        }

        public static CustomArray ResizeArray(CustomArray old_custom_array, int new_size)
        {
            CustomArray new_custom_array = new(new_size);

            int border = old_custom_array.Columns <= new_custom_array.Columns ? old_custom_array.Columns : new_custom_array.Columns;
            for (int i = 0; i < border; i++)
                new_custom_array[i] = old_custom_array[i];

            return new_custom_array;
        }

        //не используется
        public static CustomArray ReverseArray(CustomArray old_custom_array)
        {
            int size = old_custom_array.Columns;
            CustomArray new_custom_array = new(size);

            for (int i = 0; i < size; i++)
                new_custom_array[i] = old_custom_array[size - 1 - i];

            return new_custom_array;
        }

        public static bool operator <(CustomArray lhs, CustomArray rhs)
        { return lhs < rhs; }
        public static bool operator >(CustomArray lhs, CustomArray rhs)
        { return lhs > rhs; }
        public static bool operator ==(CustomArray lhs, CustomArray rhs)
        { return lhs == rhs; }
        public static bool operator ==(int lhs, CustomArray rhs) //не используется
        { 
            //оптимизировать - хэш-таблица или бинарный поиск
            for (int i = 0; i < rhs.Columns; i++)
                if (lhs == rhs[i]) 
                    return true;
            return false; 
        }
        public static bool operator !=(CustomArray lhs, CustomArray rhs)
        { return lhs != rhs; }
        public static bool operator !=(int lhs, CustomArray rhs) //не используется
        { return !(lhs == rhs); }
    }
}
