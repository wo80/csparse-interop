
namespace CSparse.Complex.Tests
{
    using CSparse.Complex.Factorization;
    using CSparse.Factorization;
    using System.Numerics;

    class TestSparseCholesky : Test
    {
        public TestSparseCholesky()
            : base("CSparse.NET Cholesky")
        {
        }

        protected override void TestRandom(SparseMatrix matrix)
        {
        }

        protected override void TestRandomMulti(SparseMatrix matrix)
        {
        }

        protected override IDisposableSolver<Complex> CreateSolver(SparseMatrix matrix, bool symmetric)
        {
            if (!symmetric)
            {
                // throw
            }

            return new DisposableSparseCholesky(matrix);
        }
    }

    class DisposableSparseCholesky : IDisposableSolver<Complex>
    {
        SparseCholesky cholesky;

        public DisposableSparseCholesky(SparseMatrix matrix)
        {
            cholesky = SparseCholesky.Create(matrix, ColumnOrdering.MinimumDegreeAtPlusA);
        }

        public void Solve(Complex[] input, Complex[] result)
        {
            cholesky.Solve(input, result);
        }

        public void Dispose()
        {
        }
    }
}
