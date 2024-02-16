namespace CSparse.Examples
{
    using CSparse.Double;
    using CSparse.Solvers;
    using CSparse.Storage;
    using System;

    using CVector = CSparse.Complex.Vector;
    using CComplex = System.Numerics.Complex;

    /// <summary>
    /// Print information about eigenvalue problems.
    /// </summary>
    static class Solution
    {
        public static bool Verbose = false;

        #region Output

        internal static void PrintHeader(string name, int size, IEigenSolverResult result, bool complex, bool symmetric, bool generalized, ShiftMode mode, bool complexShift = false)
        {
            Console.Write("Test {0} [{1}, {2}, {3}, mode: ", name, complex ? "complex" : "real", symmetric ? "sym" : "non-sym", generalized ? "gen" : "std");

            switch (mode)
            {
                case ShiftMode.None:
                    Console.Write("reg");
                    break;
                case ShiftMode.Regular:
                    Console.Write("inv");
                    break;
                case ShiftMode.Buckling:
                    Console.Write("buk");
                    break;
                case ShiftMode.Cayley:
                    Console.Write("cay");
                    break;
            }

            Console.Write(complexShift ? " imag] " : "] ");

            if (Verbose)
            {
                Console.WriteLine();
                Console.WriteLine("Dimension of the system            : " + size);
                Console.WriteLine("Number of requested eigenvalues    : " + result.Count);
                Console.WriteLine("Number of converged eigenvalues    : " + result.ConvergedEigenValues);
                Console.WriteLine("Number of Arnoldi vectors generated: " + result.ArnoldiCount);
                Console.WriteLine("Number of iterations taken         : " + result.IterationsTaken);
                Console.WriteLine();
            }
        }

        internal static void PrintEigenvalues(IEigenSolverResult result, bool symmetric)
        {
            Console.WriteLine("Eigenvalues:");

            if (symmetric)
            {
                var evals = result.EigenValuesReal();

                for (int i = 0; i < result.ConvergedEigenValues; i++)
                {
                    Console.WriteLine("  lambda[" + (i + 1) + "]: " + evals[i]);
                }
            }
            else
            {
                var evals = result.EigenValues;

                for (int i = 0; i < result.ConvergedEigenValues; i++)
                {
                    Console.WriteLine("  lambda[" + (i + 1) + "]: " + evals[i]);
                }
            }

            Console.WriteLine();
        }

        private static void PrintResiduals(CompressedColumnStorage<double> A, CompressedColumnStorage<double> B, bool symmetric, IEigenSolverResult result)
        {
            if (!result.HasEigenVectors)
            {
                return;
            }

            if (symmetric)
            {
                A = A.Expand();

                if (B != null)
                {
                    B = B.Expand();
                }
            }

            int n = A.RowCount;
            int nconv = result.ConvergedEigenValues;

            // Printing the residual norm || A*x - lambda*B*x ||
            // for the nconv accurately computed eigenvectors.

            var r = new double[nconv]; // residuals

            if (symmetric)
            {
                var evals = result.EigenValuesReal();
                var evecs = result.EigenVectorsReal();

                var x = new double[n];
                var y = new double[n];

                for (int i = 0; i < nconv; i++)
                {
                    var lambda = evals[i];

                    evecs.Column(i, x);

                    Vector.Copy(x, y);

                    if (B != null)
                    {
                        // y = B*x
                        B.Multiply(x, y);
                    }

                    // y = A*x - lambda*x
                    A.Multiply(1.0, x, -lambda, y);

                    r[i] = Vector.Norm(n, y) / Math.Abs(lambda);
                }
            }
            else
            {
                var evals = result.EigenValues;
                var evecs = result.EigenVectors;

                var x = new CComplex[n];
                var y = new CComplex[n];

                for (int i = 0; i < nconv; i++)
                {
                    var lambda = evals[i];

                    evecs.Column(i, x);

                    CVector.Copy(x, y);

                    if (B != null)
                    {
                        // y = B*x
                        B.Multiply(x, y);
                    }

                    // y = A*x - lambda*B*x
                    A.Multiply(1.0, x, -lambda, y);

                    r[i] = CVector.Norm(n, y) / CComplex.Abs(lambda);
                }
            }

            for (int i = 0; i < nconv; i++)
            {
                Console.WriteLine("||A*x(" + (i + 1) + ") - lambda(" + (i + 1) + ")*x(" + (i + 1) + ")||: " + r[i]);
            }

            Console.WriteLine();
        }

        private static void PrintResiduals(CompressedColumnStorage<CComplex> A, CompressedColumnStorage<CComplex> B, bool symmetric, IEigenSolverResult result)
        {
            if (!result.HasEigenVectors)
            {
                return;
            }

            if (symmetric)
            {
                A = A.Expand();

                if (B != null)
                {
                    B = B.Expand();
                }
            }

            int n = A.RowCount;
            int nconv = result.ConvergedEigenValues;

            // Printing the residual norm || A*x - lambda*B*x ||
            // for the nconv accurately computed eigenvectors.

            var r = new double[nconv]; // residuals

            var evals = result.EigenValues;
            var evecs = result.EigenVectors;

            var x = new CComplex[n];
            var y = new CComplex[n];

            for (int i = 0; i < nconv; i++)
            {
                var lambda = evals[i];

                evecs.Column(i, x);

                CVector.Copy(x, y);

                if (B != null)
                {
                    // y = B*x
                    B.Multiply(x, y);
                }

                // y = A*x - lambda*B*x
                A.Multiply(1.0, x, -lambda, y);

                r[i] = CVector.Norm(n, y) / CComplex.Abs(lambda);
            }

            for (int i = 0; i < nconv; i++)
            {
                Console.WriteLine("||A*x(" + (i + 1) + ") - lambda(" + (i + 1) + ")*x(" + (i + 1) + ")||: " + r[i]);
            }

            Console.WriteLine();
        }

        #endregion

        /// <summary>
        /// Prints eigenvalues and eigenvectors of symmetric eigen-problems.
        /// </summary>
        public static void Symmetric(string name, CompressedColumnStorage<double> A, IEigenSolverResult result, ShiftMode mode)
        {
            Symmetric(name, A, null, result, mode);
        }

        /// <summary>
        /// Prints eigenvalues and eigenvectors of symmetric generalized eigen-problems.
        /// </summary>
        public static void Symmetric(string name, CompressedColumnStorage<double> A, CompressedColumnStorage<double> B, IEigenSolverResult result, ShiftMode mode)
        {
            PrintHeader(name, A.RowCount, result, false, true, B != null, mode);

            if (!EnsureSuccess(result))
            {
                return;
            }

            if (Verbose)
            {
                PrintEigenvalues(result, true);
                PrintResiduals(A, B, true, result);
            }
        }

        /// <summary>
        /// Prints eigenvalues and eigenvectors of non-symmetric eigen-problems.
        /// </summary>
        public static void General(string name, CompressedColumnStorage<double> A, IEigenSolverResult result, bool shift)
        {
            General(name, A, null, result, shift, false);
        }

        /// <summary>
        /// Prints eigenvalues and eigenvectors of non-symmetric generalized eigen-problems.
        /// </summary>
        public static void General(string name, CompressedColumnStorage<double> A, CompressedColumnStorage<double> B, IEigenSolverResult result, bool shift, bool complex = false)
        {
            PrintHeader(name, A.RowCount, result, false, false, B != null, shift ? ShiftMode.Regular : ShiftMode.None, complex);

            if (!EnsureSuccess(result))
            {
                return;
            }

            if (Verbose)
            {
                PrintEigenvalues(result, false);
                PrintResiduals(A, B, false, result);
            }
        }

        /// <summary>
        /// Prints eigenvalues and eigenvectors of non-symmetric eigen-problems.
        /// </summary>
        public static void Print(string name, CompressedColumnStorage<CComplex> A, IEigenSolverResult result, bool shift)
        {
            Print(name, A, null, result, shift);
        }

        /// <summary>
        /// Prints eigenvalues and eigenvectors of non-symmetric generalized eigen-problems.
        /// </summary>
        public static void Print(string name, CompressedColumnStorage<CComplex> A, CompressedColumnStorage<CComplex> B, IEigenSolverResult result, bool shift)
        {
            PrintHeader(name, A.RowCount, result, true, false, B != null, shift ? ShiftMode.Regular : ShiftMode.None);

            if (!EnsureSuccess(result))
            {
                return;
            }

            if (Verbose)
            {
                PrintEigenvalues(result, false);
                PrintResiduals(A, B, false, result);
            }
        }

        #region SVD

        /// <summary>
        /// Prints singular values and condition number.
        /// </summary>
        public static void Condition(CompressedColumnStorage<double> A, IEigenSolverResult result)
        {
            Console.Write("Test ARPACK [real, svd, full] ");

            if (!EnsureSuccess(result))
            {
                return;
            }

            if (Verbose)
            {
                int m = A.RowCount;
                int n = A.ColumnCount;
                int nconv = result.ConvergedEigenValues;

                Console.WriteLine();
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
        }

        /// <summary>
        /// Prints singular values and vectors.
        /// </summary>
        public static void SVD(CompressedColumnStorage<double> A, IEigenSolverResult result)
        {
            Console.Write("Test ARPACK [real, svd, part] ");

            if (!EnsureSuccess(result))
            {
                return;
            }

            if (Verbose)
            {
                int m = A.RowCount;
                int n = A.ColumnCount;
                int nconv = result.ConvergedEigenValues;

                Console.WriteLine();
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
        }

        #endregion

        private static bool EnsureSuccess(IEigenSolverResult result)
        {
            try
            {
                result.EnsureSuccess();

                var color = Console.ForegroundColor;

                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine("OK");
                Console.ForegroundColor = color;

                return true;
            }
            catch (Exception e)
            {
                var color = Console.ForegroundColor;

                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("solver failed");
                Console.ForegroundColor = color;

                Console.WriteLine(e.Message);
            }

            return false;
        }
    }
}
