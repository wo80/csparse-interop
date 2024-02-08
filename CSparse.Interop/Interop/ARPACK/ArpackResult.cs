
namespace CSparse.Interop.ARPACK
{
    using CSparse.Interop.Common;
    using CSparse.Solvers;
    using CSparse.Storage;
    using System;
    using System.Collections.Generic;
    using System.Numerics;
    using System.Runtime.InteropServices;

    /// <summary>
    /// ARPACK result.
    /// </summary>
    public abstract class ArpackResult<T> : IEigenSolverResult
        where T : struct, IEquatable<T>, IFormattable
    {
        // The following objects are actually double[] arrays, which are initialized in the derived classes.
        // See implementation of abstract methods CreateEigenValuesArray() and CreateEigenVectorsMatrix().
        //
        // If anyone needs single precision, those could also be float[] arrays.

        protected object eigvec;
        protected object eigvalr;
        protected object eigvali;

        protected int size;

        /// <summary>
        /// Initializes a new instance of the <see cref="ArpackResult{T}"/> class.
        /// </summary>
        /// <param name="k">The number of eigenvalues requested.</param>
        /// <param name="size">The problem size.</param>
        public ArpackResult(int k, int size)
        {
            this.Count = k;

            this.size = size;
        }

        /// <inheritdoc />
        public int Count { get; private set; }

        /// <inheritdoc />
        public int ConvergedEigenValues { get; internal set; }

        /// <inheritdoc />
        public int IterationsTaken { get; internal set; }

        /// <inheritdoc />
        public int ArnoldiCount { get; internal set; }

        /// <summary>
        /// Gets the error code returned by ARPACK (0 = all fine).
        /// </summary>
        public int ErrorCode { get; internal set; }

        /// <inheritdoc />
        public bool HasEigenVectors => ConvergedEigenValues > 0 && eigvec != null;

        /// <inheritdoc />
        public Matrix<Complex> EigenVectors
        {
            get
            {
                if (eigvec == null)
                {
                    return null;
                }

                return CreateEigenVectorsMatrix();
            }
        }

        /// <inheritdoc />
        public Complex[] EigenValues
        {
            get
            {
                if (eigvalr == null)
                {
                    return null;
                }

                return CreateEigenValuesArray();
            }
        }

        /// <summary>
        /// Throws an <see cref="ArpackException"/>, if ARPACK failed to solve the problem.
        /// </summary>
        public void EnsureSuccess()
        {
            if (ErrorCode != 0)
            {
                throw new ArpackException(ErrorCode);
            }
        }

        /// <inheritdoc />
        public abstract double[] EigenValuesReal();

        /// <inheritdoc />
        public abstract Matrix<double> EigenVectorsReal();

        /// <summary>
        /// Creates the array of eigenvalues.
        /// </summary>
        protected abstract Complex[] CreateEigenValuesArray();

        /// <summary>
        /// Creates the matrix of eigenvectors.
        /// </summary>
        protected abstract Matrix<Complex> CreateEigenVectorsMatrix();

        internal ar_result GetEigenvalueStorage(List<GCHandle> handles)
        {
            ar_result e = default(ar_result);

            e.eigvalr = InteropHelper.Pin(eigvalr, handles);
            e.eigvali = InteropHelper.Pin(eigvali, handles);

            e.eigvec = InteropHelper.Pin(eigvec, handles);

            e.info = 0;

            return e;
        }
    }
}
