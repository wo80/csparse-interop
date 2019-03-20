
namespace CSparse.Double.Tests
{
    using CSparse.Double.Solver;
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

                var result = (ExtendedEigensolverResult)solver.SolveStandard(k0, Interop.MKL.Job.Largest);

                timer.Stop();

                Display.Time(timer.ElapsedTicks);

                if (result.Status == Interop.MKL.SparseStatus.Success)
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

        private static bool CheckResiduals(SparseMatrix A, ExtendedEigensolverResult result, bool print)
        {
            var evals = result.EigenValuesReal();
            var evecs = result.EigenVectorsReal();

            return Helper.CheckResiduals(A, result.ConvergedEigenvalues, evals, evecs, print);
        }
    }
}
