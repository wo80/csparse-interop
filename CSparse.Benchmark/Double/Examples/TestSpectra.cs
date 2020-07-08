namespace CSparse.Double.Examples
{
    using CSparse.Double;
    using CSparse.Double.Solver;
    using CSparse.Interop.Spectra;
    using CSparse.Solvers;
    using System;

    using ShiftMode = CSparse.Interop.ARPACK.ShiftMode;

    using CVector = CSparse.Complex.Vector;
    using Complex = System.Numerics.Complex;

    /// <summary>
    /// Examples taken from ARPACK++ and applied to Spectra.
    /// </summary>
    static class TestSpectra
    {
        public static void Run()
        {
            Symmetric();
            //General();
        }

        public static void Symmetric()
        {
            LSymReg();
            LSymShf();
            LSymGReg();
            LSymGShf();
            LSymGBkl();
            LSymGCay();
        }

        public static void General()
        {
            LNSymReg();
            LNSymShf();
            LNSymGRe();
            LNSymGSh();
            LNSymGSC();
        }

        #region Symmetric

        /// <summary>
        /// Example program that illustrates how to solve a real symmetric standard eigenvalue
        /// problem in regular mode.
        /// </summary>
        /// <remarks>
        /// MODULE LSymReg.cc
        /// 
        /// In this example we try to solve A*x = x*lambda in regular  mode, where A is derived
        /// from the standard central difference  discretization of the 2-dimensional Laplacian
        /// on the unit square with zero Dirichlet boundary conditions.
        /// </remarks>
        static void LSymReg()
        {
            int nx = 10;

            // Creating a 100x100 matrix.
            var A = Symmetrize(Generate.SymmetricMatrixA(nx));

            // Defining what we need: the four eigenvectors of A with smallest magnitude.
            var prob = new Spectra(A, true) { ComputeEigenVectors = true };

            // Finding eigenvalues and eigenvectors.
            var result = prob.SolveStandard(2, Spectrum.SmallestMagnitude);

            // Printing solution.
            Solution.Symmetric(A, (SpectraResult)result, false);
        }

        /// <summary>
        /// Example program that illustrates how to solve a real symmetric standard eigenvalue
        /// problem in shift and invert mode.
        /// </summary>
        /// <remarks>
        /// MODULE LSymShf.cc
        /// 
        /// In this example we try to solve A*x = x*lambda in shift and invert mode, where A is
        /// derived from the central difference discretization of the one-dimensional Laplacian
        /// on [0, 1] with zero Dirichlet boundary conditions.
        /// 
        /// The SuperLU package is called to solve some linear systems involving (A-sigma*I).
        /// This is needed to implement the shift and invert strategy.
        /// </remarks>
        static void LSymShf()
        {
            int n = 100; // Dimension of the problem.

            // Creating a 100x100 matrix.
            var A = Symmetrize(Generate.SymmetricMatrixB(n));

            // Defining what we need: the four eigenvectors of A nearest to 1.0.
            var prob = new Spectra(A, true) { ComputeEigenVectors = true };

            // Finding eigenvalues and eigenvectors.
            var result = prob.SolveStandard(4, 1.0);

            // Printing solution.
            Solution.Symmetric(A, (SpectraResult)result, true);
        }

        /// <summary>
        /// Example program that illustrates how to solve a real symmetric generalized eigenvalue
        /// problem in regular mode.
        /// </summary>
        /// <remarks>
        /// MODULE LSymGReg.cc
        /// 
        /// In this example we try to solve A*x = B*x*lambda in regular mode, where A and B are obtained
        /// from the finite element discretization of the 1-dimensional discrete Laplacian
        ///                               d^2u / dx^2
        /// on the interval [0,1] with zero Dirichlet boundary conditions using piecewise linear elements.
        /// 
        /// The SuperLU package is called to solve some linear systems involving B.
        /// </remarks>
        static void LSymGReg()
        {
            int n = 100; // Dimension of the problem.

            // Creating matrices A and B.
            var A = Symmetrize(Generate.SymmetricMatrixC(n));
            var B = Symmetrize(Generate.SymmetricMatrixD(n));

            // Defining what we need: the four eigenvectors with largest magnitude.
            var prob = new Spectra(A, B, true) { ComputeEigenVectors = true };

            // Finding eigenvalues and eigenvectors.
            var result = prob.SolveGeneralized(4, Spectrum.LargestMagnitude);

            // Printing solution.
            Solution.Symmetric(A, B, (SpectraResult)result, ShiftMode.None);
        }

        /// <summary>
        /// Example program that illustrates how to solve a real symmetric generalized eigenvalue
        /// problem in shift and invert mode.
        /// </summary>
        /// <remarks>
        /// MODULE LSymGShf.cc
        /// 
        /// In this example we try to solve A*x = B*x*lambda in shift and invert mode, where A and B are
        /// obtained from the finite element discretization of the 1-dimensional discrete Laplacian
        ///                               d^2u / dx^2
        /// on the interval [0,1] with zero Dirichlet boundary conditions using piecewise linear elements.
        /// 
        /// The SuperLU package is called to solve some linear systems involving (A-sigma*B).
        /// </remarks>
        static void LSymGShf()
        {
            int n = 100; // Dimension of the problem.

            // Creating matrices A and B.
            var A = Symmetrize(Generate.SymmetricMatrixC(n));
            var B = Symmetrize(Generate.SymmetricMatrixD(n));

            // Defining what we need: the four eigenvectors nearest to 0.0.
            var prob = new Spectra(A, B, true) { ComputeEigenVectors = true };

            // Finding eigenvalues and eigenvectors.
            var result = prob.SolveGeneralized(4, 0.0, ShiftMode.Regular);

            // Printing solution.
            Solution.Symmetric(A, B, (SpectraResult)result, ShiftMode.Regular);
        }

        /// <summary>
        /// Example program that illustrates how to solve a real symmetric generalized eigenvalue
        /// problem in buckling mode.
        /// </summary>
        /// <remarks>
        /// MODULE LSymGBkl.cc
        /// 
        /// In this example we try to solve A*x = B*x*lambda in buckling mode, where A and B are obtained
        /// from the finite element discretization of the 1-dimensional discrete Laplacian
        ///                               d^2u / dx^2
        /// on the interval [0,1] with zero Dirichlet boundary conditions using piecewise linear elements.
        /// 
        /// The SuperLU package is called to solve some linear systems involving (A-sigma*B).
        /// </remarks>
        static void LSymGBkl()
        {
            int n = 100; // Dimension of the problem.

            // Creating matrices A and B.
            var A = Symmetrize(Generate.SymmetricMatrixC(n));
            var B = Symmetrize(Generate.SymmetricMatrixD(n));

            // Defining what we need: the four eigenvectors nearest to 1.0.
            var prob = new Spectra(A, B, true) { ComputeEigenVectors = true };

            // Finding eigenvalues and eigenvectors.
            var result = prob.SolveGeneralized(4, 1.0, ShiftMode.Buckling);

            // Printing solution.
            Solution.Symmetric(A, B, (SpectraResult)result, ShiftMode.Buckling);
        }

        /// <summary>
        /// Example program that illustrates how to solve a real symmetric generalized eigenvalue
        /// problem in Cayley mode.
        /// </summary>
        /// <remarks>
        /// MODULE LSymGCay.cc
        /// 
        /// In this example we try to solve A*x = B*x*lambda in Cayley mode, where A and B are obtained
        /// from the finite element discretization of the 1-dimensional discrete Laplacian
        ///                               d^2u / dx^2
        ///  on the interval [0,1] with zero Dirichlet boundary conditions using piecewise linear elements.
        ///
        /// The SuperLU package is called to solve some linear systems involving (A-sigma*B).
        /// </remarks>
        static void LSymGCay()
        {
            int n = 100; // Dimension of the problem.

            // Creating matrices A and B.
            var A = Symmetrize(Generate.SymmetricMatrixC(n));
            var B = Symmetrize(Generate.SymmetricMatrixD(n));

            // Defining what we need: the four eigenvectors nearest to 150.0.
            var prob = new Spectra(A, B, true) { ComputeEigenVectors = true };

            // Finding eigenvalues and eigenvectors.
            var result = prob.SolveGeneralized(4, 150.0, ShiftMode.Cayley);

            // Printing solution.
            Solution.Symmetric(A, B, (SpectraResult)result, ShiftMode.Cayley);
        }

        #endregion

        #region General

        /// <summary>
        /// Example program that illustrates how to solve a real nonsymmetric standard eigenvalue
        /// problem in regular mode.
        /// </summary>
        /// <remarks>
        /// MODULE LNSymReg.cc
        /// 
        /// In this example we try to solve A*x = x*lambda in regular mode, where A is derived from
        /// the standard central difference discretization of the 2-dimensional convection-diffusion
        /// operator
        ///                    (Laplacian u) + rho*(du/dx)
        /// on a unit square with zero Dirichlet boundary conditions.
        /// </remarks>
        static void LNSymReg()
        {
            int nx = 10;

            // Creating a 100x100 matrix.
            var A = Generate.BlockTridMatrix(nx);

            // Defining what we need: the four eigenvectors of A with largest magnitude.
            var prob = new Spectra(A) { ComputeEigenVectors = true };

            // Finding eigenvalues and eigenvectors.
            var result = prob.SolveStandard(4, Spectrum.LargestMagnitude);

            // Printing solution.
            Solution.General(A, (SpectraResult)result, false);
        }

        /// <summary>
        /// Example program that illustrates how to solve a real nonsymmetric standard eigenvalue
        /// problem in shift and invert mode.
        /// </summary>
        /// <remarks>
        /// MODULE LNSymShf.cc
        /// 
        /// In this example we try to solve A*x = x*lambda in shift and invert mode, where A is
        /// derived from 2-D Brusselator Wave Model. The shift is a real number.
        /// 
        /// The SuperLU package is called to solve some linear systems involving (A-sigma*I).
        /// </remarks>
        static void LNSymShf()
        {
            int n = 200; // Dimension of the problem.

            // Creating a 200x200 matrix.
            var A = Generate.BrusselatorMatrix(1.0, 0.004, 0.008, 2.0, 5.45, n);

            // Defining what we need: the four eigenvectors of BWM nearest to 0.0.
            var prob = new Spectra(A) { ComputeEigenVectors = true, ArnoldiCount = 30 };

            // Finding eigenvalues and eigenvectors.
            var result = prob.SolveStandard(4, 0.0, Spectrum.LargestMagnitude);

            // Printing solution.
            Solution.General(A, (SpectraResult)result, true);
        }

        /// <summary>
        /// Example program that illustrates how to solve a real nonsymmetric generalized eigenvalue
        /// problem in regular mode.
        /// </summary>
        /// <remarks>
        /// MODULE LNSymGRe.cc
        /// 
        /// In this example we try to solve A*x = B*x*lambda in regular mode, where A and B are derived
        /// from the finite element discretization of the 1-dimensional convection-diffusion operator
        ///                     (d^2u / dx^2) + rho*(du/dx)
        /// on the interval [0,1] with zero Dirichlet boundary conditions using linear elements.
        /// 
        /// The SuperLU package is called to solve some linear systems involving B.
        /// </remarks>
        static void LNSymGRe()
        {
            int n = 100; // Dimension of the problem.
            double rho = 10.0; // A parameter used in StiffnessMatrix.

            // Creating matrices A and B.
            var A = Generate.StiffnessMatrix(n, rho);
            var B = Generate.MassMatrix(n);

            // Defining what we need: the four eigenvectors with largest magnitude.
            var prob = new Spectra(A, B) { ComputeEigenVectors = true };

            // Finding eigenvalues and eigenvectors.
            var result = prob.SolveGeneralized(4, Spectrum.LargestMagnitude);

            // Printing solution.
            Solution.General(A, B, (SpectraResult)result, false);
        }

        /// <summary>
        /// Example program that illustrates how to solve a real nonsymmetric generalized eigenvalue
        /// problem in real shift and invert mode.
        /// </summary>
        /// <remarks>
        /// MODULE LNSymGSh.cc
        /// 
        /// In this example we try to solve A*x = B*x*lambda in shift and inverse mode, where A and B
        /// are derived from the finite element discretization of the 1-dimensional convection-diffusion
        /// operator
        ///                     (d^2u / dx^2) + rho*(du/dx)
        /// on the interval [0,1] with zero Dirichlet boundary conditions using linear elements.
        /// The shift sigma is a real number.
        /// 
        /// The SuperLU package is called to solve some linear systems involving (A-sigma*B).
        /// </remarks>
        static void LNSymGSh()
        {
            int n = 100; // Dimension of the problem.
            double rho = 10.0; // A parameter used in StiffnessMatrix.

            // Creating matrices A and B.
            var A = Generate.StiffnessMatrix(n, rho);
            var B = Generate.MassMatrix(n);

            // Defining what we need: the four eigenvectors nearest to 1.0.
            var prob = new Spectra(A, B) { ComputeEigenVectors = true };

            // Finding eigenvalues and eigenvectors.
            var result = prob.SolveGeneralized(4, 1.0);

            // Printing solution.
            Solution.General(A, B, (SpectraResult)result, true);
        }

        /// <summary>
        /// Example program that illustrates how to solve a nonsymmetric generalized eigenvalue
        /// problem in complex shift and invert mode (taking the real part of OP*x).
        /// </summary>
        /// <remarks>
        /// MODULE LNSymGSC.cc
        /// 
        /// In this example we try to solve A*x = B*x*lambda in complex shift and inverse mode,
        /// where A is the tridiagonal matrix with 2 on the diagonal, -2 on the subdiagonal and
        /// 3 on the superdiagonal, and B is the tridiagonal matrix with 4 on the diagonal and
        /// 1 on the off-diagonals. The shift is a complex number.
        /// 
        /// The SuperLU package is called to solve some complex linear systems involving (A-sigma*B).
        /// </remarks>
        static void LNSymGSC()
        {
            int n = 100; // Dimension of the problem.

            // Creating matrices A and B.
            var A = Generate.NonSymMatrixE(n);
            var B = Generate.NonSymMatrixF(n);

            // Defining what we need: the four eigenvectors nearest to 0.4 + 0.6i.
            var prob = new Spectra(A, B) { ComputeEigenVectors = true };

            /*
            // Finding eigenvalues and eigenvectors.
            var result = prob.SolveGeneralized(4, 0.4, 0.6, 'R');

            // Printing solution.
            Solution.General(A, B, (SpectraResult)result, true, true);
            //*/
        }

        #endregion

        /// <summary>
        /// Expand matrix to full storage (symmetric matrix stores upper part only).
        /// </summary>
        private static SparseMatrix Symmetrize(SparseMatrix A)
        {
            // Transpose A.
            var T = A.Transpose();

            // Remove diagonal.
            T.Keep((i, j, a) => i != j);

            return (SparseMatrix)A.Add(T);
        }

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
            public static void Symmetric(SparseMatrix A, SpectraResult result, bool shift)
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

                        r[i] = Vector.Norm(y) / Math.Abs(lambda);
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
            public static void Symmetric(SparseMatrix A, SparseMatrix B, SpectraResult result, ShiftMode mode)
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

                        r[i] = Vector.Norm(y) / Math.Abs(lambda);
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
            public static void General(SparseMatrix A, SpectraResult result, bool shift)
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

                        r[i] = CVector.Norm(y) / Complex.Abs(lambda);
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
            public static void General(SparseMatrix A, SparseMatrix B, SpectraResult result, bool shift, bool cshift = false)
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

                        r[i] = CVector.Norm(y) / Complex.Abs(lambda);
                    }

                    for (int i = 0; i < nconv; i++)
                    {
                        Console.WriteLine("||A*x(" + i + ") - lambda(" + i + ")*B*x(" + i + ")||: " + r[i]);
                    }

                    Console.WriteLine();
                }
            }

            #endregion

            private static bool EnsureSuccess(SpectraResult result)
            {
                try
                {
                    result.EnsureSuccess();

                    return true;
                }
                catch (SpectraException e)
                {
                    Console.WriteLine(e.Message);
                }

                return false;
            }
        }
    }
}
