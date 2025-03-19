namespace CSparse.Storage
{
    using System;
    using System.Collections.Generic;

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
        /// Remove all non-zero entries in the lower part of the matrix.
        /// </summary>
        /// <param name="matrix">The matrix.</param>
        /// <param name="clone">If true, return a clone of the original matrix, otherwise modify given instance in place.</param>
        public static CompressedColumnStorage<T> ToUpper<T>(this CompressedColumnStorage<T> matrix, bool clone = true)
            where T : struct, IEquatable<T>, IFormattable
        {
            var A = clone ? matrix.Clone() : matrix;

            A.Keep((i, j, _) => i <= j);

            return A;
        }

        /// <summary>
        /// Remove all non-zero entries in the lower part of the matrix.
        /// </summary>
        /// <param name="matrix">The matrix.</param>
        /// <param name="clone">If true, return a clone of the original matrix, otherwise modify given instance in place.</param>
        public static CompressedColumnStorage<T> ToLower<T>(this CompressedColumnStorage<T> matrix, bool clone = true)
            where T : struct, IEquatable<T>, IFormattable
        {
            var A = clone ? matrix.Clone() : matrix;

            A.Keep((i, j, _) => i >= j);

            return A;
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
        /// Enumerate all matrix entries that match the given predicate.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="matrix">The matrix.</param>
        /// <param name="predicate">The predicate.</param>
        /// <returns></returns>
        public static IEnumerable<Tuple<int, int, T>> EnumerateIndexed<T>(this CompressedColumnStorage<T> matrix, Func<int, int, T, bool> predicate)
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
                        yield return new Tuple<int, int, T>(ai[j], i, ax[j]);
                    }
                }
            }
        }

        internal static CompressedColumnStorage<T> Create<T>(int rowCount, int columnCount, int valueCount)
            where T : struct, IEquatable<T>, IFormattable
        {
            if (typeof(T) == typeof(double))
            {
                return new CSparse.Double.SparseMatrix(rowCount, columnCount, valueCount)
                    as CompressedColumnStorage<T>;
            }

            if (typeof(T) == typeof(System.Numerics.Complex))
            {
                return new CSparse.Complex.SparseMatrix(rowCount, columnCount, valueCount)
                    as CompressedColumnStorage<T>;
            }

            throw new NotSupportedException();
        }
    }
}
