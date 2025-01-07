using System;
using System.Threading;

namespace pv_projekt
{
    /// <summary>
    /// Main program class for parallel matrix multiplication.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// The entry point of the program.
        /// Handles matrix size input, random matrix generation, and parallel multiplication.
        /// </summary>
        /// <param name="args">Command-line arguments (not used).</param>
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
                    }
                    else if (size > 100)
                    {
                        Console.WriteLine("Velikost matice nemůže být větší než 100. Zkus to znovu.");
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

            // Generating random matrices
            int[,] matrixA = GenerateMatrix(size);
            int[,] matrixB = GenerateMatrix(size);

            Console.WriteLine("\nMatice A:");
            PrintMatrix(matrixA);

            Console.WriteLine("\nMatice B:");
            PrintMatrix(matrixB);

            // Perform parallel multiplication
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
            
            Console.WriteLine("\nStiskněte ENTER pro ukončení...");
            Console.ReadLine();
        }

        /// <summary>
        /// Generates a random square matrix of the given size.
        /// </summary>
        /// <param name="size">The size of the square matrix (N x N).</param>
        /// <returns>A 2D array representing the matrix.</returns>
        static int[,] GenerateMatrix(int size)
        {
            Random random = new Random();
            int[,] matrix = new int[size, size];
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    matrix[i, j] = random.Next(1, 10); // Random numbers between 1 and 9
                }
            }
            return matrix;
        }

        /// <summary>
        /// Prints a matrix to the console.
        /// </summary>
        /// <param name="matrix">The matrix to print.</param>
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

    /// <summary>
    /// A class for performing parallel matrix multiplication.
    /// </summary>
    public class MatrixMultiplier
    {
        private int[,] MatrixA;
        private int[,] MatrixB;
        private int[,] Result;
        private int Size;

        /// <summary>
        /// Initializes a new instance of the <see cref="MatrixMultiplier"/> class.
        /// </summary>
        /// <param name="matrixA">The first input matrix.</param>
        /// <param name="matrixB">The second input matrix.</param>
        /// <param name="result">The matrix to store the multiplication result.</param>
        public MatrixMultiplier(int[,] matrixA, int[,] matrixB, int[,] result)
        {
            MatrixA = matrixA;
            MatrixB = matrixB;
            Result = result;
            Size = matrixA.GetLength(0);
        }

        /// <summary>
        /// Performs parallel matrix multiplication using multiple threads.
        /// </summary>
        public void ParallelMultiply()
        {
            int numThreads = Environment.ProcessorCount; // Number of available threads
            Thread[] threads = new Thread[numThreads];

            // Divide rows among threads
            int rowsPerThread = (Size + numThreads - 1) / numThreads; // Round up
            for (int i = 0; i < numThreads; i++)
            {
                int startRow = i * rowsPerThread;
                int endRow = Math.Min(startRow + rowsPerThread, Size);

                if (startRow >= Size) break; // Avoid processing out of bounds

                threads[i] = new Thread(() => MultiplyRows(startRow, endRow));
                threads[i].Start();
            }

            // Wait for all threads to complete
            foreach (var thread in threads)
            {
                thread?.Join(); // Check for null threads
            }
        }

        /// <summary>
        /// Multiplies rows of the matrices for a specific range.
        /// </summary>
        /// <param name="startRow">The starting row index.</param>
        /// <param name="endRow">The ending row index (exclusive).</param>
        private void MultiplyRows(int startRow, int endRow)
        {
            try
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
            catch (Exception ex)
            {
                Console.WriteLine($"Chyba při výpočtu pro řádek {startRow}-{endRow}: {ex.Message}");
            }
        }
    }
}
