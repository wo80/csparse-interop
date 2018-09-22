
namespace CSparse.Complex
{
    using CSparse.Storage;
    using System;
    using System.Numerics;

    static class Generate
    {
        #region Random

        private const int RANDOM_SEED = 357801;

        /// <summary>
        /// Create a random sparse matrix.
        /// </summary>
        /// <param name="rows">The number of rows.</param>
        /// <param name="columns">The number of columns.</param>
        /// <param name="density">The density (between 0.0 and 1.0).</param>
        /// <returns>Random sparse matrix.</returns>
        public static SparseMatrix Random(int rows, int columns, double density)
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
        public static SparseMatrix Random(int rows, int columns, double density, Random random)
        {
            // Number of non-zeros per row.
            int nz = (int)Math.Max(columns * density, 1d);

            var C = new CoordinateStorage<Complex>(rows, columns, rows * nz);

            for (int i = 0; i < rows; i++)
            {
                // Ensure non-zero diagonal.
                C.At(i, i, random.NextDouble() - 0.5);

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
        public static SparseMatrix RandomHermitian(int size, double density)
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
        public static SparseMatrix RandomHermitian(int size, double density, bool definite)
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
        public static SparseMatrix RandomHermitian(int size, double density, bool definite, Random random)
        {
            // Total number of non-zeros.
            int nz = (int)Math.Max(size * size * density, 1d);

            var C = new CoordinateStorage<Complex>(size, size, nz);

            int m = nz / 2;

            var norm = new double[size];

            for (int k = 0; k < m; k++)
            {
                int i = (int)Math.Min(random.NextDouble() * size, size - 1);
                int j = (int)Math.Min(random.NextDouble() * size, size - 1);

                if (i == j)
                {
                    // Skip diagonal.
                    continue;
                }

                // Fill only lower part.
                if (i < j)
                {
                    int temp = i;
                    i = j;
                    j = temp;
                }

                var value = new Complex(random.NextDouble(), random.NextDouble());

                norm[i] += Complex.Abs(value);
                norm[j] += Complex.Abs(value);

                C.At(i, j, value);
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

        #region Laplacian

        /// <summary>
        /// Get the 1D Laplacian matrix (with Dirichlet boundary conditions).
        /// </summary>
        /// <param name="nx">Grid size.</param>
        /// <param name="eigenvalues">Vector to store eigenvalues (optional).</param>
        /// <returns>Laplacian sparse matrix.</returns>
        public static CompressedColumnStorage<Complex> Laplacian(int nx, double[] eigenvalues = null)
        {
            if (nx == 1)
            {
                // Handle special case n = 1.
                var A = new CoordinateStorage<Complex>(nx, nx, 1);

                A.At(0, 0, 2.0);

                return Converter.ToCompressedColumnStorage(A);
            }

            var C = new CoordinateStorage<Complex>(nx, nx, 3 * nx);

            for (int i = 0; i < nx; i++)
            {
                C.At(i, i, 2.0);

                if (i == 0)
                {
                    C.At(i, i + 1, -1.0);
                }
                else if (i == (nx - 1))
                {
                    C.At(i, i - 1, -1.0);
                }
                else
                {
                    C.At(i, i - 1, -1.0);
                    C.At(i, i + 1, -1.0);
                }
            }

            if (eigenvalues != null)
            {
                // Compute eigenvalues.
                int count = Math.Min(nx, eigenvalues.Length);

                var eigs = new double[nx];

                for (int i = 0; i < count; i++)
                {
                    eigs[i] = 4 * Math.Pow(Math.Sin((i + 1) * Math.PI / (2 * (nx + 1))), 2);
                }

                Array.Sort(eigs);

                for (int i = 0; i < count; ++i)
                {
                    eigenvalues[i] = eigs[i];
                }
            }

            return Converter.ToCompressedColumnStorage(C);
        }

        /// <summary>
        /// Get the 2D Laplacian matrix (with Dirichlet boundary conditions).
        /// </summary>
        /// <param name="nx">Number of elements in x direction.</param>
        /// <param name="ny">Number of elements in y direction.</param>
        /// <param name="eigenvalues">Vector to store eigenvalues (optional).</param>
        /// <returns>Laplacian sparse matrix.</returns>
        public static CompressedColumnStorage<Complex> Laplacian(int nx, int ny, double[] eigenvalues = null)
        {
            var Ix = Eye(nx);
            var Iy = Eye(ny);

            var Dx = Laplacian(nx);
            var Dy = Laplacian(ny);

            if (eigenvalues != null)
            {
                // Compute eigenvalues.
                int count = Math.Min(nx * ny, eigenvalues.Length);
                int index = 0;

                var eigs = new double[nx * ny];

                double ax, ay;

                for (int i = 0; i < nx; i++)
                {
                    ax = 4 * Math.Pow(Math.Sin((i + 1) * Math.PI / (2 * (nx + 1))), 2);
                    for (int j = 0; j < ny; j++)
                    {
                        ay = 4 * Math.Pow(Math.Sin((j + 1) * Math.PI / (2 * (ny + 1))), 2);
                        eigs[index++] = ax + ay;
                    }
                }

                Array.Sort(eigs);

                for (int i = 0; i < count; ++i)
                {
                    eigenvalues[i] = eigs[i];
                }
            }

            return Kronecker(Iy, Dx).Add(Kronecker(Dy, Ix));
        }

        /// <summary>
        /// Create a sparse identity matrix.
        /// </summary>
        /// <param name="rows">The size of the matrix.</param>
        /// <returns>Sparse identity matrix.</returns>
        private static CompressedColumnStorage<Complex> Eye(int size)
        {
            var A = new SparseMatrix(size, size);

            var ap = new int[size + 1];
            var ai = new int[size];
            var ax = new Complex[size];

            for (int i = 0; i < size; i++)
            {
                ap[i] = i;
                ai[i] = i;
                ax[i] = Complex.One;
            }

            ap[size] = size;

            A.ColumnPointers = ap;
            A.RowIndices = ai;
            A.Values = ax;

            return A;
        }

        /// <summary>
        /// Computes the Kronecker product.
        /// </summary>
        private static CompressedColumnStorage<Complex> Kronecker(CompressedColumnStorage<Complex> A, CompressedColumnStorage<Complex> B)
        {
            var ap = A.ColumnPointers;
            var aj = A.RowIndices;
            var ax = A.Values;

            var bp = B.ColumnPointers;
            var bj = B.RowIndices;
            var bx = B.Values;

            int rowsA = A.RowCount;
            int rowsB = B.RowCount;

            var counts = new int[rowsA * rowsB];

            int k = 0;

            // Count non-zeros in each row of kron(A, B).
            for (int i = 0; i < rowsA; i++)
            {
                for (int j = 0; j < rowsB; j++)
                {
                    counts[k++] = (ap[i + 1] - ap[i]) * (bp[j + 1] - bp[j]);
                }
            }

            int colsA = A.ColumnCount;
            int colsB = B.ColumnCount;

            var C = new SparseMatrix(rowsA * rowsB, colsA * colsB);

            C.ColumnPointers = new int[colsA * colsB + 1];

            int nnz = CumulativeSum(C.ColumnPointers, counts, counts.Length);

            var cj = C.RowIndices = new int[nnz];
            var cx = C.Values = new Complex[nnz];

            k = 0;

            // For each row in A ...
            for (int ia = 0; ia < rowsA; ia++)
            {
                // ... and each row in B ...
                for (int ib = 0; ib < rowsB; ib++)
                {
                    // ... get element a_{ij}
                    for (int j = ap[ia]; j < ap[ia + 1]; j++)
                    {
                        var idx = aj[j];
                        var aij = ax[j];

                        // ... and multiply it with current row of B
                        for (int s = bp[ib]; s < bp[ib + 1]; s++)
                        {
                            cj[k] = (idx * colsB) + bj[s];
                            cx[k] = aij * bx[s];
                            k++;
                        }
                    }
                }
            }

            return C;
        }

        private static int CumulativeSum(int[] sum, int[] counts, int size)
        {
            int i, nz = 0;

            for (i = 0; i < size; i++)
            {
                sum[i] = nz;
                nz += counts[i];
                counts[i] = sum[i]; // also copy p[0..n-1] back into c[0..n-1]
            }

            sum[size] = nz;

            return nz;
        }

        #endregion
    }
}
