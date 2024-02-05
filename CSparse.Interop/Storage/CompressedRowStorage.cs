using System;

namespace CSparse.Storage
{
    public class CompressedRowStorage<T> : ILinearOperator<T> where T : struct, IEquatable<T>, IFormattable
    {
        /// <summary>The number of rows.</summary>
        protected readonly int rows;

        /// <summary>The number of columns.</summary>
        protected readonly int columns;

        /// <inheritdoc />
        public int RowCount => rows;

        /// <inheritdoc />
        public int ColumnCount => columns;

        /// <summary>
        /// Gets the number of non-zero entries.
        /// </summary>
        public int NonZerosCount => RowPointers[rows];

        /// <summary>
        /// Row pointers with last entry equal number of non-zeros (size = RowCount + 1)
        /// </summary>
        public int[] RowPointers;

        /// <summary>
        /// Column indices (size >= NonZerosCount)
        /// </summary>
        public int[] ColumnIndices;

        /// <summary>
        /// Numerical values (size >= NonZerosCount)
        /// </summary>
        public T[] Values;

        /// <summary>
        /// Initializes a new instance of the <see cref="CompressedRowStorage{T}"/> class.
        /// </summary>
        public CompressedRowStorage(CompressedColumnStorage<T> other)
        {
            rows = other.RowCount;
            columns = other.ColumnCount;

            var A = other.Transpose();

            RowPointers = A.ColumnPointers;
            ColumnIndices = A.RowIndices;
            Values = A.Values;
        }

        /// <inheritdoc />
        public void Multiply(T[] x, T[] y) => Multiply(x.AsSpan(), y.AsSpan());

        /// <inheritdoc />
        public void Multiply(ReadOnlySpan<T> x, Span<T> y)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public void Multiply(T alpha, T[] x, T beta, T[] y) => Multiply(alpha, x.AsSpan(), beta, y.AsSpan());

        /// <inheritdoc />
        public void Multiply(T alpha, ReadOnlySpan<T> x, T beta, Span<T> y)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public void TransposeMultiply(T[] x, T[] y) => TransposeMultiply(x.AsSpan(), y.AsSpan());

        /// <inheritdoc />
        public void TransposeMultiply(ReadOnlySpan<T> x, Span<T> y)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public void TransposeMultiply(T alpha, T[] x, T beta, T[] y) => TransposeMultiply(alpha, x.AsSpan(), beta, y.AsSpan());

        /// <inheritdoc />
        public void TransposeMultiply(T alpha, ReadOnlySpan<T> x, T beta, Span<T> y)
        {
            throw new NotImplementedException();
        }
    }
}