
namespace CSparse.Double
{
    using CSparse.Benchmark;
    using CSparse.Double.Factorization;
    using CSparse.Interop.Umfpack;
    using CSparse.Storage;

    class BenchmarkUmfpack : Benchmark<Umfpack, double>
    {
        public BenchmarkUmfpack(MatrixFileCollection collection)
            :  base(collection)
        {
        }
        
        protected override Umfpack CreateSolver(CompressedColumnStorage<double> matrix, bool symmetric)
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
            return Util.ComputeError(actual, expected);
        }

        private double ComputeResidual(CompressedColumnStorage<double> A, double[] x, double[] b)
        {
            return Util.ComputeResidual(A, x, b);
        }
    }
}
