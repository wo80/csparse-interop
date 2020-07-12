
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

            // Initial subspace dimension.
            int k0 = 5;

            // Exact eigenvalues.
            var z = new double[k0];

            size = (int)Math.Sqrt(size) + 1;

            var A = (SparseMatrix)Generate.Laplacian(size, size, z);

            try
            {
                Run(A, k0, true);
            }
            catch (Exception e)
            {
                Display.Error(e.Message);
            }
        }

        public void Run(SparseMatrix A, int m, bool symmetric)
        {
            var solver = new ExtendedEigensolver(A);

            var timer = Stopwatch.StartNew();
            
            var result = (ExtendedEigensolverResult)solver.SolveStandard(m, Interop.MKL.Job.Largest);

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

        private static bool CheckResiduals(SparseMatrix A, ExtendedEigensolverResult result, bool print)
        {
            var evals = result.EigenValuesReal();
            var evecs = result.EigenVectorsReal();

            return Helper.CheckResiduals(A, result.ConvergedEigenValues, evals, evecs, print);
        }
    }
}
