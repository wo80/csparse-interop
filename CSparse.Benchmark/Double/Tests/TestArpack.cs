﻿
namespace CSparse.Double.Tests
{
    using CSparse.Double.Solver;
    using System;
    using System.Diagnostics;

    class TestArpack
    {
        private const double ERROR_THRESHOLD = 1e-3;

        public void Run(int size)
        {
            Console.Write("Testing ARPACK ... ");

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
            // For real symmetric problems, ARPACK++ expects the matrix to be upper triangular.
            var U = A.ToUpper();

            var solver = new Arpack(U, symmetric)
            {
                Tolerance = 1e-6,
                ComputeEigenVectors = true
            };

            var timer = Stopwatch.StartNew();

            var result = solver.SolveStandard(m, 0.0);
            //var result = solver.SolveStandard(m, Spectrum.SmallestMagnitude);

            //var result = solver.SolveStandard(m, 8.0);
            //var result = solver.SolveStandard(m, Spectrum.LargestMagnitude);

            timer.Stop();

            Display.Time(timer.ElapsedTicks);

            result.EnsureSuccess();

            if (Helper.CheckResiduals(A, result, symmetric, true))
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
