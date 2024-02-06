
namespace CSparse.Interop.Tests.Double
{
    using CSparse.Double;
    using CSparse.Double.Factorization.SuiteSparse;
    using CSparse.Factorization;

    class TestSPQR : TestBase
    {
        public TestSPQR()
            : base("SPQR")
        {
        }

        protected override void TestRandomSymmetric(SparseMatrix matrix)
        {
        }

        protected override IDisposableSolver<double> CreateSolver(SparseMatrix matrix, bool symmetric)
        {
            var solver = new SPQR(matrix);

            if (symmetric)
            {
            }

            return solver;
        }
    }
}
