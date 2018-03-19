
namespace CSparse.Double
{
    using CSparse.Double.Factorization;
    using CSparse.Factorization;
    using CSparse.Interop.SPQR;

    class TestSPQR : Test
    {
        public TestSPQR()
            : base("SPQR")
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
