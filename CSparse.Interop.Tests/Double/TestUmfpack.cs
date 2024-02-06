
namespace CSparse.Interop.Tests.Double
{
    using CSparse.Double;
    using CSparse.Double.Factorization.SuiteSparse;
    using CSparse.Factorization;
    using CSparse.Interop.SuiteSparse.Umfpack;

    class TestUmfpack : TestBase
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
