
namespace CSparse.Double.Tests
{
    using CSparse.Double.Factorization;
    using CSparse.Factorization;
    using CSparse.Interop.Umfpack;

    class TestUmfpack : Test
    {
        public TestUmfpack()
            : base("UMFPACK")
        {
        }

        protected override IDisposableSolver<double> CreateSolver(SparseMatrix matrix, bool symmetric)
        {
            var solver = new Umfpack(matrix);

            if (symmetric)
            {
                var options = solver.Control;

                options.Strategy = UmfpackStrategy.Symmetric;
            }

            return solver;
        }
    }
}
