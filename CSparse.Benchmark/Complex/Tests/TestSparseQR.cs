
namespace CSparse.Complex.Tests
{
    using CSparse.Complex.Factorization;
    using CSparse.Factorization;
    using System.Numerics;

    class TestSparseQR : Test
    {
        public TestSparseQR()
            : base("CSparse.NET QR")
        {
        }

        protected override IDisposableSolver<Complex> CreateSolver(SparseMatrix matrix, bool symmetric)
        {
            return new DisposableSparseQR(matrix);
        }
    }

    class DisposableSparseQR : IDisposableSolver<Complex>
    {
        SparseQR qr;

        public DisposableSparseQR(SparseMatrix matrix)
        {
            qr = SparseQR.Create(matrix, ColumnOrdering.MinimumDegreeAtA);
        }

        public void Solve(Complex[] input, Complex[] result)
        {
            qr.Solve(input, result);
        }

        public void Dispose()
        {
        }
    }
}
