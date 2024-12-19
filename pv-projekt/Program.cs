using System;
using System.Threading;

namespace pv_projekt
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Paralelní násobení matic");

            int size = 0;
            while (true)
            {
                Console.Write("Zadej velikost matice (N x N, N > 0): ");
                string input = Console.ReadLine();

                try
                {
                    size = int.Parse(input);
                    if (size <= 0)
                    {
                        Console.WriteLine("Velikost matice musí být kladné číslo větší než 0. Zkus to znovu.");
                        continue;
                    }else if (size > 20)
                    {
                        Console.WriteLine("Velikost matice nemůže být větší než 20. Zkus to znovu.");
                        continue;
                    }
                    break;
                }
                catch (FormatException)
                {
                    Console.WriteLine("Zadaná hodnota není číslo. Zkus to znovu.");
                }
                catch (OverflowException)
                {
                    Console.WriteLine("Zadané číslo je příliš velké. Zkus to znovu.");
                }
            }

            // Generování náhodných matic
            int[,] matrixA = GenerateMatrix(size);
            int[,] matrixB = GenerateMatrix(size);

            Console.WriteLine("\nMatice A:");
            PrintMatrix(matrixA);

            Console.WriteLine("\nMatice B:");
            PrintMatrix(matrixB);

            // Výpočet paralelně
            int[,] result = new int[size, size];
            try
            {
                MatrixMultiplier multiplier = new MatrixMultiplier(matrixA, matrixB, result);
                multiplier.ParallelMultiply();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Při paralelním násobení došlo k chybě: {ex.Message}");
                return;
            }

            Console.WriteLine("\nVýsledek (Matice C):");
            PrintMatrix(result);
        }

        // Generování náhodné matice
        static int[,] GenerateMatrix(int size)
        {
            Random random = new Random();
            int[,] matrix = new int[size, size];
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    matrix[i, j] = random.Next(1, 10); // Náhodná čísla 1-9
                }
            }
            return matrix;
        }

        // Tisk matice
        static void PrintMatrix(int[,] matrix)
        {
            int rows = matrix.GetLength(0);
            int cols = matrix.GetLength(1);
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    Console.Write($"{matrix[i, j],3} ");
                }
                Console.WriteLine();
            }
        }
    }

    public class MatrixMultiplier
    {
        private int[,] MatrixA;
        private int[,] MatrixB;
        private int[,] Result;
        private int Size;

        public MatrixMultiplier(int[,] matrixA, int[,] matrixB, int[,] result)
        {
            MatrixA = matrixA;
            MatrixB = matrixB;
            Result = result;
            Size = matrixA.GetLength(0);
        }

        // Paralelní násobení
        public void ParallelMultiply()
        {
            int numThreads = Environment.ProcessorCount; // Počet dostupných vláken
            Thread[] threads = new Thread[numThreads];

            // Kontrola správného rozdělení na vlákna
            int rowsPerThread = (Size + numThreads - 1) / numThreads; // Zaokrouhlení nahoru
            for (int i = 0; i < numThreads; i++)
            {
                int startRow = i * rowsPerThread;
                int endRow = Math.Min(startRow + rowsPerThread, Size);

                if (startRow >= Size) break; // Pokud by došlo k překročení řádků

                threads[i] = new Thread(() => MultiplyRows(startRow, endRow));
                threads[i].Start();
            }

            // Čekání na dokončení všech vláken
            foreach (var thread in threads)
            {
                thread?.Join(); // Ověření, zda vlákno není null
            }
        }

        // Výpočet určitého rozsahu řádků
        private void MultiplyRows(int startRow, int endRow)
        {
            for (int i = startRow; i < endRow; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    Result[i, j] = 0;
                    for (int k = 0; k < Size; k++)
                    {
                        Result[i, j] += MatrixA[i, k] * MatrixB[k, j];
                    }
                }
            }
        }
    }
}
