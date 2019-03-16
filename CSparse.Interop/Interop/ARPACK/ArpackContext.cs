
namespace CSparse.Interop.ARPACK
{
    using CSparse.Interop.Common;
    using CSparse.Storage;
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using System.Text;

    // TODO: no need for IDisposable at the moment.

    /// <summary>
    /// ARPACK eigenvalue solver.
    /// </summary>
    public abstract class ArpackContext<T>
        where T : struct, IEquatable<T>, IFormattable
    {
        // Sparse matrix (column compressed storage).
        protected readonly CompressedColumnStorage<T> A;

        // Sparse matrix for generalized problems.
        protected readonly CompressedColumnStorage<T> B;

        protected bool symmetric;

        protected int size;
        
        /// <summary>
        /// Gets or sets the number of Arnoldi vectors used in each iteration.
        /// </summary>
        public int ArnoldiCount { get; set; }

        /// <summary>
        /// Gets or sets the maximum number of iterations.
        /// </summary>
        public int Iterations { get; set; }

        /// <summary>
        /// Gets or sets the residual tolerance (if &lt;= 0, ARPACK will use machine epsilon).
        /// </summary>
        public double Tolerance { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to compute eigenvectors.
        /// </summary>
        public bool ComputeEigenVectors { get; set; }

        /// <summary>
        /// Initialize the standard eigenvalue problem.
        /// </summary>
        public ArpackContext(CompressedColumnStorage<T> A, bool symmetric)
        {
            if (A.RowCount != A.ColumnCount)
            {
                throw new ArgumentException("Matrix must be square.", "A");
            }

            this.size = A.RowCount;
            this.A = A;

            this.symmetric = symmetric;
            
            Iterations = 1000;
        }

        /// <summary>
        /// Initialize the generalized eigenvalue problem.
        /// </summary>
        public ArpackContext(CompressedColumnStorage<T> A, CompressedColumnStorage<T> B, bool symmetric)
            : this(A, symmetric)
        {
            if (B.RowCount != B.ColumnCount)
            {
                throw new ArgumentException("Matrix must be square.", "B");
            }

            if (this.size != B.RowCount)
            {
                throw new ArgumentException("Matrix must be of same dimension as A.", "B");
            }

            if (!symmetric)
            {
                //throw new ArgumentException(Properties.Resources.ArgumentMatrixSymmetric);
            }

            this.B = B;
        }
        
        /// <summary>
        /// Solve the standard eigenvalue problem.
        /// </summary>
        /// <param name="k">The number of eigenvalues to compute.</param>
        /// <param name="job">The part of the spectrum to compute.</param>
        /// <returns>The number of converged eigenvalues.</returns>
        public abstract ArpackResult<T> SolveStandard(int k, string job);

        /// <summary>
        /// Solve the standard eigenvalue problem in shift-invert mode.
        /// </summary>
        /// <param name="k">The number of eigenvalues to compute.</param>
        /// <param name="sigma">The shift value.</param>
        /// <param name="job">The part of the spectrum to compute.</param>
        /// <returns>The number of converged eigenvalues.</returns>
        public abstract ArpackResult<T> SolveStandard(int k, T sigma, string job = Job.LargestMagnitude);

        /// <summary>
        /// Solve the generalized eigenvalue problem.
        /// </summary>
        /// <param name="k">The number of eigenvalues to compute.</param>
        /// <param name="job">The part of the spectrum to compute.</param>
        /// <returns>The number of converged eigenvalues.</returns>
        public abstract ArpackResult<T> SolveGeneralized(int k, string job);

        /// <summary>
        /// Solve the generalized eigenvalue problem in shift-invert mode.
        /// </summary>
        /// <param name="k">The number of eigenvalues to compute.</param>
        /// <param name="sigma">The shift value.</param>
        /// <param name="job">The part of the spectrum to compute.</param>
        /// <returns>The number of converged eigenvalues.</returns>
        public abstract ArpackResult<T> SolveGeneralized(int k, T sigma, string job = Job.LargestMagnitude);

        internal ar_spmat GetMatrix(CompressedColumnStorage<T> matrix, List<GCHandle> handles)
        {
            ar_spmat a = default(ar_spmat);

            a.n = size;
            a.p = InteropHelper.Pin(matrix.ColumnPointers, handles);
            a.i = InteropHelper.Pin(matrix.RowIndices, handles);
            a.x = InteropHelper.Pin(matrix.Values, handles);
            a.nnz = matrix.NonZerosCount;

            return a;
        }
        
        protected StringBuilder ToStringBuilder(string job)
        {
            return new StringBuilder(job);
        }
    }
}
