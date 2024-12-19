using NUnit.Framework;
using pv_projekt;

namespace pv_projekt
{
    [TestFixture]
    public class MatrixMultiplierTests
    {
        [Test]
        public void TestMatrixMultiplication_SmallMatrix()
        {
            // Arrange
            int[,] matrixA = new int[,] { { 1, 2 }, { 3, 4 } };
            int[,] matrixB = new int[,] { { 5, 6 }, { 7, 8 } };
            int[,] expected = new int[,] { { 19, 22 }, { 43, 50 } };
            int[,] result = new int[2, 2];

            MatrixMultiplier multiplier = new MatrixMultiplier(matrixA, matrixB, result);

            // Act
            multiplier.ParallelMultiply();

            // Assert
            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        public void TestMatrixMultiplication_LargerMatrix()
        {
            // Arrange
            int[,] matrixA = new int[,] { { 1, 2, 3 }, { 4, 5, 6 }, { 7, 8, 9 } };
            int[,] matrixB = new int[,] { { 9, 8, 7 }, { 6, 5, 4 }, { 3, 2, 1 } };
            int[,] expected = new int[,] 
            { 
                { 30, 24, 18 }, 
                { 84, 69, 54 }, 
                { 138, 114, 90 } 
            };
            int[,] result = new int[3, 3];

            MatrixMultiplier multiplier = new MatrixMultiplier(matrixA, matrixB, result);

            // Act
            multiplier.ParallelMultiply();

            // Assert
            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        public void TestMatrixMultiplication_IdentityMatrix()
        {
            // Arrange
            int[,] identity = new int[,] { { 1, 0 }, { 0, 1 } };
            int[,] matrix = new int[,] { { 5, 6 }, { 7, 8 } };
            int[,] expected = new int[,] { { 5, 6 }, { 7, 8 } };
            int[,] result = new int[2, 2];

            MatrixMultiplier multiplier = new MatrixMultiplier(identity, matrix, result);

            // Act
            multiplier.ParallelMultiply();

            // Assert
            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        public void TestMatrixMultiplication_ZeroMatrix()
        {
            // Arrange
            int[,] zeroMatrix = new int[,] { { 0, 0 }, { 0, 0 } };
            int[,] matrix = new int[,] { { 5, 6 }, { 7, 8 } };
            int[,] expected = new int[,] { { 0, 0 }, { 0, 0 } };
            int[,] result = new int[2, 2];

            MatrixMultiplier multiplier = new MatrixMultiplier(zeroMatrix, matrix, result);

            // Act
            multiplier.ParallelMultiply();

            // Assert
            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        public void TestMatrixMultiplication_DifferentSizes_ThrowsException()
        {
            // Arrange
            int[,] matrixA = new int[,] { { 1, 2 }, { 3, 4 } };
            int[,] matrixB = new int[,] { { 5, 6, 7 }, { 8, 9, 10 } };
            int[,] result = new int[2, 3]; // Invalid size for result

            // Act & Assert
            Assert.Throws<ArgumentException>(() =>
            {
                MatrixMultiplier multiplier = new MatrixMultiplier(matrixA, matrixB, result);
                multiplier.ParallelMultiply();
            });
        }
    }
}
