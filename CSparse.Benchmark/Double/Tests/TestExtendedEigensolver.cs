
namespace CSparse.Double.Tests
{
    using CSparse.Double.Solver;
    using CSparse.Interop.MKL;
    using System;
    using System.Diagnostics;

    class TestExtendedEigensolver
    {
        private const double ERROR_THRESHOLD = 1e-3;

        public void Run(int size)
        {
            Console.Write("Testing MKL Extended Eigensolver ... ");

            var timer = new Stopwatch();

            // Initial subspace dimension.
            int k0 = 5;

            // Exact eigenvalues.
            var z = new double[k0];

            size = (int)Math.Sqrt(size) + 1;

            var A = (SparseMatrix)Generate.Laplacian(size, size, z);

            int N = A.RowCount;

            var solver = new ExtendedEigensolver(A);
            
            try
            {
                timer.Start();

                var result = solver.SolveStandard(k0, Job.Smallest);

                timer.Stop();

                Display.Time(timer.ElapsedTicks);

                if (result.Status == SparseStatus.Success)
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
            catch (DllNotFoundException)
            {
                throw;
            }
            catch (Exception e)
            {
                Display.Error(e.Message);
            }
        }

        private static bool CheckResiduals(SparseMatrix A, ExtendedEigensolverResult<double> result, bool print)
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

            var x = new double[N];
            var y = new double[N];

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
