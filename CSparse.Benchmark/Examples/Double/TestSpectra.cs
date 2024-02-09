namespace CSparse.Examples.Double
{
    using CSparse.Double.Solver;
    using CSparse.Solvers;

    /// <summary>
    /// Examples taken from ARPACK++ and applied to Spectra.
    /// </summary>
    static class TestSpectra
    {
        public static void Run()
        {
            Symmetric();
            General();
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
            //LNSymGRe();
            //LNSymGSh();
            //LNSymGSC();
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
            var A = Generate.SymmetricMatrixA(nx).Expand();

            // Defining what we need: the four eigenvectors of A with smallest magnitude.
            var prob = new Spectra(A, true) { ComputeEigenVectors = true };

            // Finding eigenvalues and eigenvectors.
            var result = prob.SolveStandard(2, Spectrum.SmallestMagnitude);

            // Printing solution.
            Solution.Symmetric("Spectra", A, result,  ShiftMode.None);
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
            var A = Generate.SymmetricMatrixB(n).Expand();

            // Defining what we need: the four eigenvectors of A nearest to 1.0.
            var prob = new Spectra(A, true) { ComputeEigenVectors = true };

            // Finding eigenvalues and eigenvectors.
            var result = prob.SolveStandard(4, 1.0);

            // Printing solution.
            Solution.Symmetric("Spectra", A, result, ShiftMode.Regular);
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
            var A = Generate.SymmetricMatrixC(n).Expand();
            var B = Generate.SymmetricMatrixD(n).Expand();

            // Defining what we need: the four eigenvectors with largest magnitude.
            var prob = new Spectra(A, B, true) { ComputeEigenVectors = true };

            // Finding eigenvalues and eigenvectors.
            var result = prob.SolveGeneralized(4, Spectrum.LargestMagnitude);

            // Printing solution.
            Solution.Symmetric("Spectra", A, B, result, ShiftMode.None);
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
            var A = Generate.SymmetricMatrixC(n).Expand();
            var B = Generate.SymmetricMatrixD(n).Expand();

            // Defining what we need: the four eigenvectors nearest to 0.0.
            var prob = new Spectra(A, B, true) { ComputeEigenVectors = true };

            // Finding eigenvalues and eigenvectors.
            var result = prob.SolveGeneralized(4, 0.0, ShiftMode.Regular);

            // Printing solution.
            Solution.Symmetric("Spectra", A, B, result, ShiftMode.Regular);
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
            var A = Generate.SymmetricMatrixC(n).Expand();
            var B = Generate.SymmetricMatrixD(n).Expand();

            // Defining what we need: the four eigenvectors nearest to 1.0.
            var prob = new Spectra(A, B, true) { ComputeEigenVectors = true };

            // Finding eigenvalues and eigenvectors.
            var result = prob.SolveGeneralized(4, 1.0, ShiftMode.Buckling);

            // Printing solution.
            Solution.Symmetric("Spectra", A, B, result, ShiftMode.Buckling);
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
            var A = Generate.SymmetricMatrixC(n).Expand();
            var B = Generate.SymmetricMatrixD(n).Expand();

            // Defining what we need: the four eigenvectors nearest to 150.0.
            var prob = new Spectra(A, B, true) { ComputeEigenVectors = true };

            // Finding eigenvalues and eigenvectors.
            var result = prob.SolveGeneralized(4, 150.0, ShiftMode.Cayley);

            // Printing solution.
            Solution.Symmetric("Spectra", A, B, result, ShiftMode.Cayley);
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
            Solution.General("Spectra", A, result, false);
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
            Solution.General("Spectra", A, result, true);
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
            Solution.General("Spectra", A, B, result, false);
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
            Solution.General("Spectra", A, B, result, true);
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
            Solution.General(A, B, result, true, true);
            //*/
        }

        #endregion
    }
}
