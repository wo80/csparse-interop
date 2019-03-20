
namespace CSparse.Interop.MKL
{
    using CSparse.Storage;
    using System;
    using System.Numerics;

    public abstract class ExtendedEigensolverResult<T>
        where T : struct, IEquatable<T>, IFormattable
    {
        protected int size;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExtendedEigensolverResult{T}"/> class.
        /// </summary>
        /// <param name="info">The status returned by Intel's extended eigensolver.</param>
        /// <param name="size">The matrix size.</param>
        /// <param name="k">The number of eigenvalues found (k &lt; k0).</param>
        /// <param name="e">Array of length k0. The first k entries of e are eigenvalues found in the interval.</param>
        /// <param name="x">Matrix with k0 columns containing the orthonormal eigenvectors corresponding to the
        /// computed eigenvalues e, with the i-th column of x holding the eigenvector associated with e[i].</param>
        /// <param name="r">Array of length k0 containing the relative residual vector (in the first k components).</param>
        public ExtendedEigensolverResult(SparseStatus info, int size, int k, double[] e, Matrix<T> x, double[] r)
        {
            this.size = size;

            Status = info;

            ConvergedEigenvalues = k;
            Residuals = r;
        }

        /// <summary>
        /// Gets the status code returned by FEAST.
        /// </summary>
        public SparseStatus Status { get; protected set; }
        
        /// <summary>
        /// Gets the number of converged eigenvalues.
        /// </summary>
        public int ConvergedEigenvalues { get; protected set; }

        /// <summary>
        /// Gets the dense matrix of eigenvectors stored in column major order.
        /// </summary>
        public Matrix<Complex> EigenVectors { get; protected set; }

        /// <summary>
        /// Gets the eigenvalues.
        /// </summary>
        public Complex[] EigenValues { get; protected set; }

        /// <summary>
        /// Gets the residuals vector.
        /// </summary>
        public double[] Residuals { get; protected set; }

        /// <summary>
        /// Gets the real part of the eigenvalues.
        /// </summary>
        public abstract double[] EigenValuesReal();
        
        /// <summary>
        /// Gets the real part of the eigenvectors.
        /// </summary>
        public abstract Matrix<double> EigenVectorsReal();

        protected virtual Complex[] CreateEigenValues(double[] x, int length)
        {
            var result = new Complex[length];

            for (int i = 0; i < length; i++)
            {
                result[i] = x[i];
            }

            return result;
        }
    }
}