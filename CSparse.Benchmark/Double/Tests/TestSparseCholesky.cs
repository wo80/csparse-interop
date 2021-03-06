﻿
namespace CSparse.Double.Tests
{
    using CSparse.Double.Factorization;
    using CSparse.Factorization;

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

        public DisposableSparseCholesky(SparseMatrix matrix)
        {
            cholesky = SparseCholesky.Create(matrix, ColumnOrdering.MinimumDegreeAtPlusA);
        }

        public void Solve(double[] input, double[] result)
        {
            cholesky.Solve(input, result);
        }

        public void Dispose()
        {
        }
    }
}
