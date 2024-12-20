using NUnit.Framework;
using pv_projekt;

namespace pv_projekt
{
    [TestFixture]
    public class MatrixMultiplierTests
    {
        /// <summary>
        /// Tests the matrix multiplication functionality with a small 2x2 matrix.
        /// </summary>
        /// <remarks>
        /// This test verifies that the matrix multiplication is performed correctly for small matrices.
        /// </remarks>
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

        /// <summary>
        /// Tests the matrix multiplication functionality with a larger 3x3 matrix.
        /// </summary>
        /// <remarks>
        /// This test verifies that the matrix multiplication is performed correctly for larger matrices.
        /// </remarks>
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

        /// <summary>
        /// Tests the matrix multiplication functionality using an identity matrix.
        /// </summary>
        /// <remarks>
        /// This test verifies that multiplying any matrix by an identity matrix results in the original matrix.
        /// </remarks>
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

        /// <summary>
        /// Tests the matrix multiplication functionality using a zero matrix.
        /// </summary>
        /// <remarks>
        /// This test verifies that multiplying any matrix by a zero matrix results in a zero matrix.
        /// </remarks>
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
        /// <summary>
        /// Tests the matrix multiplication functionality with matrices containing negative numbers.
        /// </summary>
        /// <remarks>
        /// This test verifies that the matrix multiplication is performed correctly when both matrices contain negative numbers.
        /// </remarks>
        [Test]
        public void TestMatrixMultiplication_NegativeNumbers()
        {
            // Arrange
            int[,] matrixA = new int[,] { { -1, -2 }, { -3, -4 } };
            int[,] matrixB = new int[,] { { -5, -6 }, { -7, -8 } };
            int[,] expected = new int[,] { { 19, 22 }, { 43, 50 } };
            int[,] result = new int[2, 2];

            MatrixMultiplier multiplier = new MatrixMultiplier(matrixA, matrixB, result);

            // Act
            multiplier.ParallelMultiply();

            // Assert
            Assert.That(result, Is.EqualTo(expected));
        }
    }
}
