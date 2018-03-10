namespace CSparse.Interop.Cholmod
{
    using CSparse.Factorization;
    using CSparse.Interop.Common;
    using CSparse.Storage;
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;

    /// <summary>
    /// CHOLMOD context wrapping native factorization.
    /// </summary>
    public abstract class CholmodContext<T> : IDisposable, ISolver<T>
        where T : struct, IEquatable<T>, IFormattable
    {
        protected readonly CompressedColumnStorage<T> matrix;

        // Contains handles to pinned objects associated with above matrix storage.
        protected List<GCHandle> handles;

        protected bool factorized;

        protected CholmodCommon common;

        protected CholmodSparse A;
        protected CholmodFactor L;

        // Stores a pointer to cholmod_factor (native memory). Needed for disposing with cholmod_free_factor.
        private IntPtr Lp;

        /// <summary>
        /// Return the CHOLMOD version.
        /// </summary>
        /// <returns>The CHOLMOD version</returns>
        public static Version Version()
        {
            int[] version = new int[3];

            int i = NativeMethods.cholmod_version(version);

            return new Version(version[0], version[1], version[2]);
        }

        /// <summary>
        /// Initializes a new instance of the CholmodContext class.
        /// </summary>
        /// <param name="matrix">The sparse matrix to factorize.</param>
        public CholmodContext(CompressedColumnStorage<T> matrix)
        {
            this.matrix = matrix;
            this.handles = new List<GCHandle>();

            common = new CholmodCommon();

            NativeMethods.cholmod_start(ref common);
        }

        ~CholmodContext()
        {
            Dispose(false);
        }

        /// <summary>
        /// Factorizes the matrix associated to this CHOLMOD instance.
        /// </summary>
        public void Factorize()
        {
            if (DoFactorize() != 1)
            {
                throw new CholmodException(common.status);
            }

            factorized = true;
        }

        /// <summary>
        /// Solves a system of linear equations, Ax = b.
        /// </summary>
        /// <param name="input">Right hand side b</param>
        /// <param name="result">Solution vector x.</param>
        public abstract void Solve(T[] input, T[] result);

        /// <summary>
        /// Solves the transpose system of linear equations, A'x = b.
        /// </summary>
        /// <param name="input">Right hand side b</param>
        /// <param name="result">Solution vector x.</param>
        public abstract void SolveTranspose(T[] input, T[] result);

        /// <summary>
        /// Solves multiple systems of linear equations, AX = B.
        /// </summary>
        /// <param name="input">Right hand side B</param>
        /// <param name="result">Solution matrix X.</param>
        public virtual void Solve(DenseColumnMajorStorage<T> input, DenseColumnMajorStorage<T> result)
        {
            if (!factorized)
            {
                Factorize();
            }

            if (DoSolve(CholmodSolve.A, input, result) != 1)
            {
                throw new CholmodException(common.status);
            }
        }

        /// <summary>
        /// Do symbolic and numeric factorization.
        /// </summary>
        protected virtual int DoFactorize()
        {
            A = CreateSparse(matrix, handles);

            if (NativeMethods.cholmod_check_sparse(ref A, ref common) != 1)
            {
                return -1;
            }

            Lp = NativeMethods.cholmod_analyze(ref A, ref common);

            L = (CholmodFactor)Marshal.PtrToStructure(Lp, typeof(CholmodFactor));

            return NativeMethods.cholmod_factorize(ref A, ref L, ref common);
        }

        /// <summary>
        /// Solve multiple systems of linear equations.
        /// </summary>
        /// <param name="sys">The system to solve.</param>
        /// <param name="input">Right hand side B</param>
        /// <param name="result">Solution matrix X.</param>
        protected virtual int DoSolve(CholmodSolve sys, DenseColumnMajorStorage<T> input, DenseColumnMajorStorage<T> result)
        {
            var h = new List<GCHandle>();

            try
            {
                var B = CreateDense(input, h);

                var ptr = NativeMethods.cholmod_solve((int)sys, ref L, ref B, ref common);

                if (common.status != Constants.CHOLMOD_OK)
                {
                    throw new CholmodException(common.status);
                }

                var X = (CholmodDense)Marshal.PtrToStructure(ptr, typeof(CholmodDense));

                CopyDense(X, result);

                NativeMethods.cholmod_free_dense(ref ptr, ref common);

                return 1;
            }
            finally
            {
                InteropHelper.Free(h);
            }
        }

        /// <summary>
        /// Copy native memory to dense matrix.
        /// </summary>
        /// <param name="dense">CHOLMOD dense matrix.</param>
        /// <param name="matrix">Target storage.</param>
        protected abstract void CopyDense(CholmodDense dense, DenseColumnMajorStorage<T> matrix);

        /// <summary>
        /// Create CHOLMOD dense matrix from managed type.
        /// </summary>
        /// <param name="matrix">The source matrix.</param>
        /// <param name="handles">List of handles.</param>
        /// <returns></returns>
        protected abstract CholmodDense CreateDense(DenseColumnMajorStorage<T> matrix, List<GCHandle> handles);

        /// <summary>
        /// Create CHOLMOD sparse matrix from managed type.
        /// </summary>
        /// <param name="matrix">The source matrix.</param>
        /// <param name="handles">List of handles.</param>
        /// <returns></returns>
        protected abstract CholmodSparse CreateSparse(CompressedColumnStorage<T> matrix, List<GCHandle> handles);

        #region IDisposable

        // See https://msdn.microsoft.com/de-de/library/ms244737.aspx

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                InteropHelper.Free(handles);
            }

            if (Lp != IntPtr.Zero)
            {
                NativeMethods.cholmod_free_factor(ref Lp, ref common);

                NativeMethods.cholmod_finish(ref common);

                L = default(CholmodFactor);
            }
        }

        #endregion
    }
}
