
namespace CSparse.Complex.Solver
{
    using CSparse.Interop.ARPACK;
    using System.Numerics;

    /// <inheritdoc />
    public class ArpackResult : ArpackResult<Complex>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ArpackResult"/> class.
        /// </summary>
        /// <param name="k">The number of eigenvalues to compute.</param>
        /// <param name="size">The problem size.</param>
        /// <param name="computeEigenVectors">A value, indicating whether eigenvectors are requested.</param>
        public ArpackResult(int k, int size, bool computeEigenVectors)
            : base(k, size)
        {
            CreateWorkspace(computeEigenVectors);
        }

        /// <inheritdoc />
        protected override Complex[] CreateEigenvaluesArray()
        {
            int k = this.Count;

            var result = new Complex[k];

            var rp = (double[])eigvalr;

            for (int i = 0; i < k; i++)
            {
                result[i] = new Complex(rp[2 * i], rp[2 * i + 1]);
            }

            return result;
        }
        
        private void CreateWorkspace(bool computeEigenVectors)
        {
            int k = this.Count;

            // For complex matrices all eigenvalues are stored in
            // eigvalr with interleaved real and imaginary part.
            
            eigvalr = new double[2 * k];

            if (computeEigenVectors)
            {
                eigvec = new DenseMatrix(size, k);
            }
        }
    }
}
