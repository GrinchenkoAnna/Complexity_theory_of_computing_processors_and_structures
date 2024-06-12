using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Complexity_theory_of_computing_processors_and_structures
{
    class CustomArray
    {
        private short dimension;            //размерность массива - 1 или 2
        private int columns, rows;     //размеры массива (столбцов, строк)

        public int[] custom_array1;         //одномерный массив
        public int[,] custom_array2;        //двумерный массив

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
            set
            {
                if (value >= 0) columns = value;
                else throw new ArgumentException("Недопустимый размер массива");
            }
        }
        public int Rows
        {
            get => rows;
            set
            {
                if (value >= 0) rows = value;
                else throw new ArgumentException("Недопустимый размер массива");
            }
        }

        public CustomArray(int size)
        {
            dimension = 1;
            Columns = size;
            custom_array1 = new int[Columns];
        }
        public CustomArray(int size, int min, int max) : this(size)
        {
            Random random = new();
            for (int i = 0; i < Columns; i++)
                custom_array1[i] = random.Next(min, max);
        }

        public CustomArray(int size1, int size2)
        {
            dimension = 2;
            Columns = size1;
            Rows = size2;
            custom_array2 = new int[Rows, Columns];
        }

        public CustomArray(int size1, int size2, int min, int max) : this(size1, size2)
        {
            Random random = new();
            for (int i = 0; i < Rows; i++)
                for (int j = 0; j < Columns; j++)
                    custom_array2[i, j] = random.Next(min, max);
        }

        public void PrintCustomArray(int output_width = 5)
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
                            Console.Write($"{custom_array2[i, j]}".PadRight(output_width));
                        Console.WriteLine();
                    }
                    break;
            }

            Console.ResetColor();
            Console.WriteLine();
        }

        public static CustomArray ResizeCustomArray(CustomArray old_custom_array, int new_size)
        {
            CustomArray new_custom_array = new(new_size);

            int border = old_custom_array.Columns <= new_custom_array.Columns ? old_custom_array.Columns : new_custom_array.Columns;
            for (int i = 0; i < border; i++)
                new_custom_array[i] = old_custom_array[i];

            return new_custom_array;
        }

        public static bool operator <(CustomArray lhs, CustomArray rhs)
        { return lhs < rhs; }
        public static bool operator >(CustomArray lhs, CustomArray rhs)
        { return lhs > rhs; }
        public static bool operator ==(CustomArray lhs, CustomArray rhs)
        { return lhs == rhs; }
        public static bool operator !=(CustomArray lhs, CustomArray rhs)
        { return lhs != rhs; }
    }
}
