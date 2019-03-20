
namespace CSparse.Complex
{
    using CSparse.Solvers;
    using CSparse.Storage;
    using System;
    using System.Numerics;

    static class Helper
    {
        private const double ERROR_THRESHOLD = 1e-3;

        #region Linear equations

        public static double ComputeError(Complex[] actual, Complex[] expected, bool relativeError = true)
        {
            var e = Vector.Clone(actual);

            Vector.Axpy(-1.0, expected, e);

            if (relativeError)
            {
                return Vector.Norm(e) / Vector.Norm(expected);
            }

            return Vector.Norm(e);
        }

        public static double ComputeResidual(CompressedColumnStorage<Complex> A, Complex[] x, Complex[] b, bool relativeError = true)
        {
            var e = Vector.Clone(b);

            A.Multiply(-1.0, x, 1.0, e);

            if (relativeError)
            {
                return Vector.Norm(e) / (A.FrobeniusNorm() * Vector.Norm(b));
            }

            return Vector.Norm(e);
        }

        #endregion

        #region Eigenvalues

        /// <summary>
        /// Check residuals of eigenvalue problem.
        /// </summary>
        /// <param name="A">The matrix.</param>
        /// <param name="result">The eigensolver result.</param>
        /// <param name="print">If true, print residuals.</param>
        /// <returns>True, if all residuals are below threshold.</returns>
        public static bool CheckResiduals(SparseMatrix A, IEigenSolverResult result, bool print)
        {
            int N = A.RowCount;

            var m = result.ConvergedEigenValues;

            var v = result.EigenValues;
            var X = result.EigenVectors;

            if (print)
            {
                Console.WriteLine();
                Console.WriteLine("       Lambda         Residual");
            }

            var x = new Complex[N];
            var y = new Complex[N];

            bool ok = true;

            for (int i = 0; i < m; i++)
            {
                var lambda = v[i].Real;

                X.Column(i, x);

                Vector.Copy(x, y);

                // y = A*x - lambda*x
                A.Multiply(1.0, x, -lambda, y);

                double r = Vector.Norm(y);

                if (r > ERROR_THRESHOLD)
                {
                    ok = false;
                }

                if (print)
                {
                    Console.WriteLine("{0,3}:   {1,10:0.00000000}   {2,10:0.00e+00}", i, lambda, r);
                }
            }

            return ok;
        }

        #endregion
    }
}
