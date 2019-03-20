
namespace CSparse.Double.Tests
{
    using CSparse.Double.Solver;
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
            
            var A = (SparseMatrix)Generate.Laplacian(size, size, z);

            // For real symmetric problems, ARPACK++ expects the matrix to be upper triangular.
            var U = A.ToUpper();
            
            var solver = new Arpack(U, true)
            {
                Tolerance = 1e-6,
                ComputeEigenVectors = true
            };
            
            try
            {
                timer.Start();

                var result = solver.SolveStandard(k, 0.0);
                //var result = solver.SolveStandard(k, Spectrum.SmallestMagnitude);

                //var result = solver.SolveStandard(k, 8.0);
                //var result = solver.SolveStandard(k, Spectrum.LargestMagnitude);

                timer.Stop();

                Display.Time(timer.ElapsedTicks);
                
                result.EnsureSuccess();

                if (Helper.CheckResiduals(A, result, true))
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
    }
}
