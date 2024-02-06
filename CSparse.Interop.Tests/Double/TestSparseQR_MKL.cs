
namespace CSparse.Interop.Tests.Double
{
    using CSparse.Double;
    using CSparse.Double.Factorization.MKL;
    using CSparse.Factorization;
    using CSparse.Storage;

    class TestSparseQR_MKL : TestBase
    {
        public TestSparseQR_MKL()
            : base("SparseQR MKL")
        {
        }

        protected override void TestRandomSymmetric(SparseMatrix matrix)
        {
        }

        protected override IDisposableSolver<double> CreateSolver(SparseMatrix matrix, bool symmetric)
        {
            // MKL sparse QR expects CSR storage, so we have to transpose the matrix.
            var solver = new SparseQR(new CompressedRowStorage<double>(matrix));

            if (symmetric)
            {
            }

            return solver;
        }
    }
}
