
namespace CSparse.Double.Benchmarks
{
    using CSparse.Benchmark;
    using CSparse.Double.Factorization;
    using CSparse.Factorization;
    using CSparse.Interop.SuperLU;
    using CSparse.Storage;

    class BenchmarkSuperLU : Benchmark<double>
    {
        public BenchmarkSuperLU(MatrixFileCollection collection)
            :  base(collection)
        {
        }
        
        protected override IDisposableSolver<double> CreateSolver(CompressedColumnStorage<double> matrix, bool symmetric)
        {
            var solver = new SuperLU((SparseMatrix)matrix);

            if (symmetric)
            {
                var options = solver.Options;

                options.SymmetricMode = true;
                options.ColumnOrderingMethod = OrderingMethod.MinimumDegreeAtPlusA;
                options.DiagonalPivotThreshold = 0.001;
            }

            return solver;
        }

        protected override double[] CreateTestVector(int size)
        {
            return Vector.Create(size, 1.0);
        }

        protected override double ComputeError(double[] actual, double[] expected)
        {
            return Helper.ComputeError(actual, expected);
        }

        private double ComputeResidual(CompressedColumnStorage<double> A, double[] x, double[] b)
        {
            return Helper.ComputeResidual(A, x, b);
        }
    }
}
