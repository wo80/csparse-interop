
namespace CSparse.Double.Tests
{
    using CSparse.Double.Solver;
    using CSparse.Interop.ARPACK;
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
            var U = (SparseMatrix)A.Clone();

            // For real symmetric problems, ARPACK++ expects the matrix to be upper triangular.
            U.Keep((i, j, _) => i <= j);
            
            var solver = new Arpack(U, true)
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

        private static bool CheckResiduals(SparseMatrix A, ArpackResult<double> result, bool print)
        {
            int N = A.RowCount;

            var m = result.ConvergedEigenvalues;

            var v = result.EigenValuesReal();
            var X = result.EigenVectorsReal();

            if (print)
            {
                Console.WriteLine();
                Console.WriteLine("       Lambda         Residual");
            }

            var x = new double[N];
            var y = new double[N];

            bool ok = true;

            for (int i = 0; i < m; i++)
            {
                var lambda = v[i];

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
