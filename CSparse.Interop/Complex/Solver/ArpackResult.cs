
namespace CSparse.Complex.Solver
{
    using CSparse.Interop.ARPACK;
    using System.Numerics;

    public class ArpackResult : ArpackResult<Complex>
    {
        public ArpackResult(int k, int size, bool computeEigenVectors)
            : base(k, size, computeEigenVectors)
        {
        }

        protected override void CreateWorkspace(bool computeEigenVectors)
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
    }
}
