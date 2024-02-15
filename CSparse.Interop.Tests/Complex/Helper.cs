
namespace CSparse.Interop.Tests.Complex
{
    using CSparse.Complex;
    using CSparse.Solvers;
    using CSparse.Storage;
    using System;
    using Complex = System.Numerics.Complex;

    static class Helper
    {
        private const double ERROR_THRESHOLD = 1e-3;

        #region Linear equations

        public static double ComputeError(Complex[] actual, Complex[] expected, bool relativeError = true)
        {
            var e = Vector.Clone(actual);

            Vector.Axpy(-1.0, expected, e);

            int n = e.Length;

            if (relativeError)
            {
                return Vector.Norm(n, e) / Vector.Norm(n, expected);
            }

            return Vector.Norm(n, e);
        }

        public static double ComputeResidual(CompressedColumnStorage<Complex> A, Complex[] x, Complex[] b, bool relativeError = true)
        {
            var e = Vector.Clone(b);

            A.Multiply(-1.0, x, 1.0, e);

            int n = A.RowCount;

            if (relativeError)
            {
                return Vector.Norm(n, e) / (A.FrobeniusNorm() * Vector.Norm(n, b));
            }

            return Vector.Norm(n, e);
        }

        #endregion

        #region Eigenvalues

        /// <summary>
        /// Check residuals of eigenvalue problem.
        /// </summary>
        /// <param name="A">The matrix.</param>
        /// <param name="result">The eigensolver result.</param>
        /// <returns>True, if all residuals are below threshold.</returns>
        public static bool CheckResiduals(SparseMatrix A, IEigenSolverResult result)
        {
            int n = A.RowCount;

            var m = result.ConvergedEigenValues;

            var v = result.EigenValues;
            var X = result.EigenVectors;

            var x = new Complex[n];
            var y = new Complex[n];

            for (int i = 0; i < m; i++)
            {
                var lambda = v[i].Real;

                X.Column(i, x);

                Vector.Copy(x, y);

                // y = A*x - lambda*x
                A.Multiply(1.0, x, -lambda, y);

                double r = Vector.Norm(n, y);

                if (r > ERROR_THRESHOLD)
                {
                    return false;
                }
            }

            return true;
        }

        #endregion
    }
}
