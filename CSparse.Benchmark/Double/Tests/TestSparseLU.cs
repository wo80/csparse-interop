
namespace CSparse.Double.Tests
{
    using CSparse.Double.Factorization;
    using CSparse.Factorization;

    class TestSparseLU : Test
    {
        public TestSparseLU()
            : base("CSparse.NET LU")
        {
        }

        protected override IDisposableSolver<double> CreateSolver(SparseMatrix matrix, bool symmetric)
        {
            return new DisposableSparseLU(matrix);
        }
    }

    class DisposableSparseLU : IDisposableSolver<double>
    {
        SparseLU lu;

        public DisposableSparseLU(SparseMatrix matrix)
        {
            lu = SparseLU.Create(matrix, ColumnOrdering.MinimumDegreeAtPlusA, 0.1);
        }

        public void Solve(double[] input, double[] result)
        {
            lu.Solve(input, result);
        }

        public void Dispose()
        {
        }
    }
}
