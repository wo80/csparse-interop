
namespace CSparse.Interop.MKL.SparseQR
{
    using CSparse.Factorization;
    using CSparse.Interop.Common;
    using CSparse.Storage;
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;

    // See https://www.intel.com/content/www/us/en/docs/onemkl/developer-reference-c/2024-0/mkl-sparse-qr.html
    //
    // * Currently, mkl_sparse_?_qr supports only square and overdetermined systems. For underdetermined systems you can manually transpose
    //   the system matrix and use QR decomposition for AT to get the minimum-norm solution for the original underdetermined system.
    // * Currently, mkl_sparse_?_qr supports only CSR format for the input matrix, non - transpose operation, and single right - hand side.
    // * Currently, the only supported value is SPARSE_OPERATION_NON_TRANSPOSE (non-transpose case; that is, A*x = b is solved).
    // * Currently, the only supported value is SPARSE_MATRIX_TYPE_GENERAL (the matrix is processed as-is).

    public class SparseQRContext<T> : IDisposableSolver<T>
        where T : struct, IEquatable<T>, IFormattable
    {
        protected readonly CompressedRowStorage<T> matrix;

        internal MatrixDescriptor descr;

        protected IntPtr csrA;

        protected bool factorized;

        // Contains handles to pinned objects associated with the factorization.
        protected List<GCHandle> handles;

        /// <inheritdoc />
        public int NonZerosCount => -1;

        public SparseQRContext(CompressedRowStorage<T> matrix)
        {
            if (matrix.RowCount < matrix.ColumnCount)
            {
                throw new NotSupportedException("MKL sparse QR does not support underdetermined systems.");
            }

            handles = new List<GCHandle>();

            this.matrix = matrix;
        }

        // abstract
        protected virtual SparseStatus DoInitialize(CompressedRowStorage<T> matrix)
        {
            throw new NotImplementedException();
        }

        protected virtual SparseStatus DoFactorize()
        {
            throw new NotImplementedException();
        }

        protected virtual SparseStatus DoSolve(IntPtr A, int columns, IntPtr x,  int ldx, IntPtr b, int ldb)
        {
            throw new NotImplementedException();
        }

        protected SparseStatus DoReorder()
        {
            return NativeMethods.mkl_sparse_qr_reorder(csrA, descr);
        }

        public void Solve(DenseColumnMajorStorage<T> input, DenseColumnMajorStorage<T> result)
        {
            if (!factorized)
            {
                if (DoInitialize(matrix) != SparseStatus.Success) throw new Exception("DoInitialize");
                if (DoReorder() != SparseStatus.Success) throw new Exception("DoReorder");
                if (DoFactorize() != SparseStatus.Success) throw new Exception("DoFactorize");
            }

            int ldx = result.RowCount;
            int ldb = input.RowCount;
            int columns = input.ColumnCount;

            var h = new List<GCHandle>();

            try
            {
                var b = InteropHelper.Pin(input.Values, h);
                var x = InteropHelper.Pin(result.Values, h);

                DoSolve(csrA, columns, x, ldx, b, ldb);
            }
            finally
            {
                InteropHelper.Free(h);
            }
        }

        /// <inheritdoc />
        public virtual void Solve(T[] input, T[] result)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public virtual void Solve(ReadOnlySpan<T> input, Span<T> result)
        {
            throw new NotImplementedException();
        }

        #region IDisposable

        // See https://docs.microsoft.com/en-us/dotnet/standard/garbage-collection/implementing-dispose

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                InteropHelper.Free(handles);
            }

            if (csrA != IntPtr.Zero)
            {
                NativeMethods.mkl_sparse_destroy(csrA);
                csrA = IntPtr.Zero;
            }
        }

        #endregion
    }
}
