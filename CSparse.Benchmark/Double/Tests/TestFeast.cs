
namespace CSparse.Double.Tests
{
    using CSparse.Double.Solver;
    using CSparse.Interop.Feast;
    using System;
    using System.Diagnostics;

    class TestFeast
    {
        private const double ERROR_THRESHOLD = 1e-3;

        public void Run()
        {
            Console.Write("Testing FEAST ... ");

            var timer = new Stopwatch();

            // Initial subspace dimension.
            int m0 = 25;

            // Exact eigenvalues.
            var z = new double[m0];

            var A = (SparseMatrix)Generate.Laplacian(50, 50, z);

            int N = A.RowCount;

            var solver = new Feast(A);

            //solver.Options.PrintStatus = true;

            // The current version of MKL (2018.1) uses FEAST 2.0. One of the difficulties is
            // to guess a correct size for the initial subspace dimension, so all eigenvalues
            // of the requested interval can be computed (the latest version of  FEAST offers
            // a method to estimate the number of eigenvalues inside of the search interval).

            try
            {
                timer.Start();

                var result = solver.SolveStandard(m0, 0.0, 0.1); // interval [0.0, 0.2] would fail

                timer.Stop();

                Display.Time(timer.ElapsedTicks);

                if (result.RelativeTraceError > ERROR_THRESHOLD)
                {
                    Display.Warning("relative error too large");
                }
                else if (result.Status == 0)
                {
                    Display.Ok("OK");
                }
                else if (result.Status == 3)
                {
                    // If the subspace guess is too small, status will be 3 and either
                    // the dimension must be increased or the search interval reduced.
                    Display.Warning("subspace size too small");
                }
                else
                {
                    Display.Warning("status = " + result.Status);
                }

                //PrintResiduals(A, result);
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

        private static void PrintResiduals(SparseMatrix A, FeastResult<double> result)
        {
            int N = A.RowCount;

            var m = result.ConvergedEigenvalues;

            var v = result.EigenValues;
            var X = result.EigenVectors;

            Console.WriteLine();
            Console.WriteLine("       Lambda         Residual");

            var x = new double[N];
            var y = new double[N];

            for (int i = 0; i < m; i++)
            {
                var lambda = v[i].Real;

                X.Column(i, x);

                Vector.Copy(x, y);

                // y = A*x - lambda*x
                A.Multiply(1.0, x, -lambda, y);

                Console.WriteLine("{0,3}:   {1,10:0.00000000}   {2,10:0.00e+00}", i, lambda, Vector.Norm(y));
            }
        }
    }
}
