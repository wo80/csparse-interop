
namespace CSparse.Benchmark
{
    using CSparse.Factorization;
    using CSparse.IO;
    using CSparse.Storage;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;

    public abstract class Benchmark<S, T>
        where S : IDisposable, ISolver<T>
        where T : struct, IEquatable<T>, IFormattable
    {
        private MatrixFileCollection collection;

        protected Benchmark(MatrixFileCollection collection)
        {
            this.collection = collection;
        }

        public void Run(IBenchmarkResultWriter export)
        {
            var timer = new Stopwatch();

            // Running symmetric tests.
            var results = new List<BenchmarkResult>();

            foreach (var file in collection.Get(true))
            {
                Run(file, timer, results);
            }

            export.Add("symmetric", results);

            // Running non-symmetric tests.
            results = new List<BenchmarkResult>();

            foreach (var file in collection.Get(false))
            {
                Run(file, timer, results);
            }

            export.Add("general", results);
        }

        private void Run(MatrixFile file, Stopwatch timer, List<BenchmarkResult> results)
        {
            string name = Path.GetFileName(file.Path);

            if (!File.Exists(file.Path))
            {
                results.Add(new BenchmarkResult(file, "File not found."));

                Console.WriteLine("File not found: {0}", Path.GetFullPath(file.Path));

                return;
            }

            try
            {
                var A = MatrixMarketReader.ReadMatrix<T>(file.Path);

                int columns = A.ColumnCount;

                var x = CreateTestVector(columns);
                var s = CreateTestVector(columns);
                var b = new T[A.RowCount];

                A.Multiply(x, b);

                Array.Clear(x, 0, columns);

                var info = new BenchmarkResult(file, A.RowCount, columns, GetNonZerosCount(A));

                info.Time = Solve(A, b, x, file.Symmetric, timer);

                info.Residual = ComputeError(x, s);

                results.Add(info);
            }
            catch (Exception e)
            {
                results.Add(new BenchmarkResult(file, e.Message));
            }
        }

        private double Solve(CompressedColumnStorage<T> matrix, T[] input, T[] x, bool symmetric, Stopwatch timer)
        {
            // Make a copy of the data (the solver might modify it).
            var A = matrix.Clone();
            var b = (T[])input.Clone();

            timer.Restart();

            using (var solver = CreateSolver(A, symmetric))
            {
                solver.Solve(b, x);
            }

            timer.Stop();

            return timer.Elapsed.TotalMilliseconds;
        }

        private int GetNonZerosCount(Matrix<T> matrix)
        {
            return ((CompressedColumnStorage<T>)matrix).NonZerosCount;
        }

        protected abstract T[] CreateTestVector(int size);

        protected abstract S CreateSolver(CompressedColumnStorage<T> matrix, bool symmetric);

        protected abstract double ComputeError(T[] actual, T[] expected);
    }
}
