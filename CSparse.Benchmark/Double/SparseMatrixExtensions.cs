
namespace CSparse.Double
{
    using System;
    using System.Numerics;

    static class SparseMatrixExtensions
    {
        /// <summary>
        /// Counts all matrix entries that match the predicate.
        /// </summary>
        /// <param name="func">Predicate function.</param>
        /// <returns></returns>
        public static int Count(this SparseMatrix A, Func<int, int, double, bool> func)
        {
            var ax = A.Values;
            var ap = A.ColumnPointers;
            var ai = A.RowIndices;

            int rowCount = A.RowCount;
            int columnCount = A.ColumnCount;

            int end, count = 0;

            for (int j = 0; j < columnCount; j++)
            {
                end = ap[j + 1];
                
                for (int i = ap[j]; i < end; i++)
                {
                    if (func(ai[i], j, ax[i]))
                    {
                        count++;
                    }
                }
            }

            return count;
        }

        /// <summary>
        /// Check if any matrix entry matches the predicate.
        /// </summary>
        /// <param name="func">Predicate function.</param>
        /// <returns></returns>
        public static bool Any(this SparseMatrix A, Func<int, int, double, bool> func)
        {
            var ax = A.Values;
            var ap = A.ColumnPointers;
            var ai = A.RowIndices;

            int rowCount = A.RowCount;
            int columnCount = A.ColumnCount;
            
            for (int j = 0; j < columnCount; j++)
            {
                int end = ap[j + 1];

                for (int i = ap[j]; i < end; i++)
                {
                    if (func(ai[i], j, ax[i]))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Remove all non-zero entries in the lower part of the matrix.
        /// </summary>
        /// <param name="clone">If true, return a clone of the original matrix, otherwise modify given instance in place.</param>
        public static SparseMatrix ToUpper(this SparseMatrix A, bool clone = true)
        {
            var B = clone ? (SparseMatrix)A.Clone() : A;

            B.Keep((i, j, _) => i <= j);

            return B;
        }

        /// <summary>
        /// Expand matrix to full storage (symmetric matrix is expected to store upper part only).
        /// </summary>
        public static SparseMatrix Expand(this SparseMatrix A)
        {
            if (A.Any((i, j, a) => i > j && a != 0.0))
            {
                throw new Exception("Expected matrix to be upper.");
            }

            // Transpose A.
            var T = A.Transpose();

            // Remove diagonal.
            T.Keep((i, j, a) => i != j);

            return (SparseMatrix)A.Add(T);
        }

        /// <summary>
        /// Multiplies a real-valued (m-by-n) matrix by a complex vector, y = A*x. 
        /// </summary>
        /// <param name="x">Vector of length n (column count).</param>
        /// <param name="y">Vector of length m (row count), containing the result.</param>
        public static void Multiply(this SparseMatrix A, Complex[] x, Complex[] y)
        {
            var ax = A.Values;
            var ap = A.ColumnPointers;
            var ai = A.RowIndices;

            int rowCount = A.RowCount;
            int columnCount = A.ColumnCount;

            for (int j = 0; j < rowCount; j++)
            {
                y[j] = Complex.Zero;
            }

            int end;
            Complex xi;

            for (int i = 0; i < columnCount; i++)
            {
                xi = x[i];

                end = ap[i + 1];

                for (int k = ap[i]; k < end; k++)
                {
                    y[ai[k]] += ax[k] * xi;
                }
            }
        }

        /// <summary>
        /// Multiplies a real-valued (m-by-n) matrix by a complex vector, y = alpha * A * x + beta * y.
        /// </summary>
        /// <param name="alpha">Scaling factor fo vertor x.</param>
        /// <param name="x">Vector of length n (column count).</param>
        /// <param name="beta">Scaling factor fo vertor y.</param>
        /// <param name="y">Vector of length m (row count), containing the result.</param>
        public static void Multiply(this SparseMatrix A, Complex alpha, Complex[] x, Complex beta, Complex[] y)
        {
            var ax = A.Values;
            var ap = A.ColumnPointers;
            var ai = A.RowIndices;

            int rowCount = A.RowCount;
            int columnCount = A.ColumnCount;

            // Scale y by beta
            for (int j = 0; j < rowCount; j++)
            {
                y[j] = beta * y[j];
            }

            int end;
            Complex xi;

            for (int i = 0; i < columnCount; i++)
            {
                xi = alpha * x[i];

                end = ap[i + 1];

                for (int k = ap[i]; k < end; k++)
                {
                    y[ai[k]] += ax[k] * xi;
                }
            }
        }
    }
}
