
namespace CSparse.Double.Examples
{
    using CSparse.Double;
    using System;
    using System.Numerics;

    static class SparseMatrixExtensions
    {
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
