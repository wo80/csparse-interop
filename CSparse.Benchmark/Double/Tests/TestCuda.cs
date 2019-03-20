
namespace CSparse.Double.Tests
{
    using CSparse.Double.Factorization;
    using CSparse.Factorization;
    using CSparse.Interop.CUDA;
    using System;
    using System.Diagnostics;
    using System.Globalization;

    static class TestCuda
    {
        private const double ERROR_THRESHOLD = 1e-3;

        public static void Run(int size, double density)
        {
            try
            {
                Console.WriteLine("Running CUDA tests (Double) ... [N = {0}]", size);
                Console.WriteLine();

                var A = Generate.Random(size, size, density);
                var B = Generate.RandomSymmetric(size, density, true);

                // Initialize CUDA device.
                Cuda.Initialize();

                RunCudaTest(A, B);

                Console.WriteLine();
            }
            catch (CudaException e)
            {
                Console.WriteLine(e.Result);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public static void ReportGpuTime(double seconds)
        {
            var color = Console.ForegroundColor;

            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write("   GPU factorization time: ");

            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine(string.Format(CultureInfo.InvariantCulture, "{0:0.000s} ", seconds));

            Console.ForegroundColor = color;
        }

        private static void RunCudaTest(SparseMatrix A, SparseMatrix B)
        {
            // NOTE: for real symmetric matrices, there's no need to transpose the storage, so
            //       we call the solver constructor with 3rd argument transpose = false.

            using (var stream = new CudaStream())
            using (var solver = new CudaCholesky(stream, B, false))
            {
                TestRandomSymmetric(solver, B, "CUDA Cholesky");

                ReportGpuTime(solver.FactorizationTime);
            }

            using (var stream = new CudaStream())
            using (var solver = new CudaQR(stream, A))
            {
                TestRandom(solver, A, "CUDA QR");

                ReportGpuTime(solver.FactorizationTime);
            }

            using (var stream = new CudaStream())
            using (var solver = new CudaQR(stream, B, false))
            {
                TestRandomSymmetric(solver, B, "CUDA QR");

                ReportGpuTime(solver.FactorizationTime);
            }
        }

        private static void TestRandom(IDisposableSolver<double> solver, SparseMatrix matrix, string name)
        {
            Console.Write("Testing {0} ... ", name);

            RunTest(solver, (SparseMatrix)matrix.Clone(), false);
        }

        private static void TestRandomSymmetric(IDisposableSolver<double> solver, SparseMatrix matrix, string name)
        {
            Console.Write("Testing {0} (symmetric) ... ", name);

            RunTest(solver, (SparseMatrix)matrix.Clone(), true);
        }

        private static void RunTest(IDisposableSolver<double> solver, SparseMatrix A, bool symmetric)
        {
            int n = A.RowCount;

            var x = Vector.Create(n, 1.0);
            var b = Vector.Create(n, 0.0);
            var s = Vector.Clone(x);

            A.Multiply(x, b);

            Vector.Clear(x);

            var timer = Stopwatch.StartNew();

            try
            {
                solver.Solve(b, x);

                timer.Stop();

                Display.Time(timer.ElapsedTicks);

                double error = Helper.ComputeError(x, s);

                if (double.IsNaN(error))
                {
                    Display.Warning("solver failed");
                }
                else if (error > ERROR_THRESHOLD)
                {
                    Display.Warning("relative error too large");
                }
                else
                {
                    Display.Ok("OK");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
