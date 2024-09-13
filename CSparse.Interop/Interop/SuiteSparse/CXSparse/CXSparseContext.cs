namespace CSparse.Interop.SuiteSparse.CXSparse
{
    using CSparse.Factorization;
    using CSparse.Interop.Common;
    using CSparse.Storage;
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;

    /// <summary>
    /// CXSparse context wrapping native factorization.
    /// </summary>
    public abstract class CXSparseContext<T> : IDisposableSolver<T>
        where T : struct, IEquatable<T>, IFormattable
    {
        protected readonly CompressedColumnStorage<T> matrix;
        protected readonly ColumnOrdering ordering;

        protected T[] w; // workspace for solve

        internal csn N;
        internal css S;

        protected bool factorized;

        // Contains handles to pinned objects associated with the factorization.
        protected List<GCHandle> handles;

        /// <inheritdoc />
        public int NonZerosCount => GetNonZerosCount();

        /// <summary>
        /// Return the CXSparse version.
        /// </summary>
        /// <returns>The CXSparse version.</returns>
        public static Version Version()
        {
            int[] version = new int[3];
            NativeMethods.cxsparse_version(version);
            return new Version(version[0], version[1], version[2]);
        }

        public CXSparseContext(CompressedColumnStorage<T> matrix, ColumnOrdering ordering)
        {
            handles = new List<GCHandle>();

            this.matrix = matrix;
            this.ordering = ordering;

            // Make sure to have enough workspace for QR.
            int n = Math.Max(matrix.RowCount, matrix.ColumnCount);

            w = new T[n];
        }

        ~CXSparseContext()
        {
            Dispose(false);
        }

        /// <summary>
        /// Factorizes the matrix associated to this CXSparse context.
        /// </summary>
        public void Factorize()
        {
            int status = DoFactorize();

            if (status != 0)
            {
                throw new Exception(); // TODO: exception
            }

            factorized = true;
        }

        /// <inheritdoc />
        public abstract void Solve(T[] input, T[] result);

        /// <inheritdoc />
        public void Solve(ReadOnlySpan<T> input, Span<T> result)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Do symbolic and numeric factorization for current type.
        /// </summary>
        protected abstract int DoFactorize();

        internal cs CreateSparse(CompressedColumnStorage<T> matrix, List<GCHandle> handles)
        {
            var A = new cs();

            A.m = matrix.RowCount;
            A.n = matrix.ColumnCount;
            A.nz = -1; // CSC format.
            A.nzmax = matrix.NonZerosCount;
            A.p = InteropHelper.Pin(matrix.ColumnPointers, handles);
            A.i = InteropHelper.Pin(matrix.RowIndices, handles);
            A.x = InteropHelper.Pin(matrix.Values, handles);

            return A;
        }

        private int GetNonZerosCount()
        {
            int lnz = GetNonZerosCount(N.L);
            int unz = GetNonZerosCount(N.U);

            return lnz + unz;
        }

        private static int GetNonZerosCount(IntPtr mat)
        {
            if (mat == IntPtr.Zero)
            {
                return -1;
            }

            var cs_obj = Marshal.PtrToStructure<cs>(mat);

            return cs_obj.nzmax;
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

            // NOTE: cs_di_*free and cs_ci_*free both do the same, so there's
            //       no need to move this method to the derived classes.

            if (N.L != IntPtr.Zero)
            {
                cs_nfree(ref N);
                N.L = IntPtr.Zero;
            }

            if (S.pinv != IntPtr.Zero || S.q != IntPtr.Zero)
            {
                cs_sfree(ref S);
                S.pinv = IntPtr.Zero;
                S.q = IntPtr.Zero;
            }
        }

        /* free a numeric factorization */
        internal void cs_nfree(ref csn N)
        {
            NativeMethods.cs_di_spfree(N.L);
            NativeMethods.cs_di_spfree(N.U);
            NativeMethods.cs_di_free(N.pinv);
            NativeMethods.cs_di_free(N.B);
        }

        /* free a symbolic factorization */
        internal void cs_sfree(ref css S)
        {
            NativeMethods.cs_di_free(S.pinv);
            NativeMethods.cs_di_free(S.q);
            NativeMethods.cs_di_free(S.parent);
            NativeMethods.cs_di_free(S.cp);
            NativeMethods.cs_di_free(S.leftmost);
        }

        /* free a sparse matrix */
        internal void cs_spfree(ref cs A)
        {
            NativeMethods.cs_di_free(A.p);
            NativeMethods.cs_di_free(A.i);
            NativeMethods.cs_di_free(A.x);
        }

        #endregion
    }
}
