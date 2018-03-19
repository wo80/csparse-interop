
namespace CSparse.Complex
{
    using CSparse.Storage;
    using System;
    using System.Numerics;

    public static class TestRunner
    {
        public static void Run()
        {
            Console.WriteLine("Running tests (Complex) ...");

            var A = Random(1000, 1000, 0.1);
            var B = RandomHermitian(1000, 0.1, true);
            
            new TestUmfpack().Run(A, B);
            new TestCholmod().Run(A, B);
            new TestSPQR().Run(A, B);
            new TestSuperLU().Run(A, B);
            new TestPardiso().Run(A, B);

            Console.WriteLine();
        }

        #region Matrix generator

        private const int RANDOM_SEED = 357801;

        /// <summary>
        /// Create a random sparse matrix.
        /// </summary>
        /// <param name="rows">The number of rows.</param>
        /// <param name="columns">The number of columns.</param>
        /// <param name="density">The density (between 0.0 and 1.0).</param>
        /// <returns>Random sparse matrix.</returns>
        private static SparseMatrix Random(int rows, int columns, double density)
        {
            return Random(rows, columns, density, new Random(RANDOM_SEED));
        }

        /// <summary>
        /// Create a random sparse matrix.
        /// </summary>
        /// <param name="rows">The number of rows.</param>
        /// <param name="columns">The number of columns.</param>
        /// <param name="density">The density (between 0.0 and 1.0).</param>
        /// <param name="random">The random source.</param>
        /// <returns>Random sparse matrix.</returns>
        private static SparseMatrix Random(int rows, int columns, double density, Random random)
        {
            // Number of non-zeros per row.
            int nz = (int)Math.Max(columns * density, 1d);

            var C = new CoordinateStorage<Complex>(rows, columns, rows * nz);

            for (int i = 0; i < rows; i++)
            {
                // Ensure non-zero diagonal.
                C.At(i, i, Complex.One);

                for (int j = 0; j < nz; j++)
                {
                    int k = Math.Min(columns - 1, (int)(random.NextDouble() * columns));
                    
                    C.At(i, k, new Complex(random.NextDouble(), random.NextDouble()));
                }
            }

            return Converter.ToCompressedColumnStorage(C) as SparseMatrix;
        }

        /// <summary>
        /// Create a random Hermitian sparse matrix.
        /// </summary>
        /// <param name="size">The size of the matrix.</param>
        /// <param name="density">The density (between 0.0 and 1.0).</param>
        /// <returns>Random sparse matrix.</returns>
        private static SparseMatrix RandomHermitian(int size, double density)
        {
            return RandomHermitian(size, density, false, new Random(RANDOM_SEED));
        }

        /// <summary>
        /// Create a random Hermitian sparse matrix.
        /// </summary>
        /// <param name="size">The size of the matrix.</param>
        /// <param name="density">The density (between 0.0 and 1.0).</param>
        /// <param name="definite">If true, the matrix will be positive semi-definite.</param>
        /// <returns>Random sparse matrix.</returns>
        private static SparseMatrix RandomHermitian(int size, double density, bool definite)
        {
            return RandomHermitian(size, density, definite, new Random(RANDOM_SEED));
        }

        /// <summary>
        /// Create a random Hermitian sparse matrix.
        /// </summary>
        /// <param name="size">The size of the matrix.</param>
        /// <param name="density">The density (between 0.0 and 1.0).</param>
        /// <param name="definite">If true, the matrix will be positive semi-definite.</param>
        /// <param name="random">The random source.</param>
        /// <returns>Random sparse matrix.</returns>
        private static SparseMatrix RandomHermitian(int size, double density, bool definite, Random random)
        {
            // Number of non-zeros per row.
            int nz = (int)Math.Max(size * size * density, 1d);

            var C = new CoordinateStorage<Complex>(size, size, nz);

            int m = (nz - size) / 2;

            var norm = new double[size];

            for (int k = 0; k < m; k++)
            {
                int i = (int)Math.Min(random.NextDouble() * size, size - 1);
                int j = (int)Math.Min(random.NextDouble() * size, size - 1);

                // Fill lower part.
                if (i > j)
                {
                    var value = new Complex(random.NextDouble(), random.NextDouble());

                    norm[i] += Complex.Abs(value);
                    norm[j] += Complex.Abs(value);

                    C.At(i, j, value);
                }
            }

            // Fill diagonal.
            for (int i = 0; i < size; i++)
            {
                double value = random.NextDouble();

                if (definite)
                {
                    // Make the matrix diagonally dominant.
                    value = (value + 1.0) * (norm[i] + 1.0);
                }

                C.At(i, i, value);
            }

            var A = Converter.ToCompressedColumnStorage(C);

            return (SparseMatrix)A.Add(A.Transpose());
        }

        #endregion
    }
}
