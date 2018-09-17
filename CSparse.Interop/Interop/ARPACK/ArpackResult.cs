﻿
namespace CSparse.Interop.ARPACK
{
    using CSparse.Interop.Common;
    using CSparse.Storage;
    using System;
    using System.Collections.Generic;
    using System.Numerics;
    using System.Runtime.InteropServices;

    /// <summary>
    /// ARPACK result.
    /// </summary>
    public abstract class ArpackResult<T>
        where T : struct, IEquatable<T>, IFormattable
    {
        protected Matrix<T> eigvec;

        // The following objects are either float[] or double[] arrays. They are
        // initialized in the overriden CreateWorkspace method.

        protected object eigvalr;
        protected object eigvali;

        protected int size;

        /// <summary>
        /// Initializes a new instance of the <see cref="ArpackResult{T}"/> class.
        /// </summary>
        /// <param name="k">The number of eigenvalues to compute.</param>
        /// <param name="size">The problem size.</param>
        /// <param name="computeEigenVectors">A value, indicating whether eigenvectors are requested.</param>
        public ArpackResult(int k, int size, bool computeEigenVectors)
        {
            this.Count = k;

            this.size = size;

            CreateWorkspace(computeEigenVectors);
        }

        /// <summary>
        /// Gets the number of requested eigenvalues.
        /// </summary>
        public int Count { get; private set; }

        /// <summary>
        /// Gets the number of converged eigenvalues.
        /// </summary>
        public int ConvergedEigenvalues { get; internal set; }

        /// <summary>
        /// Gets the number of iteration taken.
        /// </summary>
        public int IterationsTaken { get; internal set; }

        /// <summary>
        /// Gets the error code returned by ARPACK (0 = all fine).
        /// </summary>
        public int ErrorCode { get; internal set; }

        /// <summary>
        /// Gets the dense matrix of eigenvectors stored in column major order.
        /// </summary>
        public Matrix<T> EigenVectors
        {
            get { return eigvec; }
        }

        /// <summary>
        /// Gets the eigenvalues.
        /// </summary>
        public Complex[] EigenValues
        {
            get
            {
                if (eigvalr == null)
                {
                    return null;
                }

                return CreateEigenvaluesArray();
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

        protected abstract void CreateWorkspace(bool computeEigenVectors);

        protected abstract Complex[] CreateEigenvaluesArray();

        internal ar_result GetEigenvalueStorage(List<GCHandle> handles)
        {
            ar_result e = default(ar_result);

            e.eigvalr = InteropHelper.Pin(eigvalr, handles);
            e.eigvali = InteropHelper.Pin(eigvali, handles);
            e.eigvec = InteropHelper.Pin(((DenseColumnMajorStorage<T>)eigvec).Values, handles);
            e.info = 0;

            return e;
        }
    }
}