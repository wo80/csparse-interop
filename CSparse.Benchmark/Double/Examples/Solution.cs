namespace CSparse.Double.Examples
{
    using CSparse.Double;
    using CSparse.Solvers;
    using System;

    using CVector = CSparse.Complex.Vector;
    using Complex = System.Numerics.Complex;

    /// <summary>
    /// Print information about eigenvalue problems.
    /// </summary>
    static class Solution
    {
        #region Symmetric

        // MODULE LSymSol.h

        /// <summary>
        /// Prints eigenvalues and eigenvectors of symmetric eigen-problems.
        /// </summary>
        public static void Symmetric(SparseMatrix A, IEigenSolverResult result, bool shift)
        {
            if (!EnsureSuccess(result))
            {
                return;
            }

            int n = A.RowCount;
            int nconv = result.ConvergedEigenValues;

            Console.WriteLine();
            Console.WriteLine("Testing ARPACK++ class ARluSymStdEig");
            Console.WriteLine("Real symmetric eigenvalue problem: A*x - lambda*x");

            Console.WriteLine(!shift ? "Regular mode" : "Shift and invert mode");
            Console.WriteLine();

            Console.WriteLine("Dimension of the system            : " + n);
            Console.WriteLine("Number of 'requested' eigenvalues  : " + result.Count);
            Console.WriteLine("Number of 'converged' eigenvalues  : " + nconv);
            Console.WriteLine("Number of Arnoldi vectors generated: " + result.ArnoldiCount);
            Console.WriteLine("Number of iterations taken         : " + result.IterationsTaken);
            Console.WriteLine();

            var evals = result.EigenValuesReal();
            var evecs = result.EigenVectorsReal();

            // Printing eigenvalues.

            Console.WriteLine("Eigenvalues:");

            for (int i = 0; i < nconv; i++)
            {
                Console.WriteLine("  lambda[" + (i + 1) + "]: " + evals[i]);
            }

            Console.WriteLine();


            if (evecs != null)
            {
                Symmetrize(ref A);

                // Printing the residual norm || A*x - lambda*x ||
                // for the nconv accurately computed eigenvectors.

                var x = new double[n];
                var y = new double[n];
                var r = new double[nconv]; // residuals

                for (int i = 0; i < nconv; i++)
                {
                    var lambda = evals[i];

                    evecs.Column(i, x);

                    Vector.Copy(x, y);

                    // y = A*x - lambda*x
                    A.Multiply(1.0, x, -lambda, y);

                    r[i] = Vector.Norm(n, y) / Math.Abs(lambda);
                }

                for (int i = 0; i < nconv; i++)
                {
                    Console.WriteLine("||A*x(" + (i + 1) + ") - lambda(" + (i + 1) + ")*x(" + (i + 1) + ")||: " + r[i]);
                }

                Console.WriteLine();
            }
        }

        /// <summary>
        /// Prints eigenvalues and eigenvectors of symmetric generalized eigen-problems.
        /// </summary>
        public static void Symmetric(SparseMatrix A, SparseMatrix B, IEigenSolverResult result, ShiftMode mode)
        {
            if (!EnsureSuccess(result))
            {
                return;
            }

            int n = A.RowCount;
            int nconv = result.ConvergedEigenValues;

            Console.WriteLine();
            Console.WriteLine("Testing ARPACK++ class ARluSymGenEig");
            Console.WriteLine("Real symmetric generalized eigenvalue problem: A*x - lambda*B*x");
            Console.WriteLine();

            switch (mode)
            {
                case ShiftMode.None:
                    Console.WriteLine("Regular mode");
                    break;
                case ShiftMode.Regular:
                    Console.WriteLine("Shift and invert mode");
                    break;
                case ShiftMode.Buckling:
                    Console.WriteLine("Buckling mode");
                    break;
                case ShiftMode.Cayley:
                    Console.WriteLine("Cayley mode");
                    break;
            }

            Console.WriteLine();
            Console.WriteLine("Dimension of the system            : " + n);
            Console.WriteLine("Number of 'requested' eigenvalues  : " + result.Count);
            Console.WriteLine("Number of 'converged' eigenvalues  : " + nconv);
            Console.WriteLine("Number of Arnoldi vectors generated: " + result.ArnoldiCount);
            Console.WriteLine("Number of iterations taken         : " + result.IterationsTaken);
            Console.WriteLine();

            var evals = result.EigenValuesReal();
            var evecs = result.EigenVectorsReal();

            // Printing eigenvalues.

            Console.WriteLine("Eigenvalues:");

            for (int i = 0; i < nconv; i++)
            {
                Console.WriteLine("  lambda[" + (i + 1) + "]: " + evals[i]);
            }

            Console.WriteLine();

            if (evecs != null)
            {
                Symmetrize(ref A);
                Symmetrize(ref B);

                // Printing the residual norm || A*x - lambda*B*x ||
                // for the nconv accurately computed eigenvectors.

                var x = new double[n];
                var y = new double[n];
                var r = new double[nconv]; // residuals

                for (int i = 0; i < nconv; i++)
                {
                    var lambda = evals[i];

                    evecs.Column(i, x);

                    Vector.Copy(x, y);

                    // y = B*x
                    B.Multiply(x, y);

                    // y = A*x - lambda*B*x
                    A.Multiply(1.0, x, -lambda, y);

                    r[i] = Vector.Norm(n, y) / Math.Abs(lambda);
                }

                for (int i = 0; i < nconv; i++)
                {
                    Console.WriteLine("||A*x(" + i + ") - lambda(" + i + ")*B*x(" + i + ")||: " + r[i]);
                }

                Console.WriteLine();
            }
        }

        #endregion

        #region General

        // MODULE LNSymSol.h

        /// <summary>
        /// Prints eigenvalues and eigenvectors of nonsymmetric eigen-problems.
        /// </summary>
        public static void General(SparseMatrix A, IEigenSolverResult result, bool shift)
        {
            if (!EnsureSuccess(result))
            {
                return;
            }

            int n = A.RowCount;
            int nconv = result.ConvergedEigenValues;

            Console.WriteLine();
            Console.WriteLine("Testing ARPACK++ class ARluNonSymStdEig");
            Console.WriteLine("Real non-symmetric eigenvalue problem: A*x - lambda*x");
            Console.WriteLine(!shift ? "Regular mode" : "Shift and invert mode");
            Console.WriteLine();

            Console.WriteLine("Dimension of the system            : " + n);
            Console.WriteLine("Number of 'requested' eigenvalues  : " + result.Count);
            Console.WriteLine("Number of 'converged' eigenvalues  : " + nconv);
            Console.WriteLine("Number of Arnoldi vectors generated: " + result.ArnoldiCount);
            Console.WriteLine("Number of iterations taken         : " + result.IterationsTaken);
            Console.WriteLine();

            var evals = result.EigenValues;
            var evecs = result.EigenVectors;

            // Printing eigenvalues.

            Console.WriteLine("Eigenvalues:");

            for (int i = 0; i < nconv; i++)
            {
                Console.WriteLine("  lambda[" + (i + 1) + "]: " + evals[i]);
            }

            Console.WriteLine();

            if (evecs != null)
            {
                // Printing the residual norm || A*x - lambda*x ||
                // for the nconv accurately computed eigenvectors.

                var x = new Complex[n];
                var y = new Complex[n];
                var r = new double[nconv]; // residuals

                for (int i = 0; i < nconv; i++)
                {
                    var lambda = evals[i];

                    evecs.Column(i, x);

                    CVector.Copy(x, y);

                    // y = A*x - lambda*x
                    A.Multiply(1.0, x, -lambda, y);

                    r[i] = CVector.Norm(n, y) / Complex.Abs(lambda);
                }

                for (int i = 0; i < nconv; i++)
                {
                    Console.WriteLine("||A*x(" + (i + 1) + ") - lambda(" + (i + 1) + ")*x(" + (i + 1) + ")||: " + r[i]);
                }

                Console.WriteLine();
            }
        }

        /// <summary>
        /// Prints eigenvalues and eigenvectors of nonsymmetric generalized eigen-problems.
        /// </summary>
        public static void General(SparseMatrix A, SparseMatrix B, IEigenSolverResult result, bool shift, bool cshift = false)
        {
            if (!EnsureSuccess(result))
            {
                return;
            }

            int n = A.RowCount;
            int nconv = result.ConvergedEigenValues;

            Console.WriteLine();
            Console.WriteLine("Testing ARPACK++ class ARluNonSymGenEig");
            Console.WriteLine("Real nonsymmetric generalized eigenvalue problem: A*x - lambda*B*x");

            Console.WriteLine(!shift ? "Regular mode" :
                (cshift ? "Shift and invert mode (using the imaginary part of OP)" : "Shift and invert mode (using the real part of OP)"));
            Console.WriteLine();

            Console.WriteLine("Dimension of the system            : " + n);
            Console.WriteLine("Number of 'requested' eigenvalues  : " + result.Count);
            Console.WriteLine("Number of 'converged' eigenvalues  : " + nconv);
            Console.WriteLine("Number of Arnoldi vectors generated: " + result.ArnoldiCount);
            Console.WriteLine("Number of iterations taken         : " + result.IterationsTaken);
            Console.WriteLine();

            var evals = result.EigenValues;
            var evecs = result.EigenVectors;

            // Printing eigenvalues.

            Console.WriteLine("Eigenvalues:");

            for (int i = 0; i < nconv; i++)
            {
                Console.WriteLine("  lambda[" + (i + 1) + "]: " + evals[i]);
            }

            Console.WriteLine();

            if (evecs != null)
            {
                // Printing the residual norm || A*x - lambda*B*x ||
                // for the nconv accurately computed eigenvectors.

                var x = new Complex[n];
                var y = new Complex[n];
                var r = new double[nconv]; // residuals

                for (int i = 0; i < nconv; i++)
                {
                    var lambda = evals[i];

                    evecs.Column(i, x);

                    CVector.Copy(x, y);

                    // y = B*x
                    B.Multiply(x, y);

                    // y = A*x - lambda*B*x
                    A.Multiply(1.0, x, -lambda, y);

                    r[i] = CVector.Norm(n, y) / Complex.Abs(lambda);
                }

                for (int i = 0; i < nconv; i++)
                {
                    Console.WriteLine("||A*x(" + i + ") - lambda(" + i + ")*B*x(" + i + ")||: " + r[i]);
                }

                Console.WriteLine();
            }
        }

        #endregion

        #region SVD

        /// <summary>
        /// Prints singular values and condition number.
        /// </summary>
        public static void Condition(SparseMatrix A, IEigenSolverResult result)
        {
            if (!EnsureSuccess(result))
            {
                return;
            }

            int m = A.RowCount;
            int n = A.ColumnCount;
            int nconv = result.ConvergedEigenValues;

            Console.WriteLine();
            Console.WriteLine("Testing ARPACK++ SVD");
            Console.WriteLine("Obtaining singular values by solving (A'*A)*v = sigma*v");
            Console.WriteLine();

            Console.WriteLine("Dimension of the system              : " + n);
            Console.WriteLine("Number of 'requested' singular values: " + result.Count);
            Console.WriteLine("Number of 'converged' singular values: " + nconv);
            Console.WriteLine("Number of Arnoldi vectors generated  : " + result.ArnoldiCount);
            Console.WriteLine("Number of iterations taken           : " + result.IterationsTaken);
            Console.WriteLine();

            var svals = result.EigenValuesReal();

            // Calculating singular values.

            Console.WriteLine("Singular values of both end:");

            for (int i = 0; i < nconv; i++)
            {
                svals[i] = Math.Sqrt(svals[i]);

                Console.WriteLine("  sigma[" + (i + 1) + "]: " + svals[i]);
            }

            Console.WriteLine();
            Console.WriteLine("  Condition number of A  : " + (svals[nconv - 1] / svals[0]));
            Console.WriteLine();
        }

        /// <summary>
        /// Prints singular values and vectors.
        /// </summary>
        public static void SVD(SparseMatrix A, IEigenSolverResult result)
        {
            if (!EnsureSuccess(result))
            {
                return;
            }

            int m = A.RowCount;
            int n = A.ColumnCount;
            int nconv = result.ConvergedEigenValues;

            Console.WriteLine();
            Console.WriteLine("Testing ARPACK++ SVD");
            Console.WriteLine("Compute partial SVD: A = U*S*V'");
            Console.WriteLine();

            Console.WriteLine("Dimension of the system              : " + n);
            Console.WriteLine("Number of 'requested' singular values: " + result.Count);
            Console.WriteLine("Number of 'converged' singular values: " + nconv);
            Console.WriteLine("Number of Arnoldi vectors generated  : " + result.ArnoldiCount);
            Console.WriteLine("Number of iterations taken           : " + result.IterationsTaken);
            Console.WriteLine();

            var svals = result.EigenValuesReal();
            var svecs = result.EigenVectorsReal();

            int nAx = (n > m) ? n : m;

            // Printing singular values.

            Console.WriteLine("Singular values:");

            for (int i = 0; i < nconv; i++)
            {
                Console.WriteLine("  sigma[" + (i + 1) + "]: " + svals[i]);
            }

            Console.WriteLine();

            if (svecs != null)
            {
                var temp = new double[m + n];

                var v = new double[n];
                var u = new double[m];
                var r = new double[nconv]; // residuals
                var s = new double[nconv]; // residuals (transposed system)

                for (int i = 0; i < nconv; i++)
                {
                    var sigma = svals[i];

                    svecs.Column(i, temp);

                    // Compute A*v - sigma*u
                    Array.Copy(temp, m, v, 0, n);
                    Array.Copy(temp, 0, u, 0, m);

                    A.Multiply(1.0, v, -sigma, u);

                    r[i] = Vector.Norm(m, u) / Math.Abs(sigma);

                    // Compute A'*u - sigma*v
                    Array.Copy(temp, m, v, 0, n);
                    Array.Copy(temp, 0, u, 0, m);

                    A.TransposeMultiply(1.0, u, -sigma, v);

                    s[i] = Vector.Norm(n, v) / Math.Abs(sigma);
                }

                // Printing the residual norm || A*v - sigma*u ||
                // for the nconv accurately computed vectors u and v.

                for (int i = 0; i < nconv; i++)
                {
                    Console.WriteLine("||A*v(" + (i + 1) + ") - sigma(" + (i + 1) + ")*u(" + (i + 1) + ")||: " + r[i]);
                }

                Console.WriteLine();

                // Printing the residual norm || A'*u - sigma*v ||
                // for the nconv accurately computed vectors u and v.

                for (int i = 0; i < nconv; i++)
                {
                    Console.WriteLine("||A'*u(" + (i + 1) + ") - sigma(" + (i + 1) + ")*v(" + (i + 1) + ")||: " + s[i]);
                }

                Console.WriteLine();
            }
        }

        #endregion

        private static bool EnsureSuccess(IEigenSolverResult result)
        {
            try
            {
                result.EnsureSuccess();

                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return false;
        }

        /// <summary>
        /// Expand matrix to full storage (symmetric matrix stores upper part only).
        /// </summary>
        private static void Symmetrize(ref SparseMatrix A)
        {
            // Transpose A.
            var T = A.Transpose();

            // Remove diagonal.
            T.Keep((i, j, a) => i != j);

            A = (SparseMatrix)A.Add(T);
        }
    }
}
