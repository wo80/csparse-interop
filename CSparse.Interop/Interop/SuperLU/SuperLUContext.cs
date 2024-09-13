
namespace CSparse.Interop.SuperLU
{
    using CSparse.Factorization;
    using CSparse.Interop.Common;
    using CSparse.Storage;
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;

    /// <summary>
    /// SuperLU context wrapping native factorization.
    /// </summary>
    public abstract class SuperLUContext<T> : IDisposableSolver<T>
        where T : struct, IEquatable<T>, IFormattable
    {
        protected readonly CompressedColumnStorage<T> matrix;

        protected SuperMatrix A;
        protected SuperMatrix L;
        protected SuperMatrix U;
        protected int[] perm_c;
        protected int[] perm_r;
        protected int[] etree;
        protected object R, C; // float or double arrays for scaling
        protected byte[] equed;

        // Contains handles to pinned objects associated with the factorization.
        protected List<GCHandle> handles;

        internal GlobalLU glu;
        internal SuperLUOptions options;

        protected double rpg, rcond;

        protected bool factorized;

        /// <summary>
        /// Gets the SuperLU options.
        /// </summary>
        public SuperLUOptions Options => options;

        /// <inheritdoc />
        public int NonZerosCount => GetNonZerosCount();

        public SuperLUContext(CompressedColumnStorage<T> matrix)
        {
            handles = new List<GCHandle>();

            int m = matrix.RowCount;
            int n = matrix.ColumnCount;

            this.matrix = matrix;

            A = CreateSparse(matrix, handles);
            L = new SuperMatrix();
            U = new SuperMatrix();

            glu = new GlobalLU();

            perm_c = new int[n];
            perm_r = new int[m];
            etree = new int[n];

            R = CreateArray(m);
            C = CreateArray(n);

            equed = new byte[2];

            options = new SuperLUOptions();
        }
        
        ~SuperLUContext()
        {
            Dispose(false);
        }

        /// <summary>
        /// Factorizes the matrix associated to this UMFPACK instance.
        /// </summary>
        public void Factorize()
        {
            int status = DoFactorize();

            if (status != 0)
            {
                throw new SuperLUException(status, matrix.ColumnCount);
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
        /// Solves a system of linear equations for multiple right-hand sides, AX = B.
        /// </summary>
        /// <param name="input">Right hand side matrix B.</param>
        /// <param name="result">Solution matrix X.</param>
        public void Solve(DenseColumnMajorStorage<T> input, DenseColumnMajorStorage<T> result)
        {
            if (!factorized)
            {
                Factorize();
            }

            // The number of right-hand sides.
            int count = input.ColumnCount;

            if (count != result.ColumnCount)
            {
                throw new ArgumentException("result");
            }

            int status = DoSolve(input, result);

            if (status != 0)
            {
                throw new SuperLUException(status, matrix.ColumnCount);
            }
        }

        /// <summary>
        /// Create single or double-precision floating point arrays.
        /// </summary>
        /// <param name="size">The array size.</param>
        /// <remarks>
        /// At the moment, only double-precision is implemented.
        /// </remarks>
        protected abstract object CreateArray(int size);

        protected abstract SuperMatrix CreateSparse(CompressedColumnStorage<T> matrix, List<GCHandle> handles);

        protected SuperMatrix CreateDense(Dtype type, DenseColumnMajorStorage<T> matrix, List<GCHandle> handles)
        {
            var A = new SuperMatrix();

            A.nrow = matrix.RowCount;
            A.ncol = matrix.ColumnCount;

            A.Dtype = type;
            A.Mtype = Mtype.SLU_GE;
            A.Stype = Stype.SLU_DN;

            var store = new DNformat();

            store.lda = matrix.RowCount;
            store.nzval = InteropHelper.Pin(matrix.Values, handles);

            A.Store = InteropHelper.Pin(store, handles);

            return A;
        }

        protected SuperMatrix CreateDense(Dtype type, T[] vector, List<GCHandle> handles)
        {
            var A = new SuperMatrix();

            A.nrow = vector.Length;
            A.ncol = 1;

            A.Dtype = type;
            A.Mtype = Mtype.SLU_GE;
            A.Stype = Stype.SLU_DN;

            var store = new DNformat();

            store.lda = vector.Length;
            store.nzval = InteropHelper.Pin(vector, handles);

            A.Store = InteropHelper.Pin(store, handles);

            return A;
        }

        protected SuperMatrix CreateEmptyDense(Dtype type, int size, List<GCHandle> handles)
        {
            var A = new SuperMatrix();

            A.nrow = size;
            A.ncol = 0;

            A.Dtype = type;
            A.Mtype = Mtype.SLU_GE;
            A.Stype = Stype.SLU_DN;

            var store = new DNformat();

            store.lda = size;
            store.nzval = IntPtr.Zero;

            A.Store = InteropHelper.Pin(store, handles);

            return A;
        }

        /// <summary>
        /// Do symbolic and numeric factorization for current type.
        /// </summary>
        protected abstract int DoFactorize();

        /// <summary>
        /// Solve system of linear equations.
        /// </summary>
        /// <param name="input">Right-hand side b.</param>
        /// <param name="result">The solution x.</param>
        /// <returns></returns>
        protected abstract int DoSolve(DenseColumnMajorStorage<T> input, DenseColumnMajorStorage<T> result);

        private int GetNonZerosCount()
        {
            int lnz = GetNonZerosCount(L);
            int unz = GetNonZerosCount(U);

            return lnz + unz;
        }

        private static int GetNonZerosCount(SuperMatrix mat)
        {
            if (mat.Store == IntPtr.Zero)
            {
                return -1;
            }

            switch (mat.Stype)
            {
                case Stype.SLU_NC:
                case Stype.SLU_NCP:
                    var nc = Marshal.PtrToStructure<NCformat>(mat.Store);
                    return nc.nnz;
                case Stype.SLU_NR:
                    var nr = Marshal.PtrToStructure<NRformat>(mat.Store);
                    return nr.nnz;
                case Stype.SLU_SC:
                case Stype.SLU_SCP:
                    var sc = Marshal.PtrToStructure<SCformat>(mat.Store);
                    return sc.nnz;
                case Stype.SLU_DN:
                case Stype.SLU_NR_loc:
                case Stype.SLU_SR:
                default:
                    break;
            }

            return -1;
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

            if (L.Store != IntPtr.Zero)
            {
                NativeMethods.Destroy_SuperNode_Matrix(ref L);
                L.Store = IntPtr.Zero;
            }

            if (U.Store != IntPtr.Zero)
            {
                NativeMethods.Destroy_CompCol_Matrix(ref U);
                U.Store = IntPtr.Zero;
            }
        }

        #endregion
    }
}
