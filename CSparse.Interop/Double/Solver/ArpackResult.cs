
namespace CSparse.Double.Solver
{
    using CSparse.Interop.ARPACK;
    using System;
    using System.Numerics;

    /// <inheritdoc />
    public class ArpackResult : ArpackResult<double>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ArpackResult"/> class.
        /// </summary>
        /// <param name="k">The number of eigenvalues requested.</param>
        /// <param name="size">The problem size.</param>
        /// <param name="computeEigenVectors">A value, indicating whether eigenvectors are requested.</param>
        /// <param name="symmetric">A value, indicating whether problem is symmetric.</param>
        public ArpackResult(int k, int size, bool computeEigenVectors, bool symmetric)
            : base(k, size)
        {
            CreateWorkspace(computeEigenVectors, symmetric);
        }
        
        /// <summary>
        /// Copies the real part of the eigenvalues to specified target array.
        /// </summary>
        public void GetEigenvalues(double[] target, int count)
        {
            Array.Copy((double[])eigvalr, 0, target, 0, count);
        }

        /// <inheritdoc />
        protected override Complex[] CreateEigenvaluesArray()
        {
            int k = this.Count;
            
            var result = new Complex[k];

            var rp = (double[])eigvalr;
            var ip = (double[])eigvali;

            for (int i = 0; i < k; i++)
            {
                result[i] = new Complex(rp[i], ip[i]);
            }

            return result;
        }

        private void CreateWorkspace(bool computeEigenVectors, bool symmetric)
        {
            int n = this.size;
            int k = this.Count;

            // For complex eigenvalues of non-symmetric problems, the complex conjugate will also be
            // an eigenvalue. ARPACK will always compute both eigenvalues. This means that though k
            // eigenvalues might be requested, k+1 eigenvalues will be computed. We have to allocate
            // enough memory to handle this case.

            int s = symmetric ? k : k + 1;

            // For symmetric problems, eigvali isn't used. We initialize it anyway.

            eigvalr = new double[s];
            eigvali = new double[s];

            if (computeEigenVectors)
            {
                // The DenseMatrix ctor allows passing an array with dimensions > n * k. This way
                // the n-by-k eigenvector matrix can also be used for non-symmetric problems.
                eigvec = new DenseMatrix(n, k, new double[n * s]);

                // HACK: this only works because the array values are stored in column major order.
            }
        }
    }
}
