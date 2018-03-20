
namespace CSparse.Complex
{
    using CSparse.Factorization;
    using System;
    using System.Diagnostics;
    using System.Globalization;
    using System.Numerics;

    abstract class Test
    {
        private const double ERROR_THRESHOLD = 1e-3;

        private string name;

        private Stopwatch timer = new Stopwatch();

        public Test(string name)
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
                Error(e.Message);
            }
        }

        protected virtual void TestRandom(SparseMatrix matrix)
        {
            Console.Write("Testing {0} ... ", name);

            var A = (SparseMatrix)matrix.Clone();

            int n = A.RowCount;

            var x = Vector.Create(n, 1.0);
            var b = Vector.Create(n, 0.0);
            var s = Vector.Clone(x);

            A.Multiply(x, b);

            Vector.Clear(x);

            try
            {
                timer.Restart();

                using (var solver = CreateSolver(A, false))
                {
                    solver.Solve(b, x);
                }

                timer.Stop();

                Time(timer.ElapsedMilliseconds);

                double error = Util.ComputeError(x, s);

                if (error > ERROR_THRESHOLD)
                {
                    Warning("relative error too large");
                }
                else
                {
                    Ok("OK");
                }
            }
            catch (DllNotFoundException)
            {
                throw;
            }
            catch (Exception e)
            {
                Error(e.Message);
            }
        }

        protected virtual void TestRandomSymmetric(SparseMatrix matrix)
        {
            Console.Write("Testing {0} (symmetric) ... ", name);

            var A = (SparseMatrix)matrix.Clone();

            int n = A.RowCount;

            var x = Vector.Create(n, 1.0);
            var b = Vector.Create(n, 0.0);
            var s = Vector.Clone(x);

            A.Multiply(x, b);

            Vector.Clear(x);

            try
            {
                timer.Restart();

                using (var solver = CreateSolver(A, true))
                {
                    solver.Solve(b, x);
                }

                timer.Stop();

                Time(timer.ElapsedMilliseconds);

                double error = Util.ComputeError(x, s);

                if (error > ERROR_THRESHOLD)
                {
                    Warning("relative error too large");
                }
                else
                {
                    Ok("OK");
                }
            }
            catch (DllNotFoundException)
            {
                throw;
            }
            catch (Exception e)
            {
                Error(e.Message);
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

                Time(timer.ElapsedMilliseconds);

                double error = 0.0;

                for (int i = 0; i < count; i++)
                {
                    X.Column(i, x);
                    S.Column(i, s);

                    error += Util.ComputeError(x, s);
                }
                
                if (error / count > ERROR_THRESHOLD)
                {
                    Warning("relative error too large");
                }
                else
                {
                    Ok("OK");
                }
            }
            catch (DllNotFoundException)
            {
                throw;
            }
            catch (Exception e)
            {
                Error(e.Message);
            }
        }

        protected abstract IDisposableSolver<Complex> CreateSolver(SparseMatrix matrix, bool symmetric);

        private void Time(long ms)
        {
            var color = Console.ForegroundColor;

            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write(string.Format(CultureInfo.InvariantCulture, "[{0:0.0s}] ", ms / 1000.0));
            Console.ForegroundColor = color;
        }

        private void Ok(string message)
        {
            var color = Console.ForegroundColor;

            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine(message);
            Console.ForegroundColor = color;
        }

        private void Warning(string message)
        {
            var color = Console.ForegroundColor;

            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine(message);
            Console.ForegroundColor = color;
        }

        private void Error(string message)
        {
            var color = Console.ForegroundColor;

            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine(message);
            Console.ForegroundColor = color;
        }
    }
}
