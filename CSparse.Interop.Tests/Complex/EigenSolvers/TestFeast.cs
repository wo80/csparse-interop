namespace CSparse.Interop.Tests.Complex.EigenSolvers
{
    using CSparse.Complex;
    using CSparse.Complex.Solver;
    using CSparse.Interop.MKL.Feast;
    using System;
    using System.Diagnostics;

    using Complex = System.Numerics.Complex;

    class TestFeast
    {
        private const double ERROR_THRESHOLD = 1e-3;

        public void Run(int size)
        {
            Console.Write("Testing FEAST ... ");

            // Initial subspace dimension.
            int m0 = 25;

            // Exact eigenvalues.
            var z = new double[m0];

            size = (int)Math.Sqrt(size) + 1;

            var A = (SparseMatrix)Generate.Laplacian(size, size, z);

            try
            {
                Run(A, m0, true);
            }
            catch (Exception e)
            {
                Display.Error(e.Message);
            }
        }

        public void Run(SparseMatrix A, int m, bool symmetric)
        {
            var solver = new Feast(A);

            //solver.Options.PrintStatus = true;

            // The current version of MKL (2018.1) uses FEAST 2.0. One of the difficulties is
            // to guess a correct size for the initial subspace dimension, so all eigenvalues
            // of the requested interval can be computed (the latest version of  FEAST offers
            // a method to estimate the number of eigenvalues inside of the search interval).

            var timer = Stopwatch.StartNew();

            var result = solver.SolveStandard(m, 0.0, 0.1); // interval [0.0, 0.2] would fail

            timer.Stop();

            Display.Time(timer.ElapsedTicks);

            if (result.RelativeTraceError > ERROR_THRESHOLD)
            {
                Display.Warning("relative error too large");
            }
            else if (result.Status == 3)
            {
                // If the subspace guess is too small, status will be 3 and either
                // the dimension must be increased or the search interval reduced.
                Display.Warning("subspace size too small");
            }
            else if (result.Status == 0)
            {
                if (CheckResiduals(A, result, false))
                {
                    Display.Ok("OK");
                }
                else
                {
                    Display.Warning("residual error too large");
                }
            }
            else
            {
                Display.Warning("status = " + result.Status);
            }
        }

        private static bool CheckResiduals(SparseMatrix A, FeastResult<Complex> result, bool print)
        {
            int n = A.RowCount;

            var m = result.ConvergedEigenvalues;

            var v = result.EigenValues;
            var X = result.EigenVectors;

            if (print)
            {
                Console.WriteLine();
                Console.WriteLine("       Lambda         Residual");
            }

            var x = new Complex[n];
            var y = new Complex[n];

            bool ok = true;

            for (int i = 0; i < m; i++)
            {
                var lambda = v[i].Real;

                X.Column(i, x);

                Vector.Copy(x, y);

                // y = A*x - lambda*x
                A.Multiply(1.0, x, -lambda, y);

                double r = Vector.Norm(n, y);

                if (r > ERROR_THRESHOLD)
                {
                    ok = false;
                }

                if (print)
                {
                    Console.WriteLine("{0,3}:   {1,10:0.00000000}   {2,10:0.00e+00}", i, lambda, r);
                }
            }

            return ok;
        }
    }
}
