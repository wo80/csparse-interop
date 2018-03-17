
namespace CSparse.Interop.Pardiso
{
    using CSparse.Factorization;
    using CSparse.Interop.Common;
    using CSparse.Storage;
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;

    public abstract class PardisoContext<T> : IDisposableSolver<T>
        where T : struct, IEquatable<T>, IFormattable
    {
        protected readonly CompressedColumnStorage<T> matrix;

        protected int mtype;

        // The first parameter of the PARDISO routine: typedef void * _MKL_DSS_HANDLE_t
        protected IntPtr[] pt;

        protected PardisoOptions options;

        protected bool factorized;

        protected int[] perm;

        // Maximum number of numerical factorizations.
        protected int maxfct = 1;

        // Which factorization to use.
        protected int mnum = 1;

        // Print statistical information.
        protected int msglvl = 0;

        /// <summary>
        /// Gets the PARDISO options.
        /// </summary>
        public PardisoOptions Options { get { return options; } }

        public PardisoContext(CompressedColumnStorage<T> matrix, int mtype)
        {
            this.matrix = matrix;
            this.mtype = mtype;

            this.pt = new IntPtr[64];
            this.options = new PardisoOptions();

            // User supplied permutation - not used.
            this.perm = null;

            DoInitialize();
        }
        
        ~PardisoContext()
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
                throw new PardisoException(status);
            }

