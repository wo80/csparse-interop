﻿
namespace CSparse.Double.Solver
{
    using CSparse.Interop.MKL;
    using CSparse.Interop.MKL.ExtendedEigensolver;

    public class ExtendedEigensolverResult : ExtendedEigensolverResult<double>
    {
        double[] eigenvalues;

        Matrix<double> eigenvectors;

        /// <inheritdoc />
        public ExtendedEigensolverResult(SparseStatus info, int size, int k, double[] e, Matrix<double> x, double[] r)
            : base(info, size, k, e, x, r)
        {
            eigenvalues = e;
            eigenvectors = x;
        }

        /// <inheritdoc />
        public override bool HasEigenVectors => ConvergedEigenValues > 0 && eigenvectors != null;

        /// <inheritdoc />
        public override double[] EigenValuesReal()
        {
            return eigenvalues;
        }

        /// <inheritdoc />
        public override Matrix<double> EigenVectorsReal()
        {
            return eigenvectors;
        }
    }
}
