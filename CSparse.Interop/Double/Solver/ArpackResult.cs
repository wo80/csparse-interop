
namespace CSparse.Double.Solver
{
    using CSparse.Interop.ARPACK;
    using System;
    using System.Numerics;

    public class ArpackResult : ArpackResult<double>
    {
        public ArpackResult(int k, int size, bool computeEigenVectors)
            : base(k, size, computeEigenVectors)
        {
        }
        
        /// <summary>
        /// Copies the real part of the eigenvalues to specified target array.
        /// </summary>
        public void GetEigenvalues(double[] target, int count)
        {
            Array.Copy((double[])eigvalr, 0, target, 0, count);
        }

        protected override void CreateWorkspace(bool computeEigenVectors)
        {
            int k = this.Count;
            
            // For symmetric problems, eigvali isn't used. We
            // initialize it anyway.

            eigvalr = new double[k];
            eigvali = new double[k];

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
            var ip = (double[])eigvali;

            for (int i = 0; i < k; i++)
            {
                result[i] = new Complex(rp[i], ip[i]);
            }

            return result;
        }
    }
}
