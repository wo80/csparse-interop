namespace CSparse.Double.Tests.EigenSolvers
{
    using CSparse.Double.Solver;
    using System;
    using System.Diagnostics;

    class TestSpectra
    {
        public void Run(int size)
        {
            Console.Write("Testing Spectra ... ");

            // Number of eigenvalues to compute.
            int k = 5;

            // Exact eigenvalues.
            var z = new double[k];

            size = (int)Math.Sqrt(size) + 1;

            var A = (SparseMatrix)Generate.Laplacian(size, size, z);

            try
            {
                Run(A, k, true);
            }
            catch (Exception e)
            {
                Display.Error(e.Message);
            }
        }

        public void Run(SparseMatrix A, int m, bool symmetric)
        {
            var solver = new Spectra(A, symmetric)
            {
                Tolerance = 1e-6,
                ComputeEigenVectors = true,
                ArnoldiCount = m * 3
            };

            var timer = Stopwatch.StartNew();

            var result = solver.SolveStandard(m, 0.0);
            //var result = solver.SolveStandard(m, Spectrum.SmallestMagnitude);

            //var result = solver.SolveStandard(m, 8.0);
            //var result = solver.SolveStandard(m, Spectrum.LargestMagnitude);

            timer.Stop();

            Display.Time(timer.ElapsedTicks);

            result.EnsureSuccess();

            if (Helper.CheckResiduals(A, result, symmetric, false))
            {
                Display.Ok("OK");
            }
            else
            {
                Display.Warning("residual error too large");
            }
        }
    }
}
