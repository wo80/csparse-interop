namespace CSparse.Complex.Examples
{
    using CSparse.Complex;
    using CSparse.Complex.Solver;
    using System;
    using System.Numerics;

    /// <summary>
    /// Examples taken from ARPACK++.
    /// </summary>
    static class TestArpack
    {
        public static void Run()
        {
            LCompReg();
            LCompShf();
            LCompGRe();
            LCompGSh();
        }

        /// <summary>
        /// Example program that illustrates how to solve a complex standard eigenvalue
        /// problem in regular mode.
        /// </summary>
        /// <remarks>
        /// MODULE LCompReg.cc
        /// 
        /// In this example we try to solve A*x = x*lambda in regular mode, where A is obtained
        /// from the standard central difference discretization of the convection-diffusion
        /// operator
        ///                (Laplacian u) + rho*(du / dx)
        ///                
        /// on the unit square [0,1]x[0,1] with zero Dirichlet boundary conditions.
        /// </remarks>
        static void LCompReg()
        {
            int nx = 10; // Dimension of the problem nx * nx.

            // Creating a complex matrix.
            var A = Generate.CompMatrixA(nx);

            // Defining what we need: the four eigenvectors of A with largest magnitude.
            var dprob = new Arpack(A) { ComputeEigenVectors = true };

            // Finding eigenvalues and eigenvectors.
            var result = dprob.SolveStandard(2, "LM");

            // Printing solution.
            Solution.Print(A, (ArpackResult)result, false);
        }

        /// <summary>
        /// Example program that illustrates how to solve a complex standard eigenvalue
        /// problem in shift and invert mode.
        /// </summary>
        /// <remarks>
        /// MODULE LCompShf.cc
        /// 
        /// In this example we try to solve A*x = x*lambda in shift and invert mode, where
        /// A is derived from the central difference discretization of the 1-dimensional
        /// convection-diffusion operator
        /// 
        ///                     (d^2u/dx^2) + rho*(du/dx)
        ///                     
        /// on the interval [0,1] with zero Dirichlet boundary conditions.
        /// </remarks>
        static void LCompShf()
        {
            int n = 100; // Dimension of the problem.

            Complex rho = 10.0;

            // Creating a complex matrix.
            var A = Generate.CompMatrixB(n, rho);

            // Defining what we need: the four eigenvectors of F nearest to 0.0.
            var dprob = new Arpack(A) { ComputeEigenVectors = true };

            // Finding eigenvalues and eigenvectors.
            var result = dprob.SolveStandard(4, new Complex(0.0, 0.0));

            // Printing solution.
            Solution.Print(A, (ArpackResult)result, true);
        }

        /// <summary>
        /// Example program that illustrates how to solve a complex generalized eigenvalue
        /// problem in regular mode.
        /// </summary>
        /// <remarks>
        /// MODULE LCompGRe.cc
        /// 
        /// In this example we try to solve A*x = B*x*lambda in regular mode, where A and B are
        /// derived from the finite element discretization of the 1-dimensional convection-diffusion
        /// operator
        ///                    (d^2u/dx^2) + rho*(du/dx)
        ///                    
        /// on the interval [0,1], with zero boundary conditions, using piecewise linear elements.
        /// </remarks>
        static void LCompGRe()
        {
            int n = 100; // Dimension of the problem.

            Complex rho = 10.0;

            // Creating complex matrices A and B.
            var A = Generate.CompMatrixE(n, rho);
            var B = Generate.CompMatrixF(n);

            // Defining what we need: the four eigenvectors with largest magnitude.
            var dprob = new Arpack(A, B) { ComputeEigenVectors = true };

            // Finding eigenvalues and eigenvectors.
            var result = dprob.SolveGeneralized(4, "LM");

            // Printing solution.
            Solution.Print(A, B, (ArpackResult)result, false);
        }

        /// <summary>
        /// Example program that illustrates how to solve a complex generalized eigenvalue
        /// problem in shift and invert mode.
        /// </summary>
        /// <remarks>
        /// MODULE LCompGSh.cc
        /// 
        /// In this example we try to solve A*x = B*x*lambda in shift and invert mode, where A and B
        /// are derived from a finite element discretization of a 1-dimensional convection-diffusion
        /// operator
        ///                      (d^2u/dx^2) + rho*(du/dx)
        ///                      
        /// on the interval [0,1], with zero boundary conditions, using piecewise linear elements.
        /// </remarks>
        static void LCompGSh()
        {
            int n = 100; // Dimension of the problem.

            Complex rho = 10.0;

            // Creating complex matrices A and B.
            var A = Generate.CompMatrixE(n, rho);
            var B = Generate.CompMatrixF(n);

            // Defining what we need: the four eigenvectors nearest to sigma.

            var dprob = new Arpack(A, B) { ComputeEigenVectors = true };

            // Finding eigenvalues and eigenvectors.
            var result = dprob.SolveGeneralized(4, new Complex(1.0, 0.0));

            // Printing solution.
            Solution.Print(A, B, (ArpackResult)result, true);
        }
        
        /// <summary>
        /// MODULE LCompSol.h - print information about eigenvalue problems.
        /// </summary>
        static class Solution
        {
            /// <summary>
            /// Prints eigenvalues and eigenvectors of complex eigen-problems.
            /// </summary>
            public static void Print(SparseMatrix A, ArpackResult result, bool shift)
            {
                int n = A.RowCount;
                int nconv = result.ConvergedEigenvalues;

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
            public static void Print(SparseMatrix A, SparseMatrix B, ArpackResult result, bool shift)
            {
                int n = A.RowCount;
                int nconv = result.ConvergedEigenvalues;

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
}
