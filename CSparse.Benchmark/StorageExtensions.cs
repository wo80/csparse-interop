namespace CSparse
{
    using CSparse.Storage;
    using System;

    /// <summary>
    /// CompressedColumnStorage extension methods.
    /// </summary>
    public static class CompressedColumnStorageExtensions
    {
        /// <summary>
        /// Test if any matrix entry satisfies the given predicate.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="matrix">The matrix.</param>
        /// <param name="predicate">The predicate.</param>
        /// <returns></returns>
        public static bool Any<T>(this CompressedColumnStorage<T> matrix, Func<int, int, T, bool> predicate)
            where T : struct, IEquatable<T>, IFormattable
        {
            int columns = matrix.ColumnCount;

            var ax = matrix.Values;
            var ap = matrix.ColumnPointers;
            var ai = matrix.RowIndices;

            for (int i = 0; i < columns; i++)
            {
                int end = ap[i + 1];
                for (int j = ap[i]; j < end; j++)
                {
                    if (predicate(ai[j], i, ax[j]))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Test whether the matrix is upper triangular.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="matrix">The matrix.</param>
        /// <param name="strict">If true, no diagonal entries are allowed.</param>
        /// <returns></returns>
        public static bool IsUpper<T>(this CompressedColumnStorage<T> matrix, bool strict = false)
            where T : struct, IEquatable<T>, IFormattable
        {
            return strict ? !Any(matrix, (i, j, a) => i >= j) : !Any(matrix, (i, j, a) => i > j);
        }

        /// <summary>
        /// Test whether the matrix is lower triangular.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="matrix">The matrix.</param>
        /// <param name="strict">If true, no diagonal entries are allowed.</param>
        /// <returns></returns>
        public static bool IsLower<T>(this CompressedColumnStorage<T> matrix, bool strict = false)
            where T : struct, IEquatable<T>, IFormattable
        {
            return strict ? !Any(matrix, (i, j, a) => i <= j) : !Any(matrix, (i, j, a) => i < j);
        }

        /// <summary>
        /// Expand matrix to full storage (symmetric matrix stores upper part only).
        /// </summary>
        public static CompressedColumnStorage<T> Expand<T>(this CompressedColumnStorage<T> A)
            where T : struct, IEquatable<T>, IFormattable
        {
            if (A.IsUpper() || A.IsLower())
            {
                // Transpose A.
                var B = A.Transpose();

                // Remove diagonal.
                B.Keep((i, j, a) => i != j);

                return A.Add(B);
            }

            return A;
        }
    }
}
