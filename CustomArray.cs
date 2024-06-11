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
        private int size_col, size_row;     //размеры массива (столбцов, строк)

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

        public int SizeCol
        {
            get => size_col;
            set
            {
                if (value >= 0) size_col = value;
                else throw new ArgumentException("Недопустимый размер массива");
            }
        }
        public int SizeRow
        {
            get => size_row;
            set
            {
                if (value >= 0) size_row = value;
                else throw new ArgumentException("Недопустимый размер массива");
            }
        }

        public CustomArray(int size)
        {
            dimension = 1;
            SizeCol = size;
            custom_array1 = new int[SizeCol];
        }
        public CustomArray(int size, int min, int max) : this(size)
        {
            Random random = new();
            for (int i = 0; i < SizeCol; i++)
                custom_array1[i] = random.Next(min, max);
        }

        public CustomArray(int size1, int size2)
        {
            dimension = 2;
            SizeCol = size1;
            SizeRow = size2;
            custom_array2 = new int[SizeRow, SizeCol];
        }

        public CustomArray(int size1, int size2, int min, int max) : this(size1, size2)
        {
            Random random = new();
            for (int i = 0; i < SizeRow; i++)
                for (int j = 0; j < SizeCol; j++)
                    custom_array2[i, j] = random.Next(min, max);
        }

        public void PrintCustomArray(int output_width)
        {
            Console.ForegroundColor = ConsoleColor.Red;

            switch (dimension)
            {
                case 1:
                    for (int i = 0; i < SizeCol; i++)
                        Console.Write($"{custom_array1[i]}".PadRight(output_width));
                    break;

                case 2:
                    for (int i = 0; i < SizeRow; i++)
                    {
                        for (int j = 0; j < SizeCol; j++)
                            Console.Write($"{custom_array2[i, j]}".PadRight(output_width));
                        Console.WriteLine();
                    }
                    break;
            }

            Console.ResetColor();
            Console.WriteLine();
        }

        public int GetElement(int i) { return custom_array1[i]; }
        public int GetElement(int i, int j) { return custom_array2[i, j]; }

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
