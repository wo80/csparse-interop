
namespace CSparse.Complex.Tests
{
    using CSparse.Factorization;
    using System;
    using System.Diagnostics;

    using Complex = System.Numerics.Complex;

    abstract class TestBase
    {
        private const double ERROR_THRESHOLD = 1e-3;

        private string name;

        private Stopwatch timer = new Stopwatch();

        public TestBase(string name)
        {
            this.name = name;
        }

        public void Run(SparseMatrix A, SparseMatrix B)
        {
            try
            {
                TestRandom(A);
                TestRandomSymmetric(B);
                //TestRandomMulti(A);
            }
            catch (Exception e)
            {
                Display.Error(e.Message);
            }
        }

        protected virtual void TestRandom(SparseMatrix matrix)
        {
            Console.Write("Testing {0} ... ", name);

            RunTest((SparseMatrix)matrix.Clone(), false);
        }

        protected virtual void TestRandomSymmetric(SparseMatrix matrix)
        {
            Console.Write("Testing {0} (symmetric) ... ", name);

            RunTest((SparseMatrix)matrix.Clone(), true);
        }

        protected virtual void RunTest(SparseMatrix A, bool symmetric)
        {
            int n = A.RowCount;

            var x = Vector.Create(n, 1.0);
            var b = Vector.Create(n, 0.0);
            var s = Vector.Clone(x);

            A.Multiply(x, b);

            Vector.Clear(x);

            try
            {
                timer.Restart();

                using (var solver = CreateSolver(A, symmetric))
                {
                    solver.Solve(b, x);
                }

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
            catch (DllNotFoundException)
            {
                throw;
            }
            catch (Exception e)
            {
                Display.Error(e.Message);
            }
        }

        protected virtual void TestRandomMulti(SparseMatrix matrix)
        {
            Console.Write("Testing {0} (multi) ... ", name);

            var A = (SparseMatrix)matrix.Clone();

            int count = 3;

            int n = A.RowCount;

            var b = Vector.Create(n, 0.0);
            var x = Vector.Create(n, 0.0);
            var s = Vector.Create(n, 0.0);

            var X = new DenseMatrix(n, count);
            var S = new DenseMatrix(n, count);
            var B = new DenseMatrix(n, count);

            for (int i = 0; i < count; i++)
            {
                x = Vector.Create(n, i + 1);

                X.SetColumn(i, x);
                S.SetColumn(i, x);

                A.Multiply(x, b);

                B.SetColumn(i, b);
            }

            X.Clear();

            try
            {
                timer.Restart();

                using (var solver = CreateSolver(A, false))
                {
                    //solver.Solve(B, X);
                }

                timer.Stop();

                Display.Time(timer.ElapsedTicks);

                double error = 0.0;

                for (int i = 0; i < count; i++)
                {
                    X.Column(i, x);
                    S.Column(i, s);

                    error += Helper.ComputeError(x, s);
                }
                
                if (error / count > ERROR_THRESHOLD)
                {
                    Display.Warning("relative error too large");
                }
                else
                {
                    Display.Ok("OK");
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

        protected abstract IDisposableSolver<Complex> CreateSolver(SparseMatrix matrix, bool symmetric);
    }
}
