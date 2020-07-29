
namespace CSparse.Complex.Tests
{
    using CSparse.Complex.Factorization.SuiteSparse;
    using CSparse.Factorization;
    using CSparse.Interop.SuiteSparse.Umfpack;
    using System.Numerics;

    class TestUmfpack : Test
    {
        public TestUmfpack()
            : base("UMFPACK")
        {
        }

        protected override IDisposableSolver<Complex> CreateSolver(SparseMatrix matrix, bool symmetric)
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
