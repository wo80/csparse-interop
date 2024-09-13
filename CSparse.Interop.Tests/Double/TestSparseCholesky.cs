
namespace CSparse.Interop.Tests.Double
{
    using CSparse.Double;
    using CSparse.Double.Factorization;
    using CSparse.Factorization;
    using System;

    class TestSparseCholesky : TestBase
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

        protected override IDisposableSolver<double> CreateSolver(SparseMatrix matrix, bool symmetric)
        {
            if (!symmetric)
            {
                // throw
            }

            return new DisposableSparseCholesky(matrix);
        }
    }

    class DisposableSparseCholesky : IDisposableSolver<double>
    {
        SparseCholesky cholesky;

        public int NonZerosCount => cholesky.NonZerosCount;

        public DisposableSparseCholesky(SparseMatrix matrix)
        {
            cholesky = SparseCholesky.Create(matrix, ColumnOrdering.MinimumDegreeAtPlusA);
        }

        public void Solve(double[] input, double[] result)
        {
            cholesky.Solve(input, result);
        }

        public void Solve(ReadOnlySpan<double> input, Span<double> result)
        {
            cholesky.Solve(input, result);
        }

        public void Dispose()
        {
        }
    }
}
