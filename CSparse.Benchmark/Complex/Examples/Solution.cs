namespace CSparse.Complex.Examples
{
    using CSparse.Complex;
    using CSparse.Solvers;
    using System;

    using Complex = System.Numerics.Complex;

    /// <summary>
    /// MODULE LCompSol.h - print information about eigenvalue problems.
    /// </summary>
    static class Solution
    {
        /// <summary>
        /// Prints eigenvalues and eigenvectors of complex eigen-problems.
        /// </summary>
        public static void Print(SparseMatrix A, IEigenSolverResult result, bool shift)
        {
            int n = A.RowCount;
            int nconv = result.ConvergedEigenValues;

            Console.WriteLine();
            Console.WriteLine("Testing ARPACK++ class ARluCompStdEig");
            Console.WriteLine("Complex eigenvalue problem: A*x - lambda*x");
            Console.WriteLine(shift ? "Shift and invert mode" : "Regular mode");
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

                    Vector.Copy(x, y);

                    // y = A*x - lambda*x
                    A.Multiply(1.0, x, -lambda, y);

                    r[i] = Vector.Norm(y) / Complex.Abs(lambda);
                }

                for (int i = 0; i < nconv; i++)
                {
                    Console.WriteLine("||A*x(" + (i + 1) + ") - lambda(" + (i + 1) + ")*x(" + (i + 1) + ")||: " + r[i]);
                }

                Console.WriteLine();
            }
        }

        /// <summary>
        /// Prints eigenvalues and eigenvectors of complex generalized eigen-problems.
        /// </summary>
        public static void Print(SparseMatrix A, SparseMatrix B, IEigenSolverResult result, bool shift)
        {
            int n = A.RowCount;
            int nconv = result.ConvergedEigenValues;

            Console.WriteLine();
            Console.WriteLine("Testing ARPACK++ class ARluCompGenEig");
            Console.WriteLine("Complex generalized eigenvalue problem: A*x - lambda*B*x");
            Console.WriteLine(shift ? "Shift and invert mode" : "Regular mode");
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

                    Vector.Copy(x, y);

                    // y = B*x
                    B.Multiply(x, y);

                    // y = A*x - lambda*B*x
                    A.Multiply(1.0, x, -lambda, y);

                    r[i] = Vector.Norm(y) / Complex.Abs(lambda);
                }

                for (int i = 0; i < nconv; i++)
                {
                    Console.WriteLine("||A*x(" + (i + 1) + ") - lambda(" + (i + 1) + ")*B*x(" + (i + 1) + ")||: " + r[i]);
                }

                Console.WriteLine();
            }
        }
    }
}
