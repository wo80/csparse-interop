
namespace CSparse.Double
{
    using CSparse.Solvers;
    using System;
    using System.Numerics;

    using CVector = CSparse.Complex.Vector;

    static class Helper
    {
        private const double ERROR_THRESHOLD = 1e-3;

        #region Linear equations

        public static double ComputeError(double[] actual, double[] expected, bool relativeError = true)
        {
            var e = Vector.Clone(actual);

            Vector.Axpy(-1.0, expected, e);

            if (relativeError)
            {
                return Vector.Norm(e) / Vector.Norm(expected);
            }

            return Vector.Norm(e);
        }

        #endregion

        #region Eigenvalues

        /// <summary>
        /// Check residuals of symmetric eigenvalue problem.
        /// </summary>
        /// <param name="A">The matrix A.</param>
        /// <param name="result">The eigensolver result.</param>
        /// <param name="symmetric">Indicates, whether the problem is symmetric.</param>
        /// <param name="print">If true, print residuals.</param>
        /// <returns>True, if all residuals are below threshold.</returns>
        public static bool CheckResiduals(SparseMatrix A, IEigenSolverResult result, bool symmetric, bool print)
        {
            return CheckResiduals(A, null, result, symmetric, print);
        }

        /// <summary>
        /// Check residuals of generalized symmetric eigenvalue problem.
        /// </summary>
        /// <param name="A">The matrix A.</param>
        /// <param name="B">The matrix B.</param>
        /// <param name="result">The eigensolver result.</param>
        /// <param name="symmetric">Indicates, whether the problem is symmetric.</param>
        /// <param name="print">If true, print residuals.</param>
        /// <returns>True, if all residuals are below threshold.</returns>
        public static bool CheckResiduals(SparseMatrix A, SparseMatrix B, IEigenSolverResult result, bool symmetric, bool print)
        {
            if (symmetric)
            {
                return CheckResiduals(A, B, result.ConvergedEigenValues, result.EigenValuesReal(), result.EigenVectorsReal(), print);
            }

            return CheckResiduals(A, B, result.ConvergedEigenValues, result.EigenValues, result.EigenVectors, print);
        }

        /// <summary>
        /// Check residuals of symmetric eigenvalue problem.
        /// </summary>
        /// <param name="A">The matrix A.</param>
        /// <param name="k">The number of converged eigenvalues.</param>
        /// <param name="v">The eigenvalues array.</param>
        /// <param name="X">The eigenvectors matrix.</param>
        /// <param name="print">If true, print residuals.</param>
        /// <returns>True, if all residuals are below threshold.</returns>
        public static bool CheckResiduals(SparseMatrix A, int k, double[] v, Matrix<double> X, bool print)
        {
            return CheckResiduals(A, null, k, v, X, print);
        }

        /// <summary>
        /// Check residuals of generalized symmetric eigenvalue problem.
        /// </summary>
        /// <param name="A">The matrix A.</param>
        /// <param name="B">The matrix B.</param>
        /// <param name="k">The number of converged eigenvalues.</param>
        /// <param name="v">The eigenvalues array.</param>
        /// <param name="X">The eigenvectors matrix.</param>
        /// <param name="print">If true, print residuals.</param>
        /// <returns>True, if all residuals are below threshold.</returns>
        public static bool CheckResiduals(SparseMatrix A, SparseMatrix B, int k, double[] v, Matrix<double> X, bool print)
        {
            int N = A.RowCount;

            // If more eigenvalues converged than were requested (real, non-symmetric case only).
            k = Math.Min(k, X.ColumnCount);

            if (print)
            {
                Console.WriteLine();
                Console.WriteLine("       Lambda         Residual");
            }

            var x = new double[N];
            var y = new double[N];

            bool ok = true;

            for (int i = 0; i < k; i++)
            {
                var lambda = v[i];

                X.Column(i, x);

                Vector.Copy(x, y);

                if (B != null)
                {
                    // y = B*x
                    B.Multiply(x, y);
                }

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

        /// <summary>
        /// Check residuals of generalized eigenvalue problem.
        /// </summary>
        /// <param name="A">Symmetric matrix.</param>
        /// <param name="B">Symmetric matrix.</param>
        /// <param name="k">The number of converged eigenvalues.</param>
        /// <param name="v">The eigenvalues array.</param>
        /// <param name="X">The eigenvectors matrix.</param>
        /// <param name="print">If true, print residuals.</param>
        /// <returns>True, if all residuals are below threshold.</returns>
        public static bool CheckResiduals(SparseMatrix A, SparseMatrix B, int k, Complex[] v, Matrix<Complex> X, bool print)
        {
            int N = A.RowCount;

            // If more eigenvalues converged than were requested (real, non-symmetric case only).
            k = Math.Min(k, X.ColumnCount);

            if (print)
            {
                Console.WriteLine();
                Console.WriteLine("       Lambda         Residual");
            }

            var x = new Complex[N];
            var y = new Complex[N];

            bool ok = true;

            for (int i = 0; i < k; i++)
            {
                var lambda = v[i];

                X.Column(i, x);

                CVector.Copy(x, y);

                if (B != null)
                {
                    // y = B*x
                    B.Multiply(x, y);
                }

                // y = A*x - lambda*B*x
                A.Multiply(1.0, x, -lambda, y);

                double r = CVector.Norm(y) / Complex.Abs(lambda);

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
