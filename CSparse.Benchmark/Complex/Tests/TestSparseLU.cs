
namespace CSparse.Complex.Tests
{
    using CSparse.Complex.Factorization;
    using CSparse.Factorization;
    using System.Numerics;

    class TestSparseLU : Test
    {
        public TestSparseLU()
            : base("CSparse.NET LU")
        {
        }

        protected override IDisposableSolver<Complex> CreateSolver(SparseMatrix matrix, bool symmetric)
        {
            return new DisposableSparseLU(matrix);
        }
    }

    class DisposableSparseLU : IDisposableSolver<Complex>
    {
        SparseLU lu;

        public DisposableSparseLU(SparseMatrix matrix)
        {
            lu = SparseLU.Create(matrix, ColumnOrdering.MinimumDegreeAtPlusA, 0.1);
        }

        public void Solve(Complex[] input, Complex[] result)
        {
            lu.Solve(input, result);
        }

        public void Dispose()
        {
        }
    }
}
