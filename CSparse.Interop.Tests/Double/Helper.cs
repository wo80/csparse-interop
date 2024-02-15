
namespace CSparse.Interop.Tests.Double
{
    using CSparse.Double;
    using CSparse.Solvers;
    using CSparse.Storage;
    using System;
    using Complex= System.Numerics.Complex;
    using CVector = CSparse.Complex.Vector;

    static class Helper
    {
        private const double ERROR_THRESHOLD = 1e-3;

        #region Linear equations

        public static double ComputeError(double[] actual, double[] expected, bool relativeError = true)
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

        public static double ComputeResidual(CompressedColumnStorage<double> A, double[] x, double[] b, bool relativeError = true)
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
        /// Check residuals of symmetric eigenvalue problem.
        /// </summary>
        /// <param name="A">The matrix A.</param>
        /// <param name="result">The eigensolver result.</param>
        /// <param name="symmetric">Indicates, whether the problem is symmetric.</param>
        /// <returns>True, if all residuals are below threshold.</returns>
        public static bool CheckResiduals(SparseMatrix A, IEigenSolverResult result, bool symmetric)
        {
            return CheckResiduals(A, null, result, symmetric);
        }

        /// <summary>
        /// Check residuals of generalized symmetric eigenvalue problem.
        /// </summary>
        /// <param name="A">The matrix A.</param>
        /// <param name="B">The matrix B.</param>
        /// <param name="result">The eigensolver result.</param>
        /// <param name="symmetric">Indicates, whether the problem is symmetric.</param>
        /// <returns>True, if all residuals are below threshold.</returns>
        public static bool CheckResiduals(SparseMatrix A, SparseMatrix B, IEigenSolverResult result, bool symmetric)
        {
            if (symmetric)
            {
                return CheckResiduals(A, B, result.ConvergedEigenValues, result.EigenValuesReal(), result.EigenVectorsReal());
            }

            return CheckResiduals(A, B, result.ConvergedEigenValues, result.EigenValues, result.EigenVectors);
        }

        /// <summary>
        /// Check residuals of symmetric eigenvalue problem.
        /// </summary>
        /// <param name="A">The matrix A.</param>
        /// <param name="k">The number of converged eigenvalues.</param>
        /// <param name="v">The eigenvalues array.</param>
        /// <param name="X">The eigenvectors matrix.</param>
        /// <returns>True, if all residuals are below threshold.</returns>
        public static bool CheckResiduals(SparseMatrix A, int k, double[] v, Matrix<double> X)
        {
            return CheckResiduals(A, null, k, v, X);
        }

        /// <summary>
        /// Check residuals of generalized symmetric eigenvalue problem.
        /// </summary>
        /// <param name="A">The matrix A.</param>
        /// <param name="B">The matrix B.</param>
        /// <param name="k">The number of converged eigenvalues.</param>
        /// <param name="v">The eigenvalues array.</param>
        /// <param name="X">The eigenvectors matrix.</param>
        /// <returns>True, if all residuals are below threshold.</returns>
        public static bool CheckResiduals(SparseMatrix A, SparseMatrix B, int k, double[] v, Matrix<double> X)
        {
            int n = A.RowCount;

            // If more eigenvalues converged than were requested (real, non-symmetric case only).
            k = Math.Min(k, X.ColumnCount);

            var x = new double[n];
            var y = new double[n];

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

                double r = Vector.Norm(n, y) / Math.Abs(lambda);

                if (r > ERROR_THRESHOLD)
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Check residuals of generalized eigenvalue problem.
        /// </summary>
        /// <param name="A">Symmetric matrix.</param>
        /// <param name="B">Symmetric matrix.</param>
        /// <param name="k">The number of converged eigenvalues.</param>
        /// <param name="v">The eigenvalues array.</param>
        /// <param name="X">The eigenvectors matrix.</param>
        /// <returns>True, if all residuals are below threshold.</returns>
        public static bool CheckResiduals(SparseMatrix A, SparseMatrix B, int k, Complex[] v, Matrix<Complex> X)
        {
            int n = A.RowCount;

            // If more eigenvalues converged than were requested (real, non-symmetric case only).
            k = Math.Min(k, X.ColumnCount);

            var x = new Complex[n];
            var y = new Complex[n];

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

                double r = CVector.Norm(n, y) / Complex.Abs(lambda);

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
