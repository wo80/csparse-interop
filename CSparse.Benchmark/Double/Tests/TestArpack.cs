
namespace CSparse.Double.Tests
{
    using CSparse.Double.Solver;
    using CSparse.Interop.ARPACK;
    using CSparse.Solvers;
    using System;
    using System.Diagnostics;

    class TestArpack
    {
        private const double ERROR_THRESHOLD = 1e-3;

        public void Run(int size)
        {
            Console.Write("Testing ARPACK ... ");

            var timer = new Stopwatch();

            // Number of eigenvalues to compute.
            int k = 5;

            // Exact eigenvalues.
            var z = new double[k];

            size = (int)Math.Sqrt(size) + 1;

            var A = CSparse.Double.Examples.Generate.SymmetricMatrixC(size);
            var B = CSparse.Double.Examples.Generate.SymmetricMatrixD(size);
            //var A = (SparseMatrix)Generate.Laplacian(size, size, z);
            //var U = (SparseMatrix)A.Clone();

            // For real symmetric problems, ARPACK++ expects the matrix to be upper triangular.
            //U.Keep((i, j, _) => i <= j);
            
            //var solver = new Arpack(U, true)
            var solver = new Arpack(A, B, true)
            {
                Tolerance = 1e-6,
                ComputeEigenVectors = true
            };
            
            try
            {
                timer.Start();

                //var result = solver.SolveStandard(k, 0.0);
                //var result = solver.SolveStandard(k, Spectrum.SmallestMagnitude);

                //var result = solver.SolveStandard(k, 8.0);
                //var result = solver.SolveStandard(k, Spectrum.LargestMagnitude);
                var result = solver.SolveGeneralized(k, Spectrum.LargestMagnitude);

                timer.Stop();

                Display.Time(timer.ElapsedTicks);
                
                result.EnsureSuccess();

                if (CheckResiduals(A.Expand(), B.Expand(), result, true))
                {
                    Display.Ok("OK");
                }
                else
                {
                    Display.Warning("residual error too large");
                }
            }
            catch (DllNotFoundException)
            {
                throw;
            }
            catch (Exception e)
            {
                Display.Error(e.Message);
            }
        }

        private static bool CheckResiduals(SparseMatrix A, IEigenSolverResult result, bool print)
        {
            var evals = result.EigenValuesReal();
            var evecs = result.EigenVectorsReal();
            
            return Helper.Residuals(A, result.ConvergedEigenValues, evals, evecs, print);
        }

        private static bool CheckResiduals(SparseMatrix A, SparseMatrix B, IEigenSolverResult result, bool print)
        {
            var evals = result.EigenValuesReal();
            var evecs = result.EigenVectorsReal();

            return Helper.Residuals(A, B, result.ConvergedEigenValues, evals, evecs, print);
        }
    }
}
