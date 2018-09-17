
namespace CSparse.Complex.Tests
{
    using CSparse.Complex.Solver;
    using CSparse.Interop.ARPACK;
    using System;
    using System.Diagnostics;
    using System.Numerics;

    class TestArpack
    {
        private const double ERROR_THRESHOLD = 1e-3;

        public void Run()
        {
            Console.Write("Testing ARPACK ... ");

            var timer = new Stopwatch();

            // Number of eigenvalues to compute.
            int k = 5;

            // Exact eigenvalues.
            var z = new double[k];

            var A = (SparseMatrix)Generate.Laplacian(50, 50, z);
            
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
                //var result = solver.SolveStandard(k, Job.SmallestMagnitude);

                //var result = solver.SolveStandard(k, 8.0);
                //var result = solver.SolveStandard(k, Job.LargestMagnitude);

                timer.Stop();

                Display.Time(timer.ElapsedTicks);

                result.EnsureSuccess();
                
                if (CheckResiduals(A, result, false))
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

        private static bool CheckResiduals(SparseMatrix A, ArpackResult<Complex> result, bool print)
        {
            int N = A.RowCount;

            var m = result.ConvergedEigenvalues;

            var v = result.EigenValues;
            var X = result.EigenVectors;

            if (print)
            {
                Console.WriteLine();
                Console.WriteLine("       Lambda         Residual");
            }

            var x = new Complex[N];
            var y = new Complex[N];

            bool ok = true;

            for (int i = 0; i < m; i++)
            {
                var lambda = v[i].Real;

                X.Column(i, x);

                Vector.Copy(x, y);

                // y = A*x - lambda*x
                A.Multiply(1.0, x, -lambda, y);

                double r = Vector.Norm(y);

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
