
namespace CSparse.Double.Tests
{
    using CSparse.Double.Factorization;
    using CSparse.Factorization;
    using System;

    class TestSparseQR : Test
    {
        public TestSparseQR()
            : base("CSparse.NET QR")
        {
        }

        protected override void TestRandomSymmetric(SparseMatrix matrix)
        {
        }

        protected override IDisposableSolver<double> CreateSolver(SparseMatrix matrix, bool symmetric)
        {
            return new DisposableSparseQR(matrix);
        }
    }

    class DisposableSparseQR : IDisposableSolver<double>
    {
        SparseQR qr;

        public DisposableSparseQR(SparseMatrix matrix)
        {
            qr = SparseQR.Create(matrix, ColumnOrdering.MinimumDegreeAtA);
        }

        public void Solve(double[] input, double[] result)
        {
            qr.Solve(input, result);
        }

        public void Solve(ReadOnlySpan<double> input, Span<double> result)
        {
            qr.Solve(input, result);
        }

        public void Dispose()
        {
        }
    }
}