            factorized = true;
        }

        /// <summary>
        /// Solves a system of linear equations, Ax = b.
        /// </summary>
        /// <param name="input">Right hand side vector b.</param>
        /// <param name="result">Solution vector x.</param>
        public void Solve(T[] input, T[] result)
        {
            // NOTE: use conjugate-transposed, since PARDISO expects CSR storage, but CSparse uses CSC storage.
            Solve(Constants.ConjugateTransposed, input, result);
        }

        /// <summary>
        /// Solves the transpose system of linear equations, A'x = b.
        /// </summary>
        /// <param name="input">Right hand side vector b</param>
        /// <param name="result">Solution vector x.</param>
        public void SolveTranspose(T[] input, T[] result)
        {
            Solve(Constants.NonTransposed, input, result);
        }

        /// <summary>
        /// Solves a system of linear equations for multiple right-hand sides, AX = B.
        /// </summary>
        /// <param name="input">Right hand side matrix B.</param>
        /// <param name="result">Solution matrix X.</param>
        public void Solve(DenseColumnMajorStorage<T> input, DenseColumnMajorStorage<T> result)
        {
            Solve(Constants.ConjugateTransposed, input, result);
        }

        protected void Solve(int sys, DenseColumnMajorStorage<T> input, DenseColumnMajorStorage<T> result)
        {
            if (!factorized)
            {
                Factorize();
            }

            int status = DoSolve(sys, input, result);

            if (status != 0)
            {
                throw new PardisoException(status);
            }
        }

        protected abstract void Solve(int sys, T[] input, T[] result);
        
        /// <summary>
        /// Do initialization for current type.
        /// </summary>
        protected virtual void DoInitialize()
        {
            NativeMethods.pardisoinit(pt, ref mtype, options.iparm);
        }

        /// <summary>
        /// Do symbolic factorization for current type.
        /// </summary>
        protected virtual int DoSymbolic()
        {
            int n = matrix.ColumnCount;
            int nrhs = 1;

            int error = 0;
            var iparm = options.iparm;

            var h = new List<GCHandle>();

            try
            {
                var a = InteropHelper.Pin(matrix.Values, h);
                var ia = InteropHelper.Pin(matrix.ColumnPointers, h);
                var ja = InteropHelper.Pin(matrix.RowIndices, h);

                int phase = 11;

                // Reordering and Symbolic Factorization. This step also allocates
                // all memory that is necessary for the factorization.
                NativeMethods.pardiso(pt, ref maxfct, ref mnum, ref mtype, ref phase,
                             ref n, a, ia, ja, perm, ref nrhs, iparm, ref msglvl,
                             IntPtr.Zero, IntPtr.Zero, out error);
            }
            finally
            {
                InteropHelper.Free(h);
            }

            return error;
        }

        /// <summary>
        /// Do numeric factorization for current type.
        /// </summary>
        protected virtual int DoNumeric()
        {
            int n = matrix.ColumnCount;
            int nrhs = 1;

            int error = 0;
            var iparm = options.iparm;

            var h = new List<GCHandle>();

            try
            {
                var a = InteropHelper.Pin(matrix.Values, h);
                var ia = InteropHelper.Pin(matrix.ColumnPointers, h);
                var ja = InteropHelper.Pin(matrix.RowIndices, h);

                int phase = 22;

                // Numerical factorization.
                NativeMethods.pardiso(pt, ref maxfct, ref mnum, ref mtype, ref phase,
                             ref n, a, ia, ja, perm, ref nrhs, iparm, ref msglvl,
                             IntPtr.Zero, IntPtr.Zero, out error);
            }
            finally
            {
                InteropHelper.Free(h);
            }

            return error;
        }
        
        /// <summary>
        /// Do symbolic and numeric factorization for current type.
        /// </summary>
        protected virtual int DoFactorize()
        {
            int n = matrix.ColumnCount;
            int nrhs = 1;

            int error = 0;
            var iparm = options.iparm;

            var h = new List<GCHandle>();

            try
            {
                var a = InteropHelper.Pin(matrix.Values, h);
                var ia = InteropHelper.Pin(matrix.ColumnPointers, h);
                var ja = InteropHelper.Pin(matrix.RowIndices, h);

                // Analysis and numerical factorization.
                int phase = 12;

                NativeMethods.pardiso(pt, ref maxfct, ref mnum, ref mtype, ref phase,
                             ref n, a, ia, ja, perm, ref nrhs, iparm, ref msglvl,
                             IntPtr.Zero, IntPtr.Zero, out error);
            }
            finally
            {
                InteropHelper.Free(h);
            }

            return error;
        }

        /// <summary>
        /// Solve system of linear equations.
        /// </summary>
        /// <param name="sys">The system to solve.</param>
        /// <param name="input">Right-hand side b.</param>
        /// <param name="result">The solution x.</param>
        /// <returns></returns>
        /// <remarks>
        /// Parameter sys corresponds to iparm[11]:
        ///    sys = 0: non-transposed
        ///    sys = 1: conjugate transposed
        ///    sys = 2: transposed
        /// </remarks>
        protected virtual int DoSolve(int sys, DenseColumnMajorStorage<T> input, DenseColumnMajorStorage<T> result)
        {
            int n = matrix.ColumnCount;

            // The number of right-hand sides.
            int nrhs = input.ColumnCount;
            
            if (nrhs != result.ColumnCount)
            {
                throw new ArgumentException("result");
            }

            int error = 0;
            var iparm = options.iparm;

            var h = new List<GCHandle>();

            try
            {
                var a = InteropHelper.Pin(matrix.Values, h);
                var ia = InteropHelper.Pin(matrix.ColumnPointers, h);
                var ja = InteropHelper.Pin(matrix.RowIndices, h);

                var b = InteropHelper.Pin(input.Values, h);
                var x = InteropHelper.Pin(result.Values, h);

                int phase = 33;

                iparm[11] = sys;

                // Reordering and Symbolic Factorization. This step also allocates
                // all memory that is necessary for the factorization.
                NativeMethods.pardiso(pt, ref maxfct, ref mnum, ref mtype, ref phase,
                             ref n, a, ia, ja, perm, ref nrhs, iparm, ref msglvl,
                             b, x, out error);
            }
            finally
            {
                InteropHelper.Free(h);
            }

            return error;
        }

        #region IDisposable

        // See https://docs.microsoft.com/en-us/dotnet/standard/garbage-collection/implementing-dispose

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected void Dispose(bool disposing)
        {
            if (pt != null)
            {
                int n = matrix.ColumnCount;
                int nrhs = 1;

                int error = 0;
                var iparm = options.iparm;

                var h = new List<GCHandle>();

                try
                {
                    var ia = InteropHelper.Pin(matrix.ColumnPointers, h);
                    var ja = InteropHelper.Pin(matrix.RowIndices, h);

                    // Release internal memory.
                    int phase = -1;

                    NativeMethods.pardiso(pt, ref maxfct, ref mnum, ref mtype, ref phase,
                                 ref n, IntPtr.Zero, ia, ja, null, ref nrhs, iparm, ref msglvl,
                                 IntPtr.Zero, IntPtr.Zero, out error);
                }
                finally
                {
                    InteropHelper.Free(h);
                }

                pt = null;
            }
        }

        #endregion
    }
}
