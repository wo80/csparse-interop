
namespace CSparse.Complex.Tests
{
    using CSparse.Complex.Solver;
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

            int N = A.RowCount;

            var solver = new Arpack(A, true)
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
                
                if (Helper.CheckResiduals(A, result, false))
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
