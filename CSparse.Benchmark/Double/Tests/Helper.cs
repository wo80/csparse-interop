using CSparse.Storage;
using System;

namespace CSparse.Double.Tests
{
    static class Helper
    {
        private const double ERROR_THRESHOLD = 1e-3;

        /// <summary>
        /// Check residuals of standard symmetric eigenvalue problem.
        /// </summary>
        /// <param name="A">Symmetric matrix.</param>
        /// <param name="nconv">Number of converged eigenvalues.</param>
        /// <param name="v">Eigenvalues.</param>
        /// <param name="X">Eigenvectors.</param>
        /// <param name="print">If true, print residuals.</param>
        /// <returns>True, if all residuals are below threshold.</returns>
        public static bool Residuals(SparseMatrix A, int nconv, double[] v, Matrix<double> X, bool print)
        {
            int N = A.RowCount;
            
            if (print)
            {
                Console.WriteLine();
                Console.WriteLine("       Lambda         Residual");
            }

            var x = new double[N];
            var y = new double[N];

            bool ok = true;

            for (int i = 0; i < nconv; i++)
            {
                var lambda = v[i];

                X.Column(i, x);

                Vector.Copy(x, y);

                // y = A*x - lambda*x
                A.Multiply(1.0, x, -lambda, y);

                double r = Vector.Norm(y) / Math.Abs(lambda);

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

        /// <summary>
        /// Check residuals of generalized symmetric eigenvalue problem.
        /// </summary>
        /// <param name="A">Symmetric matrix.</param>
        /// <param name="B">Symmetric matrix.</param>
        /// <param name="nconv">Number of converged eigenvalues.</param>
        /// <param name="v">Eigenvalues.</param>
        /// <param name="X">Eigenvectors.</param>
        /// <param name="print">If true, print residuals.</param>
        /// <returns>True, if all residuals are below threshold.</returns>
        public static bool Residuals(SparseMatrix A, SparseMatrix B, int nconv, double[] v, Matrix<double> X, bool print)
        {
            int N = A.RowCount;

            if (print)
            {
                Console.WriteLine();
                Console.WriteLine("       Lambda         Residual");
            }

            var x = new double[N];
            var y = new double[N];

            bool ok = true;

            for (int i = 0; i < nconv; i++)
            {
                var lambda = v[i];

                X.Column(i, x);

                Vector.Copy(x, y);

                // y = B*x
                B.Multiply(x, y);

                // y = A*x - lambda*B*x
                A.Multiply(1.0, x, -lambda, y);

                double r = Vector.Norm(y) / Math.Abs(lambda);

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
    }
}
