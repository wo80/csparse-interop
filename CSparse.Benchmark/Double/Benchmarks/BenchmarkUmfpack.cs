
namespace CSparse.Double.Benchmarks
{
    using CSparse.Benchmark;
    using CSparse.Double.Factorization.SuiteSparse;
    using CSparse.Factorization;
    using CSparse.Interop.SuiteSparse.Umfpack;
    using CSparse.Storage;

    class BenchmarkUmfpack : Benchmark<double>
    {
        public BenchmarkUmfpack(MatrixFileCollection collection)
            :  base(collection)
        {
        }
        
        protected override IDisposableSolver<double> CreateSolver(CompressedColumnStorage<double> matrix, bool symmetric)
        {
            var solver = new Umfpack((SparseMatrix)matrix);

            if (symmetric)
            {
                solver.Control.Strategy = UmfpackStrategy.Symmetric;
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
